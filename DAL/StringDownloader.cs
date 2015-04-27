using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public static class StringDownloader
    {
        public static string DownloadStringFromURL(string url)
        {
            string json;
            using (var webClient = new System.Net.WebClient())
            {
                json = webClient.DownloadString(url);
            }
            return json;
        }

    }
}
