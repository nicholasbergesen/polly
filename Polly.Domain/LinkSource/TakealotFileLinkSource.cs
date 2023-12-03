using Polly.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Domain.LinkSource
{
    public class TakealotFileLinkSource : ILinkSource
    {
        private const string TakealotApi = "https://api.takealot.com/rest/v-1-11-0/product-details";

        public async Task<IEnumerable<DownloadQueueRepositoryItem>> GetNextBatchAsync(int batchSize)
        {
            return new List<DownloadQueueRepositoryItem>();
            //if (args.Length == 0)
            //{
            //    return;
            //}

            //var files = Directory.GetFiles("D:\\priceboar\\Pricebaor.Crawler\\source", " *.txt", SearchOption.TopDirectoryOnly);
            //var done = (await File.ReadAllLinesAsync("done.txt")).ToHashSet();

            //foreach (var file in files)
            //{
            //    using StreamReader streamReader = new StreamReader(file);
            //    while (!streamReader.EndOfStream)
            //    {
            //        var line = await streamReader.ReadLineAsync();

            //        if (string.IsNullOrWhiteSpace(line)
            //            || done.Contains(line)) continue;

            //        var apiUrl = BuildApiUrl(line);
            //        var response = await DownloadFunc(apiUrl);

            //    }
            //}
        }

        protected string BuildDownloadUrl(string url)
        {
            int lastindex = url.LastIndexOf('/');
            return string.Concat(TakealotApi, url.Substring(lastindex, url.Length - lastindex), "?platform=desktop&display_credit=true");
        }
    }
}
