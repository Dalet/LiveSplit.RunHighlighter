using LiveSplit.Web.Share;
using System;
using System.Collections.Generic;

namespace LiveSplit.RunHighlighter
{
    public class TwitchExtension : Twitch
    {
        protected new static readonly TwitchExtension _Instance = new TwitchExtension();
        public new static TwitchExtension Instance { get { return _Instance; } }

        public IEnumerable<dynamic> GetPastBroadcasts(string username)
        {
            return curl(String.Format("channels/{0}/videos?broadcasts=true", username)).videos;
        }

        public dynamic GetStream(string username)
        {
            return curl(String.Format("streams/{0}", username));
        }

        public static DateTime ParseDate(string d)
        {
            var date = d.Remove(d.IndexOf("T")).Split('-');
            var year = date[0];
            var month = date[1];
            var day = date[2];

            var time = d.Remove(0, d.IndexOf("T") + 1).Split(':');
            var hour = time[0];
            var minute = time[1];
            var second = time[2].Remove(time[2].IndexOf('Z'));

            return new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), int.Parse(hour), int.Parse(minute), int.Parse(second), DateTimeKind.Utc);
        }

        public static bool IsValidUsername(string name)
        {
            var allowCharsRegex = new System.Text.RegularExpressions.Regex(@"([A-Z]|[a-z]|\d|_)");
            return !string.IsNullOrWhiteSpace(name) && allowCharsRegex.Matches(name).Count == name.Length;
        }
    }
}
