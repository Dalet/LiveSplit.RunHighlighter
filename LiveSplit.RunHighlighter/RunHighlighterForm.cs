using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Twitch = LiveSplit.RunHighlighter.TwitchExtension;

namespace LiveSplit.RunHighlighter
{
    public partial class RunHighlighterForm : Form
    {
        private RunHighlighterSettings _settings;
        private IList<RunHistory.Item> _runs;

        private dynamic _video;
        private HighlightInfo _highlightInfo;

        private DateTime _lastSearch;
        private int _lastRunSearched;

        public RunHighlighterForm(RunHighlighterSettings settings)
        {
            InitializeComponent();

            this.MaximumSize = new System.Drawing.Size(this.Width, Screen.AllScreens.Max(s => s.Bounds.Height));
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            this.ShowIcon = false;
            this._lastSearch = new DateTime(0);
            this._lastRunSearched = -1;
            this._settings = settings;

            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += " v" + ver.ToString(2) + (ver.Build > 0 ? "." + ver.Build : "");

            lstRunHistory.Items.Clear();

            this.Load += (s, e) =>
                {
                    var uiThread = SynchronizationContext.Current;
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        DialogResult result = DialogResult.Retry;
                        while ((_runs = RunHistory.GetRunHistory()) == null && result == DialogResult.Retry)
                        {
                            uiThread.Send(d => result = MessageBox.Show(this, RunHistory.LastException.Message, "Reading the history file failed", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error), null);
                        }

                        if (result == DialogResult.Cancel)
                            uiThread.Send(d => this.Close(), null);

                        Debug.WriteLine("Run History length: " + _runs.Count);

                        uiThread.Send(d => PopulateListbox(_runs), null);
                    });

                    if (!Twitch.Instance.IsLoggedIn)
                        Twitch.Instance.VerifyLogin();

                    if (!String.IsNullOrEmpty(_settings.TwitchUsername) && Twitch.IsValidUsername(_settings.TwitchUsername))
                        this.txtBoxTwitchUsername.Text = _settings.TwitchUsername;
                    else if (Twitch.Instance.IsLoggedIn)
                        this.txtBoxTwitchUsername.Text = Twitch.Instance.ChannelName;
                };

