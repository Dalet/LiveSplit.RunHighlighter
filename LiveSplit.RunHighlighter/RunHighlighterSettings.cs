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

        public const int DEFAULT_HIGHLIGHT_BUFFER = 7;
        private const string DEFAULT_TWITCH_USERNAME = "";
        public const uint DEFAULT_MAX_HISTORY_LENGTH = 50;

        private RunHighlighterComponent _component;

        public RunHighlighterSettings(RunHighlighterComponent component)
        {
            InitializeComponent();

            this._component = component;
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
        }


        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("Settings");

            settingsNode.AppendChild(ToElement(doc, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));
            settingsNode.AppendChild(ToElement(doc, "HighlightBuffer", DEFAULT_HIGHLIGHT_BUFFER));
            settingsNode.AppendChild(ToElement(doc, "TwitchUsername", DEFAULT_TWITCH_USERNAME));
            settingsNode.AppendChild(ToElement(doc, "MaxRunHistoryLength", DEFAULT_MAX_HISTORY_LENGTH));

            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            var element = (XmlElement)settings;

            this.HighlightBuffer = ParseInt(settings["HighlightBuffer"], DEFAULT_HIGHLIGHT_BUFFER);
            this.TwitchUsername = ParseString(settings["TwitchUsername"], DEFAULT_TWITCH_USERNAME);
            this.MaxRunHistoryLength = (uint)ParseInt(settings["MaxRunHistoryLength"], (int)DEFAULT_MAX_HISTORY_LENGTH);
            
            if (MaxRunHistoryLength >= numMaxHistoryLength.Minimum)
                RunHistory.MaxHistoryCount = MaxRunHistoryLength;
            else
                MaxRunHistoryLength = DEFAULT_MAX_HISTORY_LENGTH;
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ?
                (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_)
                : default_;
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
    }
}
