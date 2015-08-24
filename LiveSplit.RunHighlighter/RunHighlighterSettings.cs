using LiveSplit.UI;
using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RunHighlighter
{
    public partial class RunHighlighterSettings : UserControl
    {
        public int HighlightBuffer { get; set; }
        public int MaxRunHistoryLength { get; set; }
        public string TwitchUsername { get; set; }

        public string TitleText { get; set; }
        public string DescriptionText { get; set; }
        public bool TruncateTimes { get; set; }
        public bool HideUnreliableHistory { get; set; }

        public const int DEFAULT_HIGHLIGHT_BUFFER = 7;
        public const int DEFAULT_MAX_HISTORY_LENGTH = 50;
        public const string DEFAULT_TITLE_TEXT = "$game $category speedrun in $gametime[RT!=GT] ($realtime RTA)[/RT!=GT]";
        public const string DEFAULT_DESCRIPTION_TEXT = "";
        public const bool DEFAULT_TRUNCATE_TIMES = false;
        public const bool DEFAULT_HIDE_UNRELIABLE_HISTORY = false;
        private const string DEFAULT_TWITCH_USERNAME = "";

        public RunHighlighterSettings()
        {
            InitializeComponent();

            this.txtBoxTitle.DataBindings.Add("Text", this, "TitleText", false, DataSourceUpdateMode.OnPropertyChanged);
            this.txtBoxDescription.DataBindings.Add("Text", this, "DescriptionText", false, DataSourceUpdateMode.OnPropertyChanged);
            this.rbTruncateTimes.DataBindings.Add("Checked", this, "TruncateTimes", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numLeeway.DataBindings.Add("Value", this, "HighlightBuffer", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numMaxHistoryLength.DataBindings.Add("Value", this, "MaxRunHistoryLength", false, DataSourceUpdateMode.OnPropertyChanged);
            this.checkBox1.DataBindings.Add("Checked", this, "HideUnreliableHistory", false, DataSourceUpdateMode.OnPropertyChanged);

            // defaults
            this.HighlightBuffer = DEFAULT_HIGHLIGHT_BUFFER;
            this.TwitchUsername = DEFAULT_TWITCH_USERNAME;
            this.MaxRunHistoryLength = DEFAULT_MAX_HISTORY_LENGTH;
            this.TitleText = DEFAULT_TITLE_TEXT;
            this.DescriptionText = DEFAULT_DESCRIPTION_TEXT;
            this.TruncateTimes = DEFAULT_TRUNCATE_TIMES;
            this.HideUnreliableHistory = DEFAULT_HIDE_UNRELIABLE_HISTORY;
        }


        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "HighlightBuffer", HighlightBuffer));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "TwitchUsername", TwitchUsername));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "MaxRunHistoryLength", MaxRunHistoryLength));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "TitleText", TitleText));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "DescriptionText", DescriptionText));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "TruncateTimes", TruncateTimes));
            settingsNode.AppendChild(SettingsHelper.ToElement(doc, "HideUnreliableHistory", HideUnreliableHistory));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            var element = (XmlElement)settings;

            this.HighlightBuffer = SettingsHelper.ParseInt(settings["HighlightBuffer"], DEFAULT_HIGHLIGHT_BUFFER);
            this.TwitchUsername = SettingsHelper.ParseString(settings["TwitchUsername"], DEFAULT_TWITCH_USERNAME);
            this.TitleText = SettingsHelper.ParseString(settings["TitleText"], DEFAULT_TITLE_TEXT);
            this.DescriptionText = SettingsHelper.ParseString(settings["DescriptionText"], DEFAULT_DESCRIPTION_TEXT);
            this.TruncateTimes = SettingsHelper.ParseBool(settings["TruncateTimes"], DEFAULT_TRUNCATE_TIMES);
            this.MaxRunHistoryLength = SettingsHelper.ParseInt(settings["MaxRunHistoryLength"], DEFAULT_MAX_HISTORY_LENGTH);
            this.HideUnreliableHistory = SettingsHelper.ParseBool(settings["HideUnreliableHistory"], DEFAULT_HIDE_UNRELIABLE_HISTORY);

            if (MaxRunHistoryLength < numMaxHistoryLength.Minimum)
                MaxRunHistoryLength = DEFAULT_MAX_HISTORY_LENGTH;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            var hl = new HighlightInfo(
                null,
                new RunHistory.Run
                {
                    UtcStart = DateTime.UtcNow,
                    IsUtcStartReliable = true,
                    UtcEnd = DateTime.UtcNow + TimeSpan.FromMinutes(16.5),
                    IsUtcEndReliable = true,
                    Time = new Model.Time(TimeSpan.Parse("0:33:43.4"), TimeSpan.Parse("0:33:24.72")),
                    Game = "Half-Life",
                    Category = "Any%"
                },
                this);
            MessageBox.Show(this, String.Format("Title:\n{0}\n\nDescription:\n{1}", hl.Title, hl.Description), "Preview for a 33:24.72/33:43.4 run");
        }

        private void btnVariableHelp_Click(object sender, EventArgs e)
        {
            string varsHelp =
@"Variables:

$realtime = Real Time of the run. Exemple: '1:48:54'
$gametime = Game Time of the run. If irrelevant, it will simply display Real Time.
$game = Game name
$category = Run category
$twitchchannel = Twitch channel where the video comes from.

Conditional blocks:

[RT!=GT]text[/RT!=GT] = Shows text only if Game Time and Real Time are different.

Examples:
With a 10:03 Real Time and 8:12 Game Time run:
    'Speedrun in $gametime[RT!=GT] ($realtime RTA)[/RT!=GT]'
    => 'Speedrun in 8:12 (10:03 RTA)'
With a 10:03 Real Time and 10:03 Game Time run:
    'Speedrun in $gametime[RT!=GT] ($realtime RTA)[/RT!=GT]'
    => 'Speedrun in 10:03'";

            MessageBox.Show(this, varsHelp, "Formatting help");
        }

        private void btnHLDetailsRestoreDefault_Click(object sender, EventArgs e)
        {
            txtBoxTitle.Text = DEFAULT_TITLE_TEXT;
            txtBoxDescription.Text = DEFAULT_DESCRIPTION_TEXT;
            rbTruncateTimes.Checked = DEFAULT_TRUNCATE_TIMES;
            rbRoundTimes.Checked = !DEFAULT_TRUNCATE_TIMES;
        }
    }
}
