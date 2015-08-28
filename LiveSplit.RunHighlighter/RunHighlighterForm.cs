using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Twitch = LiveSplit.RunHighlighter.TwitchExtension;

namespace LiveSplit.RunHighlighter
{
    public partial class RunHighlighterForm : Form
    {
        private RunHighlighterSettings _settings;
        private IList<RunHistory.Run> _runs;
        private IRun _splits;

        private dynamic _video;
        private HighlightInfo _highlightInfo;
        private VideoManager _vidManager;

        private int? _lastSearchTimestamp;
        private int _lastRunSearched;

        public RunHighlighterForm(IRun splits, RunHighlighterSettings settings)
        {
            InitializeComponent();

            this.MaximumSize = new System.Drawing.Size(this.Width, Screen.AllScreens.Max(s => s.Bounds.Height));
            this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);
            this.ShowIcon = false;
            this.picStartTime.DataBindings.Add("BackColor", this.txtBoxStartTime, "BackColor", false, DataSourceUpdateMode.OnPropertyChanged);
            this.picEndTime.DataBindings.Add("BackColor", this.txtBoxEndTime, "BackColor", false, DataSourceUpdateMode.OnPropertyChanged);

            this._lastSearchTimestamp = null;
            this._lastRunSearched = -1;
            this._settings = settings;
            this._splits = splits;

            var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Text += " v" + ver.ToString(2) + (ver.Build > 0 ? "." + ver.Build : "");
            lstRunHistory.Items.Clear(); //clear items added as exemples in the Designer

            this.Load += RunHighlighterForm_Load;
            this.FormClosed += Form_OnClose;
        }

        private void RunHighlighterForm_Load(object sender, EventArgs e)
        {
            _runs = RunHistory.GetRunHistory(_splits, _settings.MaxRunHistoryLength, _settings.HideUnreliableHistory);
            PopulateListbox(_runs);

            if (Twitch.IsValidUsername(_settings.TwitchUsername))
                this.txtBoxTwitchUsername.Text = _settings.TwitchUsername;
            else if (Twitch.Instance.IsLoggedIn)
                this.txtBoxTwitchUsername.Text = Twitch.Instance.ChannelName;
        }

        void Form_OnClose(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBoxTwitchUsername.Text))
                _settings.TwitchUsername = txtBoxTwitchUsername.Text;
        }

        void PopulateListbox(IList<RunHistory.Run> runs)
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
                lstRunHistory.Items.AddRange(RunHistory.HistoryToString(runs).ToArray());
        }

        async void GetHighlightInfo(RunHistory.Run run)
        {
            var channel = txtBoxTwitchUsername.Text;

            if (!Twitch.IsValidUsername(channel))
            {
                MessageBox.Show(this, "Invalid Twitch username.", "Run Highlighter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBoxTwitchUsername.Focus();
                return;
            }

            _highlightInfo = null;
            txtBoxVidUrl.Text = "Searching...";
            lstRunHistory.Enabled = txtBoxTwitchUsername.Enabled = false;
            ResetTlpVideo();
            _lastRunSearched = lstRunHistory.SelectedIndex;

            var requestDelay = TimeSpan.FromMilliseconds(1000);
            var timeSinceLastSearch = _lastSearchTimestamp != null
                ? TimeSpan.FromMilliseconds(Environment.TickCount - (double)_lastSearchTimestamp)
                : requestDelay;

            await Task.Run(() =>
            {
                if (timeSinceLastSearch < requestDelay) //avoid spamming api requests
                    Thread.Sleep(requestDelay - timeSinceLastSearch);

                _lastSearchTimestamp = Environment.TickCount;
                _video = Twitch.Instance.SearchRunBroadcast(channel, run);
            });

            if (_video == null)
            {
                txtBoxVidUrl.Text = "No video found";
                _lastRunSearched = -1;
            }
            else
            {
                _highlightInfo = new HighlightInfo(_video, run, _settings);
                UpdateVideoGroupBox(_highlightInfo);
            }

            lstRunHistory.Enabled = txtBoxTwitchUsername.Enabled = true;
        }

        void UpdateVideoGroupBox(HighlightInfo info)
        {
            txtBoxVidUrl.Text = info.Video.url;
            tlpVideo.Enabled = true;
            txtBoxStartTime.Text = info.StartTimeString;
            txtBoxEndTime.Text = info.EndTimeString;

            //out of vid warnings
            if (info.IsStartOutOfVideo)
                SetOutOfVidWarning(txtBoxStartTime);

            if (info.IsEndOutOfVideo)
                SetOutOfVidWarning(txtBoxEndTime);

            //unreliable time warnings
            if (!info.Run.IsUtcStartReliable)
                SetUnreliableTimeWarning(txtBoxStartTime, picStartTime);

            if (!info.Run.IsUtcEndReliable)
                SetUnreliableTimeWarning(txtBoxEndTime, picEndTime);

            //disable auto-create if there are any warnings
            if (info.IsOutOfVideo || !info.Run.IsUtcStartReliable || !info.Run.IsUtcEndReliable)
                chkAutomateHighlight.Enabled = chkAutomateHighlight.Checked = false;
        }

        void SetOutOfVidWarning(TextBox timestampBox)
        {
            var outOfVidTooltip = "The recording is still being processed according to Twitch's API.\n"
                + "This time might be out of the video's range until the recording is complete.";

            timestampBox.ForeColor = System.Drawing.Color.Red;
            tooltipOutOfVid.SetToolTip(timestampBox, outOfVidTooltip);
        }

        void SetUnreliableTimeWarning(TextBox timestampBox, PictureBox warningPic)
        {
            var unreliableTooltip = "This time could be incorrect. Checking it is highly recommended.\n"
                + "This is due to a failure to synchronize with the internet clock during the run.";

            timestampBox.BackColor = System.Drawing.Color.PaleGoldenrod;
            warningPic.Image = Properties.Resources.warning;
            toolTipUnreliableTime.SetToolTip(warningPic, unreliableTooltip);
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

        private void SearchCurrentlySelected()
        {
            if (_runs == null || _runs.Count <= 0)
                return;

            if (lstRunHistory.SelectedIndex >= 0 && _lastRunSearched != lstRunHistory.SelectedIndex)
                GetHighlightInfo(_runs[lstRunHistory.SelectedIndex]);
        }

        private void btnHighlight_Click(object sender, EventArgs e)
        {
            var automated = chkAutomateHighlight.Checked;

            if (automated)
            {
                if (DialogResult.No == MessageBox.Show(this, "This will automatically create the highlight.\nDo you still want to do this?", "Confirm automatic highlighting", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
            }

            btnHighlight.Enabled = lstRunHistory.Enabled = false;
            _vidManager = new VideoManager(_settings, _highlightInfo, automated);

            var uiThread = SynchronizationContext.Current;
            EventHandler onDisposed = (s, arg) => uiThread.Post(d =>
            {
                btnHighlight.Enabled = lstRunHistory.Enabled = true;
                _vidManager = null;
            }, null);

            if (!_vidManager.IsDisposing)
                _vidManager.Disposed += onDisposed;
            else //call it manually if it was disposed before we could register the eventhandler
                onDisposed(this, EventArgs.Empty);
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
