using LiveSplit.Web.Share;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LiveSplit.RunHighlighter
{
    public class TwitchExtension : Twitch
    {
        protected new static readonly TwitchExtension _Instance = new TwitchExtension();
        public new static TwitchExtension Instance { get { return _Instance; } }

        public IEnumerable<dynamic> GetPastBroadcasts(string username)
        {
            return curl(String.Format("channels/{0}/videos?broadcasts=true", username)).videos as IEnumerable<dynamic>;
        }

        public dynamic GetStream(string username)
        {
            return curl(String.Format("streams/{0}", username));
        }

        public dynamic SearchRunBroadcast(string channel, RunHistory.Run run)
        {
            try
            {
                dynamic videos;
                if ((videos = Instance.GetPastBroadcasts(channel)) == null)
                    return null;

                dynamic streamInfo = Instance.GetStream(channel);

                int i = 0;
                foreach (dynamic video in videos)
                {
                    video.length = (double)video.length; //length is of decimal type sometimes

                    DateTime videoStart = ParseDate(video.recorded_at);
                    DateTime videoEnd = videoStart.Add(TimeSpan.FromSeconds(video.length));
                    video.stream = streamInfo.stream; // null if stream is offline
                    video.latest_video = i == 0;
                    video.is_incomplete = video.status == "recording" || (video.latest_video && video.stream != null);

                    if (videoStart <= run.UtcStart + TimeSpan.FromSeconds(12)
                        && (run.UtcEnd - TimeSpan.FromSeconds(12) <= videoEnd || video.is_incomplete))
                    {
                        return video;
                    }

                    i++;
                }
            }
            catch (Exception e)
            {
                string message = String.Empty;

                if (e is System.Net.WebException)
                {
                    message = "An error occured while trying to reach Twitch's API.";
                }
                else
                    message = "An unexpected error occured while trying to retrieve the video.";

                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
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
