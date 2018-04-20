using Polly.Data;
using Polly.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.DownloadConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            var website = DataAccess.GetWebsiteById(1);
            Downloader.Downloader downloader = new Downloader.Downloader(website);
            downloader.OnStart += Downloader_OnStart;
            downloader.OnEnd += Downloader_OnEnd;
            downloader.Start();

            while(downloader.IsAlive)
            {
                if (Console.ReadLine() == "stop")
                    return;
            }
        }

        private static void Downloader_OnStart(object sender, EventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now}] Started");
        }

        private static void Downloader_OnEnd(object sender, EventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now}] Stopped");
        }
    }
}
