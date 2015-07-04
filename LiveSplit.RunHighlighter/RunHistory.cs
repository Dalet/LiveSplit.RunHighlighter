﻿using LiveSplit.Model;
using LiveSplit.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace LiveSplit.RunHighlighter
{
    public static class RunHistory
    {
        public const string RUN_HISTORY_FILE_NAME = "RunHistory.json";

        public static Exception LastException { get; private set; }
        private static uint _maxHistoryCount = 50;
        public static uint MaxHistoryCount
        {
            get
            {
                return _maxHistoryCount;
            }
            set
            {
                _maxHistoryCount = value;
            }
        }

        public struct Item
        {
            public DateTime UtcStart { get; private set; }
            public DateTime UtcEnd { get; private set; }
            public Time Time { get; private set; }
            public string Game { get; private set; }

            public Item(DateTime utcStart, DateTime utcEnd, Time time, string game = null)
                : this()
            {
                this.UtcStart = utcStart;
                this.UtcEnd = utcEnd;
                this.Time = time;
                this.Game = game;
            }

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

            public string TimeElapsedString
            {
                get
                {
                    return TimeElapsedStringSince(DateTime.UtcNow);
                }
            }

            public dynamic ToJson()
            {
                dynamic json = new DynamicJsonObject();

                json.utc_start = UtcStart.Ticks;
                json.utc_end = UtcEnd.Ticks;
                json.time = Time.ToJson();
                json.game = Game;

                return json;
            }

            public static Item JsonToRun(dynamic json)
            {
                try
                {
                    DateTime utcStart = new DateTime(long.Parse(json.utc_start));
                    DateTime utcEnd = new DateTime(long.Parse(json.utc_end));

                    TimeSpan realTime = TimeSpan.Parse(json.time.realTime);
                    TimeSpan? gameTime = json.time.gameTime != null ? TimeSpan.Parse(json.time.gameTime) : null;
                    var time = new Time(realTime, gameTime);

                    return new Item(utcStart, utcEnd, time, json.game);
                }
                catch
                {
                    throw new ArgumentException("Invalid configuration file.");
                }
            }
        }

        public static bool AddRun(Item run)
        {
            if (run.UtcStart == null || run.UtcEnd == null)
                throw new ArgumentException();

            var runs = GetRunHistory();
            if (runs != null)
            {
                runs.Insert(0, run);
                return WriteRunHistory(runs);
            }

            return false;
        }

        public static bool RemoveLastRun()
        {
            var runs = GetRunHistory();
            if (runs != null)
            {
                if (runs.Count == 0)
                    return true;

                runs.RemoveAt(0);
                return WriteRunHistory(runs);
            }

            return false;
        }

        public static bool RemoveLastRun(DateTime utcStart, DateTime utcEnd)
        {
            var runs = GetRunHistory();
            if (runs != null)
            {
                if (runs.Count == 0)
                    return true;

                if (runs[0].UtcStart == utcStart && runs[0].UtcEnd == utcEnd)
                {
                    runs.RemoveAt(0);
                    return WriteRunHistory(runs);
                }
                else
                    Debug.WriteLine("!! Tried to remove the wrong run");
            }

            return false;
        }

        static bool WriteRunHistory(IList<Item> runs)
        {
            var funcStart = DateTime.UtcNow;
            List<dynamic> jsonRuns = new List<dynamic>();

            foreach (Item run in runs)
            {
                jsonRuns.Add(run.ToJson());
                if (jsonRuns.Count == MaxHistoryCount)
                    break;
            }

            dynamic s = new DynamicJsonObject();
            s.runs = jsonRuns;

            try
            {
                File.WriteAllText(Application.StartupPath + "\\" + RUN_HISTORY_FILE_NAME, "" + s);
                Debug.WriteLine("WriteRunHistory() runtime: " + (DateTime.UtcNow - funcStart));
                return true;
            }
            catch (Exception e)
            {
                LastException = e;
                Trace.WriteLine("Couldn't write to the config file. Message: " + e.Message);
            }

            Debug.WriteLine("WriteRunHistory() runtime: " + (DateTime.UtcNow - funcStart));
            return false;
        }

        public static IList<Item> GetRunHistory()
        {
            var funcStart = DateTime.UtcNow;
            var runs = new List<Item>();
            dynamic json;

            if (File.Exists(Application.StartupPath + "\\" + RUN_HISTORY_FILE_NAME))
            {
                try
                {
                    try
                    {
                        json = JSON.FromString(File.ReadAllText(Application.StartupPath + "\\" + RUN_HISTORY_FILE_NAME));
                    }
                    catch (ArgumentException)
                    {
                        LastException = new ArgumentException("Invalid config file.");
                        Trace.WriteLine("Couldn't read the config file. Message: " + LastException.Message);
                        return null;
                    }

                    foreach (dynamic run in json.runs)
                    {
                        runs.Add(Item.JsonToRun(run));
                        if (runs.Count == MaxHistoryCount)
                            break;
                    }
                }
                catch (Exception e)
                {
                    LastException = e;
                    Trace.WriteLine("Couldn't read the config file. Message: " + LastException.Message);
                    return null;
                }
            }

            Debug.WriteLine("GetRunHistory() runtime: " + (DateTime.UtcNow - funcStart));
            return runs;
        }
    }
}