﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LiveSplit.RunHighlighter
{
    public static class NIST
    {      
        static DateTime _lastTimeRequest = new DateTime(0, DateTimeKind.Utc);
        static DateTime? _lastResponse;


        public static bool UtcNow(out DateTime ret, bool requestServer = true)
        {
            TimeSpan timeSinceLastRequest = DateTime.UtcNow - _lastTimeRequest;
            DateTime localUTC = DateTime.UtcNow;

            if (requestServer && timeSinceLastRequest >= TimeSpan.FromMinutes(10))
            {
                var previousLastTimeRequest = _lastTimeRequest;
                _lastTimeRequest = DateTime.UtcNow;

                var response = GetNISTDate();

                if (response != null)
                {
                    Debug.WriteLine("NIST Response: " + response);
                    _lastResponse = response;
                    ret = response.Value;
                    return true;
                }
                else
                {
                    Debug.WriteLine("Failed to retrieve time from server.");
                    _lastTimeRequest = previousLastTimeRequest;
                    return UtcNowBackup(localUTC, out ret);
                }
            }
            else
            {
                Debug.WriteLineIf(requestServer, "NIST server request cooldown: " + (TimeSpan.FromMinutes(10) - timeSinceLastRequest));
                return UtcNowBackup(localUTC, out ret);
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

        private static bool UtcNowBackup(DateTime localTime, out DateTime ret)
        {
            if (_lastResponse != null) //just add the time elapsed since the last request instead
            {
                TimeSpan timeSinceLastRequest = localTime - _lastTimeRequest;

                ret = _lastResponse.Value + timeSinceLastRequest;

                Debug.WriteLine("Used last response as backup.");
                Debug.WriteLine("NIST: " + ret + " Local UTC: " + DateTime.UtcNow);

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
            DateTime funcStart = DateTime.UtcNow;

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

            return date - (DateTime.UtcNow - funcStart); //try to correct the connection delay
        }
    }
}
