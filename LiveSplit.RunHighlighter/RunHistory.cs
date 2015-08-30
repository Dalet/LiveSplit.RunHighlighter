using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveSplit.RunHighlighter
{
    public static class RunHistory
    {
        public struct Run
        {
            public Attempt Attempt { get; }
            public DateTime UtcStart => Attempt.Started.Value.Time.ToUniversalTime();
            public DateTime UtcEnd => Attempt.Ended.Value.Time.ToUniversalTime();
            public bool IsUtcStartReliable => Attempt.Started.Value.SyncedWithAtomicClock;
            public bool IsUtcEndReliable => Attempt.Ended.Value.SyncedWithAtomicClock;
            public Time Time => Attempt.Time;
            public string Game { get; }
            public string Category { get; }

            public Run(Attempt attempt, string game, string category)
            {
                Attempt = attempt;
                Game = game;
                Category = category;
            }

            public Run(Attempt attempt, IRun splits)
            {
                Attempt = attempt;
                Game = splits.GameName;
                Category = splits.CategoryName;
            }

            public string TimeString
            {
                get
                {
                    var formatter = new TimeFormatters.RegularTimeFormatter(TimeFormatters.TimeAccuracy.Seconds);
                    var gt = Time.GameTime != null && Time.RealTime != Time.GameTime
                        ? formatter.Format(Time.GameTime) + " / "
                        : "";
                    var rt = formatter.Format(Time.RealTime);
                    return gt + rt;
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
        }

        public static IList<Run> GetRunHistory(IRun splits, int maxCount = -1, bool hideUnreliable = false)
        {
            var attemptHistory = splits.AttemptHistory
                .Where(a => a.Started != null && a.Ended != null && a.Time.RealTime != null &&
                    (!hideUnreliable || (a.Started.Value.SyncedWithAtomicClock && a.Ended.Value.SyncedWithAtomicClock)))
                .ToArray();
            var history = new List<Run>();

            int i = attemptHistory.Length;
            while (--i >= 0)
            {
                if (maxCount != -1 && history.Count >= maxCount)
                    break;

                history.Add(new Run(attemptHistory[i], splits));
            }

            return history;
        }

        public static IList<string> HistoryToString(IEnumerable<Run> runs)
        {
            var runHistory = new List<string>();
            Func<string, string, int, string> AppendString = (src, c, number) =>
            {
                for (int i = 0; number > i; i++)
                    src += c;
                return src;
            };

            char[] oneSpaceWideChars = new char[] { ':', ' ' };
            int oneSpaceWideCharsMax = runs.Max(r => r.TimeString.Count(c => oneSpaceWideChars.Contains(c)));
            int twoSpacesWideCharsMax = runs.Max(r => r.TimeString.Count(c => !oneSpaceWideChars.Contains(c)));

            foreach (var run in runs)
            {
                var timeDesc = run.TimeString;

                var timeDescSpaces = AppendString("", " ", oneSpaceWideCharsMax - timeDesc.Count(c => oneSpaceWideChars.Contains(c)));
                timeDescSpaces = AppendString(timeDescSpaces, "  ", twoSpacesWideCharsMax - timeDesc.Count(c => !oneSpaceWideChars.Contains(c)));

                var runDesc = timeDescSpaces + timeDesc + "  -  " + run.TimeElapsedString;

                runHistory.Add(runDesc);
            }

            return runHistory;
        }
    }
}
