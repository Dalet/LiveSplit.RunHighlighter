using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch = LiveSplit.RunHighlighter.TwitchExtension;

namespace LiveSplit.RunHighlighter
{
    public class HighlightInfo
    {
        public RunHistory.Item Run { get; private set; }
        public dynamic Video { get; private set; }
        public bool IsStartOutOfVideo { get; private set; }
        public bool IsEndOutOfVideo { get; private set; }
        public TimeSpan Buffer { get; set; }

        public bool IsOutOfVideo
        {
            get { return IsStartOutOfVideo || IsEndOutOfVideo; }
        }

        public string ManagerURL
        {
            get { return String.Format("twitch.tv/{0}/manager/{1}/highlight", Video.channel.name, Video._id); }
        }

        public Uri ManagerURI
        {
            get { return new Uri("http://" + ManagerURL); }
        }

        public TimeSpan StartTime
        {
            get
            {
                var time = _startTime - Buffer;
                return (time > new TimeSpan(0)) ? time : new TimeSpan(0);
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                var time = _endTime + Buffer;
                return (!Video.is_incomplete && (int)time.TotalSeconds > Video.length) ? TimeSpan.FromSeconds(Video.length) : time;
            }
        }

        public string StartTimeString
        {
            get { return HighlightTimeString(StartTime); }
        }

        public string EndTimeString
        {
            get { return HighlightTimeString(EndTime); }
        }

        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public HighlightInfo(dynamic video, RunHistory.Item run, int buffer = RunHighlighterSettings.DEFAULT_HIGHLIGHT_BUFFER)
        {
            this.Video = video;
            this.Run = run;
            this.Buffer = TimeSpan.FromSeconds(buffer);

            Initialize();
        }

        private void Initialize()
        {
            DateTime start = Twitch.ParseDate(Video.recorded_at);

            _startTime = Run.UtcStart.Subtract(start);
            _startTime = _startTime < new TimeSpan(0) ? new TimeSpan(0) : _startTime;

            if (Video.is_incomplete && _startTime.TotalSeconds > Video.length)
                IsStartOutOfVideo = true;

            _endTime = (Run.UtcEnd - Run.UtcStart) + _startTime;

            if (Video.is_incomplete && _endTime.TotalSeconds > Video.length)
                IsEndOutOfVideo = true;
        }

        public static string HighlightTimeString(TimeSpan t)
        {
            var hours = (int)t.TotalHours > 0 ? (int)t.TotalHours + ":" : "";
            return hours + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
        }
    }
}
