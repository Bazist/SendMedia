using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SendMedia
{
    public class GiphySearch
    {
        public delegate void DownloadFileCompletedHandler(string fileName, string url);

        public event DownloadFileCompletedHandler DownloadFileCompleted;

        public void Search(string searchQuery)
        {
            var rawResponse = new WebClient().DownloadString("https://api.giphy.com/v1/gifs/search?q=" + searchQuery + "&api_key=dc6zaTOxFJmzC");
            JObject response = JObject.Parse(rawResponse);
            JArray photos = (JArray)response["data"];

            foreach (JObject photo in photos)
            {
                DownloadGiphy(photo["images"]["fixed_height"]["url"].ToString());
            }
        }

        private void DownloadGiphy(string url)
        {
            WebClient client = new WebClient();
            client.DownloadFileCompleted += Client_DownloadFileCompleted;

            // Starts the download
            var gif = new Gif { Filename = System.IO.Path.GetTempFileName(), Url = url };
            client.DownloadFileAsync(new Uri(gif.Url), gif.Filename, gif);
        }

        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Gif gif = (Gif)e.UserState;

            DownloadFileCompleted(gif.Filename, gif.Url);
        }
    }
}
