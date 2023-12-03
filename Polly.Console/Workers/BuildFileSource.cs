using Polly.Domain;
using RobotsParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Console.Workers
{
    internal class BuildFileSource : IAsyncWorker
    {
        private readonly IDownloader _downloader;
        private const string SourceFile = "SourceUrls.txt";
        private const string TakealotApi = "https://api.takealot.com/rest/v-1-11-0/product-details";

        public BuildFileSource(IDownloader downloader)
        {
            _downloader = downloader;
        }

        public async Task DoWorkAsync(CancellationToken token)
        {
            var robots = new Robots(_downloader.DownloadAsync, true);
            await robots.LoadRobotsFromUrl("https://takealot.com");

            foreach (var index in await robots.GetSitemapIndexes())
            {
                List<string> urls = new List<string>();

                foreach (var url in await robots.GetIndexUrls(index)) 
                {
                    int lastindex = url.LastIndexOf('/');
                    urls.Add(string.Concat(TakealotApi, url.Substring(lastindex, url.Length - lastindex), "?platform=desktop&display_credit=true"));
                }

                await File.AppendAllLinesAsync(SourceFile, urls, token);
            }    
            
        }
    }
}
