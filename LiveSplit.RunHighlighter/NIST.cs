using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LiveSplit.RunHighlighter
{
    public static class NIST
    {
        static int? _requestCooldownTimestamp;  
        static int? _lastRequestTimestamp;
        static DateTime? _lastResponse;

        static readonly TimeSpan RequestDelay = TimeSpan.FromMinutes(10);

        public static bool UtcNow(out DateTime ret, bool requestServer = true)
        {
            var funcStartTickCount = Environment.TickCount;

            TimeSpan? timeSinceLastRequest = null;
            if(_lastRequestTimestamp != null)
                timeSinceLastRequest = TimeSpan.FromMilliseconds((double)(funcStartTickCount - _lastRequestTimestamp));

            DateTime localUTC = DateTime.UtcNow;

            var cooldown = _requestCooldownTimestamp != null
                ? TimeSpan.FromMilliseconds((double)(funcStartTickCount - _requestCooldownTimestamp))
                : TimeSpan.FromHours(42);

            if (requestServer && cooldown >= RequestDelay)
            {
                var response = GetNISTDate();                

                if (response != null)
                {
                    Debug.WriteLine("NIST Response: " + response);
                    _lastRequestTimestamp = funcStartTickCount;
                    _requestCooldownTimestamp = Environment.TickCount;
                    _lastResponse = response;
                    ret = response.Value;
                    return true;
                }
                else
                {
                    Debug.WriteLine("Failed to retrieve time from server.");
                    return UtcNowBackup(timeSinceLastRequest, localUTC, out ret);
                }
            }
            else
            {
                Debug.WriteLineIf(requestServer, "NIST server request cooldown: " + (RequestDelay - cooldown));
                return UtcNowBackup(timeSinceLastRequest, localUTC, out ret);
            }
        }

        public static Task UtcNowAsync(Action<DateTime, bool> callback)
        {
            return Task.Factory.StartNew(() =>
            {
                DateTime time;
                bool serverSuccess = UtcNow(out time);
                callback(time, serverSuccess);
            });
        }

        private static bool UtcNowBackup(TimeSpan? timeSinceLastRequest, DateTime localTime, out DateTime ret)
        {
            if (_lastResponse != null) //just add the time elapsed since the last request instead
            {
                TimeSpan time = timeSinceLastRequest ?? TimeSpan.Zero;
                ret = _lastResponse.Value + time;

                Debug.WriteLine("Used last response as backup.");
                Debug.WriteLine($"NIST: {ret} (Local UTC: {localTime})");

                return true;
            }
            else //if getting the time from the server failed use local
            {
                ret = localTime;

                Debug.WriteLine("Used local time as backup.");
                Debug.WriteLine("Local UTC: " + ret);

                return false;
            }
        }

        private static DateTime? GetNISTDate()
        {
            var funcStart = Stopwatch.StartNew();

            DateTime? date = null;
            string serverResponse = string.Empty;

            try
            {
                // Open a StreamReader to a time server
                using (var tcpClient = new System.Net.Sockets.TcpClient())
                {
                    tcpClient.ConnectAsync("time.nist.gov", 13).Wait(5000);
                    using (var reader = new System.IO.StreamReader(tcpClient.GetStream()))
                    {
                        serverResponse = reader.ReadToEnd();
                    }
                }

                // Check to see that the signature is there
                if (serverResponse.Length > 47 && serverResponse.Substring(38, 9).Equals("UTC(NIST)"))
                {
                    // Parse the date
                    int jd = int.Parse(serverResponse.Substring(1, 5));
                    int yr = int.Parse(serverResponse.Substring(7, 2));
                    int mo = int.Parse(serverResponse.Substring(10, 2));
                    int dy = int.Parse(serverResponse.Substring(13, 2));
                    int hr = int.Parse(serverResponse.Substring(16, 2));
                    int mm = int.Parse(serverResponse.Substring(19, 2));
                    int sc = int.Parse(serverResponse.Substring(22, 2));

                    if (jd > 51544)
                        yr += 2000;
                    else
                        yr += 1999;

                    date = new DateTime(yr, mo, dy, hr, mm, sc);
                }
            }
            catch { return null; }

            return date - funcStart.Elapsed; //try to correct the connection delay
        }
    }
}
