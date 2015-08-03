using mshtml;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace LiveSplit.RunHighlighter
{
    public class VideoManager : IDisposable
    {
        public event EventHandler Disposed;
        public bool IsDisposing { get; private set; }

        public HighlightInfo HighlightInfo { get; private set; }
        public bool Automated { get; private set; }
        public string Channel { get; private set; }

        RunHighlighterSettings _settings;
        InternetExplorer IE;

        public VideoManager(RunHighlighterSettings settings, HighlightInfo highlightInfo, bool automated = false)
        {
            this._settings = settings;
            this.HighlightInfo = highlightInfo;
            this.Automated = automated;
            this.Channel = HighlightInfo.ManagerURI.Segments[1].Substring(0, HighlightInfo.ManagerURI.Segments[1].IndexOf('/'));

            InitializeIE();

            IE.Navigate(HighlightInfo.ManagerURL);
            IE.Visible = true;            
        }

        private void InitializeIE()
        {
            IE = new SHDocVw.InternetExplorer();

            IE.ToolBar = 0;
            IE.MenuBar = false;
            IE.StatusBar = false;
            IE.Width = 1110;
            IE.Height = 670;
            IE.DocumentComplete += IE_DocumentCompleted;
            IE.OnQuit += () => Dispose();
        }

        private void IE_DocumentCompleted(object pDisp, ref object urlObj)
        {
            var url = new Uri((string)urlObj);
            if ((url.Host != "twitch.tv" && url.Host != "www.twitch.tv") || url.LocalPath != this.HighlightInfo.ManagerURI.LocalPath)
                return;

            if (IE.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE || !isLoggedInTwitch())
                return;

            var js = Properties.Resources.waitForKeyElements + "\n" + GetInjectionCode();
            ExecuteJavascript(js);
            Debug.WriteLine("JavaScript injected.");
        }

        bool isLoggedInTwitch()
        {
            HTMLDocument doc = IE.Document;
            foreach (IHTMLElement elem in doc.getElementsByTagName("div"))
            {
                if (elem.getAttribute("className") != null && elem.getAttribute("className").Contains("isLoggedIn"))
                    return true;
            }

            return false;
        }

        string GetInjectionCode()
        {
            var document = IE.Document;
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

        void ExecuteJavascript(string code)
        {
            Thread aThread = new Thread(() =>
            {
                try
                {
                    HTMLDocument doc = IE.Document;
                    object script = doc.Script;
                    script.GetType().InvokeMember("eval", System.Reflection.BindingFlags.InvokeMethod, null, script, new object[] { code });
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            });
            aThread.SetApartmentState(ApartmentState.STA);
            aThread.Start();
        }

        public void Dispose()
        {
            if (IsDisposing)
                return;

            IsDisposing = true;

            if (IE != null)
            {
                IE.DocumentComplete -= IE_DocumentCompleted;
                IE.Quit();
            }

            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
