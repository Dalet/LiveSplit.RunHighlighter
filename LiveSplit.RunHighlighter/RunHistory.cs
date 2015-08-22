using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.RunHighlighter
{
    public class RunHistory
    {
        public class Run
        {
            public DateTime UtcStart { get; set; }
            public DateTime UtcEnd { get; set; }
            public bool IsUtcStartReliable { get; set; }
            public bool IsUtcEndReliable { get; set; }
            public Time Time { get; set; }
            public string Game { get; set; }
            public string Category { get; set; }

            public string TimeString
            {
                get
                {
                    var formatter = new TimeFormatters.RegularTimeFormatter(TimeFormatters.TimeAccuracy.Seconds);
                    return formatter.Format(Time.GameTime) + " / " + formatter.Format(Time.RealTime);
                }
            }

            public string TimeElapsedStringSince(DateTime utcNow)
            {
                string numberName;
                double number = 0;
                TimeSpan t = utcNow - this.UtcEnd;

                if (t.TotalDays >= 30)
                {
                    numberName = "month";
                    number = t.TotalDays / 30;
                }
                else if (t.TotalHours >= 24)
                {
                    numberName = "day";
                    number = t.TotalHours / 24;
                }
                else if (t.TotalMinutes >= 60)
                {
                    numberName = "hour";
                    number = t.TotalMinutes / 60;
                }
                else if (t.TotalSeconds >= 60)
                {
                    numberName = "minute";
                    number = t.TotalSeconds / 60;
                }
                else
                {
                    numberName = "second";
                    number = t.TotalSeconds;
                }

                number = number > 0 ? Math.Round(number) : 1;
                var plural = number > 1 ? "s" : String.Empty;

                return number + " " + numberName + plural + " ago";
            }
            public string TimeElapsedString => TimeElapsedStringSince(DateTime.UtcNow);

            public static Run AttemptToRun(Attempt attempt, IRun splits)
            {
                return new Run
                {
                    UtcStart = attempt.Started.Value.ToUniversalTime(),
                    UtcEnd = attempt.Ended.Value.ToUniversalTime(),
                    Time = attempt.Time,
                    Game = splits.GameName,
                    Category = splits.CategoryName
                };
            }
        }

        public static IList<Run> GetRunHistory(IRun splits, int maxCount = -1)
        {
            var attemptHistory = splits.AttemptHistory.Where(a => a.Started != null && a.Ended != null && a.Time.RealTime != null).ToArray();
            var history = new List<Run>();

            int i = attemptHistory.Length;
            while (--i >= 0)
            {
                if (maxCount != -1 && history.Count >= maxCount)
                    break;

                history.Add(Run.AttemptToRun(attemptHistory[i], splits));
            }

            return history;
        }
    }
}
