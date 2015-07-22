using System;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RunHighlighter
{
    public partial class RunHighlighterSettings : UserControl
    {
        public int HighlightBuffer { get; set; }
        public uint MaxRunHistoryLength { get; set; }
        public string TwitchUsername { get; set; }

        public string TitleText { get; set; }
        public string DescriptionText { get; set; }
        public bool TruncateTimes { get; set; }

        public const int DEFAULT_HIGHLIGHT_BUFFER = 7;
        private const string DEFAULT_TWITCH_USERNAME = "";
        public const uint DEFAULT_MAX_HISTORY_LENGTH = 50;
        public const string DEFAULT_TITLE_TEXT = "$game $category speedrun in $gametime[RT!=GT] ($realtime RTA)[/RT!=GT]";
        public const string DEFAULT_DESCRIPTION_TEXT = "";
        public const bool DEFAULT_TRUNCATE_TIMES = false;

        private RunHighlighterComponent _component;

        public RunHighlighterSettings(RunHighlighterComponent component)
        {
            InitializeComponent();

            this._component = component;
            this.txtBoxTitle.DataBindings.Add("Text", this, "TitleText", false, DataSourceUpdateMode.OnPropertyChanged);
            this.txtBoxDescription.DataBindings.Add("Text", this, "DescriptionText", false, DataSourceUpdateMode.OnPropertyChanged);
            this.rbTruncateTimes.DataBindings.Add("Checked", this, "TruncateTimes", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numLeeway.DataBindings.Add("Value", this, "HighlightBuffer", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numMaxHistoryLength.DataBindings.Add("Value", this, "MaxRunHistoryLength", false, DataSourceUpdateMode.OnPropertyChanged);
            this.numMaxHistoryLength.ValueChanged += (s, e) =>
            {
                if (_component.Activated)
                    RunHistory.MaxHistoryCount = MaxRunHistoryLength;
            };

            // defaults
            this.HighlightBuffer = DEFAULT_HIGHLIGHT_BUFFER;
            this.TwitchUsername = DEFAULT_TWITCH_USERNAME;
            this.MaxRunHistoryLength = DEFAULT_MAX_HISTORY_LENGTH;
            this.TitleText = DEFAULT_TITLE_TEXT;
            this.DescriptionText = DEFAULT_DESCRIPTION_TEXT;
            this.TruncateTimes = DEFAULT_TRUNCATE_TIMES;
        }


        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(ToElement(doc, "HighlightBuffer", HighlightBuffer));
            settingsNode.AppendChild(ToElement(doc, "TwitchUsername", TwitchUsername));
            settingsNode.AppendChild(ToElement(doc, "MaxRunHistoryLength", MaxRunHistoryLength));
            settingsNode.AppendChild(ToElement(doc, "TitleText", TitleText));
            settingsNode.AppendChild(ToElement(doc, "DescriptionText", DescriptionText));
            settingsNode.AppendChild(ToElement(doc, "TruncateTimes", TruncateTimes));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            var element = (XmlElement)settings;

            this.HighlightBuffer = ParseInt(settings["HighlightBuffer"], DEFAULT_HIGHLIGHT_BUFFER);
            this.TwitchUsername = ParseString(settings["TwitchUsername"], DEFAULT_TWITCH_USERNAME);
            this.TitleText = ParseString(settings["TitleText"], DEFAULT_TITLE_TEXT);
            this.DescriptionText = ParseString(settings["DescriptionText"], DEFAULT_DESCRIPTION_TEXT);
            this.TruncateTimes = ParseBool(settings["TruncateTimes"], DEFAULT_TRUNCATE_TIMES);
            this.MaxRunHistoryLength = (uint)ParseInt(settings["MaxRunHistoryLength"], (int)DEFAULT_MAX_HISTORY_LENGTH);

            if (MaxRunHistoryLength >= numMaxHistoryLength.Minimum)
                RunHistory.MaxHistoryCount = MaxRunHistoryLength;
            else
                MaxRunHistoryLength = DEFAULT_MAX_HISTORY_LENGTH;
        }

        static bool ParseBool(XmlElement element, bool defaultBool = default(bool))
        {
            return element != null ? Boolean.Parse(element.InnerText) : defaultBool;
        }

        static string ParseString(XmlElement stringElement, string defaultString = default(string))
        {
            return stringElement != null ? stringElement.InnerText : defaultString;
        }

        static int ParseInt(XmlElement intElement, int defaultInt = 0)
        {
            return intElement != null ? Int32.Parse(intElement.InnerText) : defaultInt;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            var hl = new HighlightInfo(
                null,
                new RunHistory.Item(DateTime.UtcNow, true, DateTime.UtcNow + TimeSpan.FromMinutes(16.5), true, new Model.Time(TimeSpan.Parse("0:33:43.4"), TimeSpan.Parse("0:33:24.72")), "Half-Life", "Any%"),
                this);
            MessageBox.Show(this, String.Format("Title:\n{0}\n\nDescription:\n{1}", hl.Title, hl.Description), "Preview for a 33:24.72/33:43.4 run");
        }

        private void btnVariableHelp_Click(object sender, EventArgs e)
        {
            string varsHelp =
@"Variables:

$realtime = Real Time of the run. Exemple: '1:48:54'
$gametime = Game Time of the run. Exemple: '1:40:03'
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
