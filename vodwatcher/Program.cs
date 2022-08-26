using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;

namespace vodwatcher
{
    static class Constants 
    {
        public static string VLC_PATH = @"C:\ProgramFiles\VideoLAN\VLC\vlc.exe";
        public static string DEV_URL_PATH = @"https://vod.544146.workers.dev/";
    }
    class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        } 

        private static async Task MainAsync()
        {
            Console.WriteLine("Enter twitch vod id: ");
            string vodId = Console.ReadLine();
            string callPath = Constants.DEV_URL_PATH + vodId;
            string vlcUri = await GetVlcUri(callPath);
            OpenVlcWithUri(vlcUri);
        }

        public static async Task<string> GetVlcUri(string callpath)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetByteArrayAsync(callpath);
            var source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument webPageReturned = new HtmlDocument();
            webPageReturned.LoadHtml(source);
            List<HtmlNode> anchorNodes = webPageReturned.DocumentNode.Descendants().Where
                (x => (x.Name == "a")).ToList();

            // a few anchor nodes are returned, just grab the first one
            string fullURL = anchorNodes[0].InnerText;
            return fullURL;
        }

        public static void OpenVlcWithUri(string path) 
        {
            var VLC = new System.Diagnostics.Process();
            VLC.StartInfo.FileName = Constants.VLC_PATH;
            VLC.StartInfo.Arguments = $"-vvv {path}";
            VLC.Start();
        }
    }
}
