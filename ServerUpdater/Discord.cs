using System;
using System.IO;
using System.Net;
using System.Text;

namespace ServerUpdater
{
    class Discord
    {
        private const int MAX_MESSAGE_LENGTH = 2000;
        private const int REQUEST_TIMEOUT = 30;

        #region Singleton

        private static readonly Lazy<Discord> _instance = new Lazy<Discord>(() => new Discord());

        public static Discord Instance
        {
            get { return _instance.Value; }
        }

        #endregion // Singleton

        public void Broadcast(string message)
        {
            if (string.IsNullOrWhiteSpace(Settings.Instance.DiscordWebHookURL) || string.IsNullOrWhiteSpace(message))
                return;

            var postData = string.Empty;

            if (Settings.Instance.DiscordBotUseTTS)
                postData += $"&tts={Settings.Instance.DiscordBotUseTTS}";
            if (!string.IsNullOrWhiteSpace(Settings.Instance.DiscordBotName))
                postData += $"&username={Settings.Instance.DiscordBotName.Replace("&", "_")}";
            postData += $"&content=";
            postData += $"{message.Replace("&", "_")}";

            if (postData.Length > MAX_MESSAGE_LENGTH)
                postData = $"{postData.Substring(0, MAX_MESSAGE_LENGTH - 3)}...";

            try
            {
                var data = Encoding.ASCII.GetBytes(postData);

                var url = Settings.Instance.DiscordWebHookURL;
                url = url.Trim();
                if (url.EndsWith("/"))
                    url = url.Substring(0, url.Length - 1);

                var httpRequest = WebRequest.Create($"{url}?wait=true");
                httpRequest.Timeout = REQUEST_TIMEOUT;
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                httpRequest.ContentLength = data.Length;

                using (var stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                var responseString = new StreamReader(httpResponse.GetResponseStream()).ReadToEnd();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Log.LogToConsole($"Discord.{nameof(Broadcast)}\r\nResponse: {responseString}");
                }
                else
                {
                    Log.LogToConsole($"Discord.{nameof(Broadcast)}\r\n{httpResponse.StatusCode}: {responseString}");
                }
            }
            catch (Exception ex)
            {
                Log.LogToConsole($"ERROR: Discord.{nameof(Broadcast)}\r\n{ex.Message}");
            }
        }
    }
}
