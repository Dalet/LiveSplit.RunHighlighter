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
        InternetExplorer _IE;

        public VideoManager(RunHighlighterSettings settings, HighlightInfo highlightInfo, bool automated = false)
        {
            this._settings = settings;
            this.HighlightInfo = highlightInfo;
            this.Automated = automated;
            this.Channel = HighlightInfo.ManagerURI.Segments[1].Substring(0, HighlightInfo.ManagerURI.Segments[1].IndexOf('/'));

            if (InitializeIE())
            {
                _IE.Navigate2(HighlightInfo.ManagerURL);
                _IE.Visible = true;
            }
            else
            {
                Dispose();
                System.Windows.Forms.MessageBox.Show("A problem occurred while initializing Internet Explorer controls.\nPlease try again.", "Run Highlighter error",
                    System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        private bool InitializeIE()
        {
            try
            {
                _IE = new SHDocVw.InternetExplorer();
            }
            catch
            {
                _IE = null;
                return false;
            }

            _IE.ToolBar = 0;
            _IE.MenuBar = false;
            _IE.StatusBar = false;
            _IE.Width = 1110;
            _IE.Height = 670;
            _IE.NavigateComplete2 += IE_NavigateComplete2;
            _IE.OnQuit += () => Dispose();

            return true;
        }

        private void IE_NavigateComplete2(object pDisp, ref object urlObj)
        {
            var url = new Uri((string)urlObj);

            if ((url.Host != "twitch.tv" && url.Host != "www.twitch.tv"))
                return;

            if (_IE.ReadyState != tagREADYSTATE.READYSTATE_INTERACTIVE && _IE.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE)
                return;

            var js = Properties.Resources.waitForKeyElements + "\n"
					 + Properties.Resources.run_highlighter_obj + "\n"
					 + GetInjectionCode();
            ExecuteJavascript(js);
            Debug.WriteLine("JavaScript injected.");
        }

        string GetInjectionCode()
        {
            var document = _IE.Document;
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
                { "{tag_list}", "speedrun, speedrunning" },
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
                return HighlightInfo.Description;
        }

        void ExecuteJavascript(string code)
        {
            Thread aThread = new Thread(() =>
            {
                try
                {
                    HTMLDocument doc = _IE.Document;
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
            aThread.Join();
        }

        public void Dispose()
        {
            if (IsDisposing)
                return;

            IsDisposing = true;

            if (_IE != null)
            {
                _IE.NavigateComplete2 -= IE_NavigateComplete2;
                _IE.Quit();
            }

            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
