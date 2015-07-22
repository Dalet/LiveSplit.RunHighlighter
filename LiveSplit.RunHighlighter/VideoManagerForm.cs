using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LiveSplit.RunHighlighter
{
    public partial class VideoManagerForm : Form
    {
        public HighlightInfo HighlightInfo { get; private set; }
        public bool Automated { get; private set; }
        public string Channel { get; private set; }

        bool _isJavaScriptInjected;
        RunHighlighterSettings _settings;

        public VideoManagerForm(RunHighlighterSettings settings, HighlightInfo highlightInfo, bool automated = false)
        {
            InitializeComponent();

            this._settings = settings;
            this.ShowIcon = false;
            this.HighlightInfo = highlightInfo;
            this.Automated = automated;
            this.Channel = HighlightInfo.ManagerURI.Segments[1].Substring(0, HighlightInfo.ManagerURI.Segments[1].IndexOf('/'));

            webBrowser.Navigate(HighlightInfo.ManagerURI);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if ((webBrowser.Url.Host != "twitch.tv" && webBrowser.Url.Host != "www.twitch.tv") || webBrowser.Url.LocalPath != this.HighlightInfo.ManagerURI.LocalPath)
                return;

            if (!_isJavaScriptInjected && webBrowser.ReadyState == WebBrowserReadyState.Complete)
            {
                var js = Properties.Resources.waitForKeyElements + "\n" + GetInjectionCode();
                ExecuteJavascript(js);
                _isJavaScriptInjected = true;
            }
        }

        string GetInjectionCode()
        {
            var document = webBrowser.Document;
            var varList = new Dictionary<string, string>
            {
                { "{automated}", Automated ? "true" : "false" },
                { "{start_time_str}", HighlightInfo.StartTimeString },
                { "{end_time_str}", HighlightInfo.EndTimeString },
                { "{start_time}", ((int)HighlightInfo.StartTime.TotalSeconds).ToString() },
                { "{end_time}", ((int)HighlightInfo.EndTime.TotalSeconds).ToString() },
                { "{duration}", HighlightInfo.HighlightTimeString(HighlightInfo.EndTime - HighlightInfo.StartTime) },
                { "{title}", HighlightInfo.Title },
                { "{description}", GetDescription() },
                { "{tag_list}", "speedrun" },
                { "{lang}", "en" },
                { "{out_of_vid}", HighlightInfo.IsOutOfVideo ? "true" : "false" }
            };

            var js = Properties.Resources.highlightInjection;

            foreach (string key in varList.Keys)
            {
                js = js.Replace(key, EscapeSpecialChars(varList[key]));
            }

            return js;
        }

        static string EscapeSpecialChars(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("\r", "\\r");
            str = str.Replace("\n", "\\n");
            str = str.Replace("\"", "\\\"");

            return str;
        }

        string GetDescription()
        {
            string newlines = !String.IsNullOrEmpty(HighlightInfo.Description) ? "\n\n" : String.Empty;

            if (Automated)
                return HighlightInfo.Description + newlines + "Automatically highlighted by Run Highlighter\nhttps://github.com/Dalet/LiveSplit.RunHighlighter/releases";
            else
                return HighlightInfo.Description + newlines + "Highlighted with Run Highlighter.\nhttps://github.com/Dalet/LiveSplit.RunHighlighter/releases";
        }

        object ExecuteJavascript(string code)
        {
            return webBrowser.Document.InvokeScript("eval", new object[] { code });
        }
    }
}
