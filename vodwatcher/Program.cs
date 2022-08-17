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

    
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private const string _VLC_PATH = @"C:\ProgramFiles\VideoLAN\VLC\vlc.exe";
        private const string _DEV_URL_PATH = @"https://vod.544146.workers.dev/";
        private string _VOD_ID = String.Empty;
        private string _CALL_PATH = String.Empty;

        public static void Main(string[] args)
        {
            SetCallPath();
            string vlcUri = GetVlcUri();
            OpenVlcWithUri(vlcUri);
        } 

        public static void SetCallPath() 
        {
            Console.WriteLine("Enter twitch vod id: ");
            _VOD_ID = Console.ReadLine();
            _CALL_PATH = _DEV_URL_PATH + _VOD_ID;
        }

        public static string GetVlcUri()
        {
            var response = await client.GetByteArrayAsync(_CALL_PATH);
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
            VLC.StartInfo.FileName = _VLC_PATH;
            VLC.StartInfo.Arguments = $"-vvv {fullURL}";
            VLC.Start();
        }
    }
}
