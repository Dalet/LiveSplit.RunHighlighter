﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Twitch = LiveSplit.RunHighlighter.TwitchExtension;

namespace LiveSplit.RunHighlighter
{
    public class HighlightInfo
    {
        public TimeSpan Buffer { get; set; }
        public bool TruncateTimes { get; set; }
        public string RawTitle { get; set; }
        public string RawDescription { get; set; }

        public RunHistory.Run Run { get; private set; }
        public dynamic Video { get; private set; }
        public bool IsStartOutOfVideo { get; private set; }
        public bool IsEndOutOfVideo { get; private set; }
        public bool IsOutOfVideo => IsStartOutOfVideo || IsEndOutOfVideo;
        public string Title => FormatText(RawTitle);
        public string Description => FormatText(RawDescription);
        public string ManagerURL => String.Format("twitch.tv/{0}/manager/{1}/highlight?start_time={2}&end_time={3}&title={4}",
				Video.channel.name, Video._id, (int)StartTime.TotalSeconds, (int)EndTime.TotalSeconds,
				Uri.EscapeDataString(Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(Title))));
        public Uri ManagerURI => new Uri("http://" + ManagerURL);
        public string StartTimeString => HighlightTimeString(StartTime);
        public string EndTimeString => HighlightTimeString(EndTime);

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

        private RunHighlighterSettings _settings;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public HighlightInfo(dynamic video, RunHistory.Run run, RunHighlighterSettings settings = null)
        {
            this._settings = settings;
            this.Video = video;
            this.Run = run;
            this.Buffer = _settings != null ? TimeSpan.FromSeconds(_settings.HighlightBuffer) : TimeSpan.FromSeconds(RunHighlighterSettings.DEFAULT_HIGHLIGHT_BUFFER);
            this.TruncateTimes = _settings != null ? _settings.TruncateTimes : RunHighlighterSettings.DEFAULT_TRUNCATE_TIMES;
            this.RawTitle = _settings != null ? _settings.TitleText : RunHighlighterSettings.DEFAULT_TITLE_TEXT;
            this.RawDescription = _settings != null ? _settings.DescriptionText : RunHighlighterSettings.DEFAULT_DESCRIPTION_TEXT;

            Initialize();
        }

        private void Initialize()
        {
            if (Video != null)
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
        }

        public string FormatText(string raw)
        {
            var match = new Regex(@"\[RT!=GT\]([\s\S]*)\[/RT!=GT\]").Match(raw);

            while (match.Success)
            {
                if (Run.Time.GameTime != null && Run.Time.GameTime.Value != Run.Time.RealTime.Value)
                {
                    raw = raw.Replace("[RT!=GT]", "");
                    raw = raw.Replace("[/RT!=GT]", "");
                }
                else
                {
                    raw = raw.Replace("[RT!=GT]" + match.Groups[1].Value + "[/RT!=GT]", "");
                }

                match = match.NextMatch();
            }

            string twitchName = "cosmowright";

            if (Video != null)
                twitchName = Video.channel.display_name;
            else if (!String.IsNullOrEmpty(_settings.TwitchUsername))
                twitchName = _settings.TwitchUsername;
            else if (Twitch.Instance.IsLoggedIn)
                twitchName = Twitch.Instance.ChannelName;

            var rtStr = HighlightTimeString(Run.Time.RealTime.Value, TruncateTimes);
            var gtStr = Run.Time.GameTime != null
                ? HighlightTimeString(Run.Time.GameTime.Value, TruncateTimes)
                : rtStr;

            var keywords = new Dictionary<string, string>
            {
                {"$realtime", rtStr},
                {"$gametime", gtStr},
                {"$twitchchannel", twitchName},
                {"$game", Run.Game},
                {"$category", Run.Category}
            };

            foreach (var key in keywords.Keys)
            {
                raw = raw.Replace(key, keywords[key]);
            }

            return raw;
        }

        public static string HighlightTimeString(TimeSpan t, bool truncate = false)
        {
            if (!truncate && t.Milliseconds >= 500)
                t += TimeSpan.FromSeconds(1);

            var hours = (int)t.TotalHours > 0 ? (int)t.TotalHours + ":" : "";
            return hours + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
        }
    }
}
