using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

namespace LiveSplit.RunHighlighter
{
    public class RunHighlighterFactory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Run Highlighter"; }
        }

        public string Description
        {
            get { return "Helps you highlight your runs faster."; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Other; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new RunHighlighterComponent(state);
        }

        public string UpdateName
        {
            get { return this.ComponentName; }
        }

        public string UpdateURL
        {
            get { return "https://raw.githubusercontent.com/Dalet/LiveSplit.RunHighlighter/master/"; }
        }

        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public string XMLURL
        {
            get { return this.UpdateURL + "Components/update.LiveSplit.RunHighlighter.xml"; }
        }
    }
}
