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

        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        } 

        public static async Task<string> MainAsync()
        {
            Console.WriteLine("Enter twitch vod id: ");
            string vodId = Console.ReadLine();

            var response = await client.GetByteArrayAsync($"https://vod.544146.workers.dev/{vodId}");
            var source = Encoding.GetEncoding("utf-8").GetString(response, 0, response.Length - 1);
            source = WebUtility.HtmlDecode(source);
            HtmlDocument resultat = new HtmlDocument();
            resultat.LoadHtml(source);
            List<HtmlNode> toftitle = resultat.DocumentNode.Descendants().Where
                (x => (x.Name == "a")).ToList();

            string fullURL = toftitle[0].InnerText;
            var VLC = new System.Diagnostics.Process();
            VLC.StartInfo.FileName = "C:\\Program Files\\VideoLAN\\VLC\\vlc.exe";
            VLC.StartInfo.Arguments = $"-vvv {fullURL}";
            VLC.Start();

            return "stream opened";
        }
    }
}