            this.FormClosed += Form_OnClose;
        }

        void Form_OnClose(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBoxTwitchUsername.Text))
                _settings.TwitchUsername = txtBoxTwitchUsername.Text;
        }

        void PopulateListbox(IList<RunHistory.Item> runs)
        {
            lstRunHistory.Items.Clear();
            lstRunHistory.Enabled = true;

            if (runs == null || runs.Count <= 0)
            {
                var message = runs != null ? "(empty)" : "(retrieving the history failed)";
                lstRunHistory.Items.Add(message);
                lstRunHistory.Enabled = false;
                txtBoxTwitchUsername.Enabled = false;
            }
            else
                lstRunHistory.Items.AddRange(RunHistoryToString(runs).ToArray());
        }

        static IList<string> RunHistoryToString(IEnumerable<RunHistory.Item> runs)
        {
            var runHistory = new List<string>();
            Func<string, string, int, string> AppendString = (src, c, number) =>
            {
                for (int i = 0; number > i; i++)
                    src += c;
                return src;
            };

            int maxSemiColumn = runs.Max(r => r.TimeString.Count(c => c == ':'));
            int maxNumbers = runs.Max(r => r.TimeString.Count(c => c != ':'));

            foreach (var run in runs)
            {
                var timeDesc = run.TimeString;

                var timeDescSpaces = AppendString("", " ", maxSemiColumn - timeDesc.Count(c => c == ':'));
                timeDescSpaces = AppendString(timeDescSpaces, "  ", maxNumbers - timeDesc.Count(c => c != ':'));

                var runDesc = timeDescSpaces + timeDesc + "  -  " + run.TimeElapsedString + "  -  " + run.Game;

                runHistory.Add(runDesc);
            }

            return runHistory;
        }

        bool ProcessHighlight(RunHistory.Item run)
        {
            _highlightInfo = null;
            txtBoxVidUrl.Text = "Searching...";
            txtBoxStartTime.Text = txtBoxEndTime.Text = String.Empty;
            txtBoxEndTime.ForeColor = txtBoxStartTime.ForeColor = System.Drawing.SystemColors.WindowText;
            tooltipOutOfVid.SetToolTip(txtBoxStartTime, "");
            tooltipOutOfVid.SetToolTip(txtBoxEndTime, "");
            tlpVideo.Enabled = false;
            tlpVideo.Refresh();

            if ((_video = SearchVideo(run)) != null)
            {
                txtBoxVidUrl.Text = String.Format("twitch.tv/{0}/manager/{1}/highlight", _video.channel.name, _video._id);
                tlpVideo.Enabled = true;
            }
            else
            {
                txtBoxVidUrl.Text = "No video found";
                return false;
            }

            _highlightInfo = new HighlightInfo(_video, run, _settings.HighlightBuffer);
            var outOfVidTooltip = "The recording is still being processed according to Twitch's API.\nThis time might be out of the video's range until the recording is complete.";

            txtBoxStartTime.Text = _highlightInfo.StartTimeString;
            if (_highlightInfo.IsStartOutOfVideo)
            {
                txtBoxStartTime.ForeColor = System.Drawing.Color.Red;
                tooltipOutOfVid.SetToolTip(txtBoxStartTime, outOfVidTooltip);
                chkAutomateHighlight.Enabled = chkAutomateHighlight.Checked = false;
            }

            txtBoxEndTime.Text = _highlightInfo.EndTimeString;
            if (_highlightInfo.IsEndOutOfVideo)
            {
                    tooltipOutOfVid.SetToolTip(txtBoxEndTime, outOfVidTooltip);
                    txtBoxEndTime.ForeColor = System.Drawing.Color.Red;
                    chkAutomateHighlight.Enabled = chkAutomateHighlight.Checked = false;
            }

            return true;
        }

        dynamic SearchVideo(RunHistory.Item run)
        {
            var requestDelay = TimeSpan.FromMilliseconds(1000);
            if (DateTime.UtcNow - _lastSearch < requestDelay) //avoid spamming api requests
                Thread.Sleep(requestDelay - (DateTime.UtcNow - _lastSearch));

            try
            {
                dynamic videos;
                if ((videos = Twitch.Instance.GetPastBroadcasts(txtBoxTwitchUsername.Text)) != null)
                {
                    dynamic streamInfo = Twitch.Instance.GetStream(txtBoxTwitchUsername.Text);
                    _lastSearch = DateTime.UtcNow;

                    int i = 0;
                    foreach (dynamic video in videos)
                    {
                        DateTime videoStart = Twitch.ParseDate(video.recorded_at);
                        DateTime videoEnd = videoStart.Add(TimeSpan.FromSeconds(video.length));
                        video.stream = streamInfo.stream; // null if stream is offline
                        video.latest_video = i == 0;
                        video.is_incomplete = video.status == "recording" || (video.latest_video && video.stream != null);

                        if (videoStart <= run.UtcStart + TimeSpan.FromSeconds(12)
                            && (run.UtcEnd - TimeSpan.FromSeconds(12) <= videoEnd || video.is_incomplete))
                        {
                            return video;
                        }

                        i++;
                    }
                }
            }
            catch (Exception e)
            {
                string message = String.Empty;

                if (e is System.Net.WebException)
                {
                    message = "An error occured while trying to reach Twitch's API.";
                }
                else
                    message = "An unexpected error occured while trying to retrieve the video.";

                MessageBox.Show(this, message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        private void btnOpenVideoManager_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://" + txtBoxVidUrl.Text);
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            if (!Twitch.Instance.IsLoggedIn)
                Twitch.Instance.VerifyLogin();

            if (Twitch.Instance.IsLoggedIn)
            {
                var automated = chkAutomateHighlight.Checked;

                if (automated)
                {
                    var result = MessageBox.Show(this, "This will automatically create the highlight.\nDo you still want to do this?", "Confirm automatic highlighting", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.No)
                        return;
                }

                using (var form = new VideoManagerForm(_highlightInfo, automated))
                {
                    form.ShowDialog(this);
                }
            }
        }

        private void txtBoxTwitchUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //enter key
            {
                if (_runs == null || _runs.Count <= 0)
                    return;

                if (lstRunHistory.SelectedIndex >= 0)
                {
                    ProcessHighlight(_runs[lstRunHistory.SelectedIndex]);
                    _lastRunSearched = lstRunHistory.SelectedIndex;
                }
                else
                    lstRunHistory.SelectedIndex = 0;
            }

            if (!char.IsControl(e.KeyChar) && !Twitch.IsValidUsername(e.KeyChar.ToString())) //block chars forbidden in twitch usernames
                e.Handled = true;
        }

        private void lstRunHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_runs == null || _runs.Count <= 0)
                return;

            if (lstRunHistory.SelectedIndex >= 0 && _lastRunSearched != lstRunHistory.SelectedIndex)
            {
                ProcessHighlight(_runs[lstRunHistory.SelectedIndex]);
                _lastRunSearched = lstRunHistory.SelectedIndex;
            }
        }

        private void txtBoxEndTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtBoxEndTime_Click(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            t.SelectAll();
        }
    }
}
