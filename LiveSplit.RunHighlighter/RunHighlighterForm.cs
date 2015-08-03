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
        private VideoManager _vidManager;

        private DateTime _lastSearch;
        private int _lastRunSearched;

        public RunHighlighterForm(RunHighlighterSettings settings)
        {
            InitializeComponent();

            this.MaximumSize = new System.Drawing.Size(this.Width, Screen.AllScreens.Max(s => s.Bounds.Height));
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            this.ShowIcon = false;
            this.picStartTime.DataBindings.Add("BackColor", this.txtBoxStartTime, "BackColor", false, DataSourceUpdateMode.OnPropertyChanged);
            this.picEndTime.DataBindings.Add("BackColor", this.txtBoxEndTime, "BackColor", false, DataSourceUpdateMode.OnPropertyChanged);

            this._lastSearch = new DateTime(0);
            this._lastRunSearched = -1;
            this._settings = settings;

            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += " v" + ver.ToString(2) + (ver.Build > 0 ? "." + ver.Build : "");
            lstRunHistory.Items.Clear();

            //make impossible to select another run while the video manager is open
            lstRunHistory.GotFocus += (s, e) =>
            {
                if (_vidManager != null)
                {
                    lstRunHistory.Parent.Focus();
                    MessageBox.Show(this, "Close the Internet Explorer window before selecting another run.", "Run Highlighter");
                }
            };

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

                    if (Twitch.IsValidUsername(_settings.TwitchUsername))
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

        void ResetTlpVideo()
        {
            txtBoxStartTime.Text = txtBoxEndTime.Text = String.Empty;
            txtBoxEndTime.ForeColor = txtBoxStartTime.ForeColor = System.Drawing.SystemColors.WindowText;
            txtBoxEndTime.BackColor = txtBoxStartTime.BackColor = System.Drawing.SystemColors.Control;
            tooltipOutOfVid.SetToolTip(txtBoxStartTime, "");
            tooltipOutOfVid.SetToolTip(txtBoxEndTime, "");
            toolTipUnreliableTime.SetToolTip(txtBoxStartTime, "");
            toolTipUnreliableTime.SetToolTip(txtBoxEndTime, "");
            picStartTime.Image = picEndTime.Image = null;
            chkAutomateHighlight.Enabled = true;
            chkAutomateHighlight.Checked = false;
            tlpVideo.Enabled = false;
        }

        bool ProcessHighlight(RunHistory.Item run)
        {
            if (!Twitch.IsValidUsername(txtBoxTwitchUsername.Text))
            {
                MessageBox.Show(this, "Invalid Twitch username.", "Run Highlighter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxTwitchUsername.Focus();
                return false;
            }

            _highlightInfo = null;
            txtBoxVidUrl.Text = "Searching...";
            _lastRunSearched = lstRunHistory.SelectedIndex;

            ResetTlpVideo();
            tlpVideo.Refresh();

            if ((_video = SearchVideo(run)) == null)
            {
                txtBoxVidUrl.Text = "No video found";
                _lastRunSearched = -1;
                return false;
            }

            _highlightInfo = new HighlightInfo(_video, run, _settings);
            txtBoxVidUrl.Text = _video.url;
            tlpVideo.Enabled = true;

            var outOfVidTooltip = "The recording is still being processed according to Twitch's API.\nThis time might be out of the video's range until the recording is complete.";
            var unreliableTooltip = "This time could be incorrect. Checking it is highly recommended.\nThis is due to a failure to synchronize with the NIST time server during the run.";

            txtBoxStartTime.Text = _highlightInfo.StartTimeString;
            if (_highlightInfo.IsStartOutOfVideo)
            {
                txtBoxStartTime.ForeColor = System.Drawing.Color.Red;
                tooltipOutOfVid.SetToolTip(txtBoxStartTime, outOfVidTooltip);
            }

            if (!run.IsUtcStartReliable)
            {
                txtBoxStartTime.BackColor = System.Drawing.Color.PaleGoldenrod;
                picStartTime.Image = Properties.Resources.warning;
                toolTipUnreliableTime.SetToolTip(picStartTime, unreliableTooltip);
            }

            txtBoxEndTime.Text = _highlightInfo.EndTimeString;
            if (_highlightInfo.IsEndOutOfVideo)
            {
                tooltipOutOfVid.SetToolTip(txtBoxEndTime, outOfVidTooltip);
                txtBoxEndTime.ForeColor = System.Drawing.Color.Red;
            }

            if (!run.IsUtcEndReliable)
            {
                txtBoxEndTime.BackColor = System.Drawing.Color.PaleGoldenrod;
                picEndTime.Image = Properties.Resources.warning;
                toolTipUnreliableTime.SetToolTip(picEndTime, unreliableTooltip);
            }

            if (_highlightInfo.IsOutOfVideo || !run.IsUtcStartReliable || !run.IsUtcEndReliable)
                chkAutomateHighlight.Enabled = chkAutomateHighlight.Checked = false;

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

        private void SearchCurrentlySelected()
        {
            if (_runs == null || _runs.Count <= 0)
                return;

            if (lstRunHistory.SelectedIndex >= 0 && _lastRunSearched != lstRunHistory.SelectedIndex)
                ProcessHighlight(_runs[lstRunHistory.SelectedIndex]);
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            var automated = chkAutomateHighlight.Checked;

            if (automated)
            {
                var result = MessageBox.Show(this, "This will automatically create the highlight.\nDo you still want to do this?", "Confirm automatic highlighting", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No)
                    return;
            }

            btnHighlight.Enabled = false;
            _vidManager = new VideoManager(_settings, _highlightInfo, automated);
            var uiThread = SynchronizationContext.Current;
            _vidManager.Disposed += (s, arg) => uiThread.Post(d =>
            {
                btnHighlight.Enabled = true;
                _vidManager = null;
            }, null);
        }

        private void txtBoxTwitchUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13) //enter key
            {
                if (lstRunHistory.SelectedIndex >= 0)
                    SearchCurrentlySelected();
                else
                    lstRunHistory.SelectedIndex = 0;
            }

            if (!char.IsControl(e.KeyChar) && !Twitch.IsValidUsername(e.KeyChar.ToString())) //block chars forbidden in twitch usernames
                e.Handled = true;
        }

        private void lstRunHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchCurrentlySelected();
        }

        private void txtBoxEndTime_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void txtBox_SelectAll(object sender, EventArgs e)
        {
            var t = (TextBox)sender;
            t.SelectAll();
        }

        private void txtBoxTwitchUsername_TextChanged(object sender, EventArgs e)
        {
            _lastRunSearched = -1;
        }
    }
}
