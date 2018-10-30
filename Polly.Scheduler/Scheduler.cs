using Polly.Data;
using RobotsSharpParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.SchedulerConsole
{
    public abstract class Scheduler
    {
        private Thread _mainDownloadThread;
        protected Website Website;

        public event EventHandler OnStart;
        public event EventHandler OnEnd;
        public event ProgressEventHandler OnProgress;

        public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);

        public Scheduler(Website website)
        {
            Website = website;
            _mainDownloadThread = new Thread(new ThreadStart(ThreadAction));
            _mainDownloadThread.Name = nameof(_mainDownloadThread);
        }

        public void Start()
        {
            _mainDownloadThread.Start();
        }

        public bool IsAlive => _mainDownloadThread.IsAlive;

        private void ThreadAction()
        {
            RaiseOnStart();
            QueueLinks().Wait();
            RaiseOnEnd();
        }

        private async Task QueueLinks()
        {
            try
            {
                Robots robots = new Robots(Website.Domain, Website.UserAgent, enableErrorCorrection: true);
                robots.Load();
                robots.OnPorgress += Robots_OnPorgress;
                var websiteLinksToDownload = await robots.GetSitemapLinksAsync();

                var filteredList = websiteLinksToDownload.Where(FilterProducts()).ToList();

                int totalRequestCount = 0;
                DateTime startTime = DateTime.Now;
                List<Task> saveTasks = new List<Task>();
                List<DownloadQueue> batch = new List<DownloadQueue>();
                int cTest = 0;
                foreach (tUrl websiteLink in filteredList)
                {
                    var downloadQueue = new DownloadQueue()
                    {
                        AddedDate = DateTime.Now,
                        DownloadUrl = BuildDownloadUrl(websiteLink.loc),
                        WebsiteId = Website.Id,
                        Priority = 5,
                    };

                    batch.Add(downloadQueue);
                    cTest++;
                    totalRequestCount++;

                    if (cTest == 1000)
                    {
                        await DataAccess.SaveAsync(batch);
                        RaiseOnProgress(totalRequestCount, filteredList.Count, startTime);
                        batch.Clear();
                        cTest = 0;
                    }
                }

                await DataAccess.SaveAsync(batch);
            }
            catch(Exception e)
            {

            }
        }

        private void Robots_OnPorgress(object sender, RobotsSharpParser.ProgressEventArgs e)
        {
            RaiseOnProgress($"Links from robots:{e.ProgressMessage}");
        }

        protected abstract string BuildDownloadUrl(string loc);
        protected abstract Func<tUrl, bool> FilterProducts();

        private void RaiseOnStart()
        {
            if (OnStart == null) return;

            OnStart(this, new EventArgs());
        }

        private void RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            if (OnProgress == null) return;

            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            string progressString = $"{requestCount} of {totalSize} {(requestCount * 1.0 / totalSize * 100.00):#.00}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
            OnProgress(this, new ProgressEventArgs(progressString));
        }

        private void RaiseOnProgress(string progressMessage)
        {
            OnProgress(this, new ProgressEventArgs(progressMessage));
        }

        private void RaiseOnEnd()
        {
            if (OnEnd == null) return;

            OnEnd(this, new EventArgs());
        }
    }

    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(string progressString)
        {
            ProgressString = progressString;
        }

        public string ProgressString { get; set; }
    }
}
