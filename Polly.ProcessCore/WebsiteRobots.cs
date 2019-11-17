using RobotsCoreParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.ProcessCore
{
    public class WebsiteRobots
    {
        public async Task DownloadWebsiteRobotsLinks(string website, string saveToFile)
        {
            var robots = new Robots(website, "RobotsCoreParser");
            robots.OnProgress += Robots_OnProgress;
            await robots.LoadAsync();
            var indexes = await robots.GetSitemapIndexesAsync();
            foreach (var index in indexes)
            {
                var indexUrls = await robots.GetUrlsAsync(index);
                await File.AppendAllLinesAsync(saveToFile, indexUrls.Select(x => x.loc));
            }
        }

        private void Robots_OnProgress(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.ProgressMessage);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
        }
    }
}
