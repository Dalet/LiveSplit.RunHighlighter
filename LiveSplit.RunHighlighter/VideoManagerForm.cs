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

        public VideoManagerForm(HighlightInfo highlightInfo, bool automated = false)
        {
            InitializeComponent();

            this.ShowIcon = false;
            this.HighlightInfo = highlightInfo;
            this.Automated = automated;
            this.Channel = HighlightInfo.ManagerURI.Segments[1].Substring(0, HighlightInfo.ManagerURI.Segments[1].IndexOf('/'));

            webBrowser.Navigate(HighlightInfo.ManagerURI);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (webBrowser.Url.Host != "twitch.tv" && webBrowser.Url.Host != "www.twitch.tv" && webBrowser.Url.AbsoluteUri != this.HighlightInfo.ManagerURI.AbsoluteUri)
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
                { "{title}", GetTitle() },
                { "{description}", GetDescription() },
                { "{tag_list}", "speedrun" },
                { "{lang}", "en" },
                { "{out_of_vid}", HighlightInfo.IsOutOfVideo ? "true" : "false" }
            };

            var js = Properties.Resources.highlightInjection;

            foreach (string key in varList.Keys)
            {
                js = js.Replace(key, varList[key]);
            }

            return js;
        }

        string GetTitle()
        {
            var formatter = new TimeFormatters.RegularTimeFormatter(TimeFormatters.TimeAccuracy.Seconds);
            var ts = HighlightInfo.Run.Time.GameTime.Value;
            if (ts.Milliseconds >= 500)
                ts += TimeSpan.FromSeconds(1);
            var time = formatter.Format(ts);

            ts = HighlightInfo.Run.Time.RealTime.Value;
            if (ts.Milliseconds >= 500)
                ts += TimeSpan.FromSeconds(1);
            var rtaTime = formatter.Format(ts);

            if (rtaTime == time)
                rtaTime = "";
            else
                rtaTime = " (" + rtaTime + " RTA)";

            return String.Format("{0} speedrun in {1}{2}", HighlightInfo.Run.Game, time, rtaTime);
        }

        string GetDescription()
        {
            if (Automated)
                return "Automatically highlighted by Run Highlighter\\nhttps://github.com/Dalet/LiveSplit.RunHighlighter/releases";
            else
                return "Highlighted with Run Highlighter.\\nhttps://github.com/Dalet/LiveSplit.RunHighlighter/releases";
        }

        object ExecuteJavascript(string code)
        {
            return webBrowser.Document.InvokeScript("eval", new object[] { code });
        }
    }
}
