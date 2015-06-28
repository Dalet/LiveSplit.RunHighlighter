using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.RunHighlighter
{
    public class RunHighlighterComponent : LogicComponent
    {
        public override string ComponentName
        {
            get
            {
                var deactivated = !Activated ? " (disabled)" : "";
                return "Run Highlighter" + deactivated;
            }
        }

        public bool Activated { get; set; }
        public DateTime ComponentCreationTime { get; private set; }
        public RunHighlighterSettings Settings { get; set; }
        public DateTime? StartTime { get; private set; }
        public DateTime? EndTime { get; private set; }

        private LiveSplitState _state;
        private TimerPhase _prevPhase;
        private Task _nistStartThread;
        private Task _nistEndThread;

        public RunHighlighterComponent(LiveSplitState state)
        {
            this.Activated = true;
            this.ComponentCreationTime = DateTime.UtcNow;
            this.StartTime = null;
            this.EndTime = null;
            this._state = state;
            this.Settings = new RunHighlighterSettings(this);
            this.ContextMenuControls = new Dictionary<string, Action>();

            _state.OnStart += State_OnStart;
            _state.OnUndoSplit += State_OnUndoSplit;

            this.ContextMenuControls.Add("Run Highlighter...", new Action(() =>
            {
                if (_nistStartThread != null && _nistStartThread.Status == TaskStatus.Running)
                {
                    Debug.WriteLine("Launch form: Waiting for nistStartThread to complete.");
                    _nistStartThread.Wait();
                }

                if (_nistEndThread != null && _nistEndThread.Status == TaskStatus.Running)
                {
                    Debug.WriteLine("Launch form: Waiting for nistEndThread to complete.");
                    _nistEndThread.Wait();
                }

                using (var form = new RunHighlighterForm(Settings))
                    form.ShowDialog(state.Form);
            }));
        }

        void State_OnStart(object sender, EventArgs e)
        {
            var previousStartThread = _nistStartThread;

            _nistStartThread = NIST.UtcNowAsync(d =>
            {
                if (previousStartThread != null && previousStartThread.Status == TaskStatus.Running)
                {
                    Debug.WriteLine("Start: Waiting for the previous nistStartThread to complete.");
                    previousStartThread.Wait();
                }

                if (_nistEndThread != null && _nistEndThread.Status == TaskStatus.Running)
                {
                    Debug.WriteLine("Start: Waiting for nistEndThread to complete.");
                    _nistEndThread.Wait();
                }

                StartTime = d;
                EndTime = null;
            });
        }

        void State_OnUndoSplit(object sender, EventArgs e)
        {
            if (_prevPhase == TimerPhase.Ended && _state.CurrentPhase == TimerPhase.Running)
            {
                Task.Factory.StartNew(() =>
                {
                    if (_nistEndThread != null && _nistEndThread.Status == TaskStatus.Running)
                    {
                        Debug.WriteLine("Undo: Waiting for nistEndThread to complete.");
                        _nistEndThread.Wait();
                    }

                    //start time and end time are checked to avoid removing the wrong run
                    if (StartTime != null && EndTime != null && RunHistory.RemoveLastRun(StartTime.Value, EndTime.Value))
                        Debug.WriteLine("Removed last run.");
                    else
                        Debug.WriteLine("Failed to remove last run.");
                });
            }
        }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            DeactivateDuplicateComponents();

            if (!Activated)
                return;

            if (state.CurrentPhase != _prevPhase && state.CurrentPhase == TimerPhase.Ended && StartTime != null)
            {
                var previousNistEndThread = _nistEndThread;

                _nistEndThread = NIST.UtcNowAsync(d =>
                {
                    if (_nistStartThread != null && _nistStartThread.Status == TaskStatus.Running)
                    {
                        Debug.WriteLine("End: Waiting for nistStartThread to complete.");
                        _nistStartThread.Wait();
                    }

                    if (previousNistEndThread != null && previousNistEndThread.Status == TaskStatus.Running)
                    {
                        Debug.WriteLine("End: Waiting for nistStartThread to complete.");
                        previousNistEndThread.Wait();
                    }

                    EndTime = d;
                    RunHistory.AddRun(new RunHistory.Item(StartTime.Value, EndTime.Value, state.CurrentTime, _state.Run.GameName));
                    Debug.WriteLine("Added a new run.");
                });
            }

            _prevPhase = state.CurrentPhase;
        }

        void DeactivateDuplicateComponents()
        {
            if (!Activated || _state.Layout == null)
                return;

            var duplicateCount = _state.Layout.LayoutComponents.Count(c => c.Component is RunHighlighterComponent && ((RunHighlighterComponent)c.Component).Activated == true);
            if (duplicateCount < 2)
                return;

            var duplicates = _state.Layout.LayoutComponents.Where(c => c.Component is RunHighlighterComponent && ((RunHighlighterComponent)c.Component).Activated == true);
            RunHighlighterComponent firstComponent = null;
            foreach (ILayoutComponent c in duplicates)
            {
                var rh = (RunHighlighterComponent)c.Component;
                if (firstComponent == null || rh.ComponentCreationTime.CompareTo(firstComponent.ComponentCreationTime) < 0)
                {
                    if (firstComponent != null)
                        firstComponent.Deactivate();

                    firstComponent = rh;
                }
                else
                {
                    rh.Deactivate();
                }
            }
        }

        public void Deactivate()
        {
            this.Activated = false;
            _state.OnStart -= State_OnStart;
            _state.OnUndoSplit -= State_OnUndoSplit;
            this.ContextMenuControls = null;
        }

        public override void Dispose()
        {
            _state.OnStart -= State_OnStart;
            _state.OnUndoSplit -= State_OnUndoSplit;
        }

        public override XmlNode GetSettings(XmlDocument document)
        {
            if (Activated)
                return Settings.GetSettings(document);
            else
                return document.CreateElement("Settings");
        }

        public override Control GetSettingsControl(LayoutMode mode)
        {
            if (Activated)
                return Settings;
            else
                return null;
        }

        public override void SetSettings(XmlNode settings)
        {
            DeactivateDuplicateComponents();

            if (Activated)
                Settings.SetSettings(settings);
        }
    }
}
