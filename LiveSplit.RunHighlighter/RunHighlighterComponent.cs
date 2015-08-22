using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RunHighlighter
{
    public class RunHighlighterComponent : LogicComponent
    {
        public override string ComponentName => "Run Highlighter";

        public RunHighlighterSettings Settings { get; set; }

        private LiveSplitState _state;

        public RunHighlighterComponent(LiveSplitState state)
        {
            this._state = state;
            this.Settings = new RunHighlighterSettings();
            this.ContextMenuControls = new Dictionary<string, Action>();

            this.ContextMenuControls.Add("Run Highlighter...", new Action(() =>
            {
                if (_state.CurrentPhase == TimerPhase.Ended)
                {
                    MessageBox.Show(_state.Form, "The timer needs to be resetted in order to update the Attempt History.", "Run Highlighter");
                    return;
                }

                var originalTopMost = _state.Form.TopMost;
                _state.Form.TopMost = false; //to ensure our Internet Explorer window isn't covered

                using (var form = new RunHighlighterForm(state.Run, Settings))
                    form.ShowDialog(state.Form);

                _state.Form.TopMost = originalTopMost;
            }));
        }
        public override XmlNode GetSettings(XmlDocument document) => Settings.GetSettings(document);

        public override Control GetSettingsControl(LayoutMode mode) => Settings;

        public override void SetSettings(XmlNode settings) => Settings.SetSettings(settings);

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }

        public override void Dispose() { }
    }
}
