using Newtonsoft.Json;
using Polly.Data;
using Polly.Domain;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class Upload : SimpleWorker
    {
        readonly IDownloader _downloader;
        readonly ITakealotMapper _takealotMapper;
        readonly object _lock = new object();
        readonly object _writelock = new object();

        public Upload(IDownloader downloader, ITakealotMapper takealotMapper)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloader = downloader;
            _takealotMapper = takealotMapper;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            var startTime = DateTime.Now;
            int count = 0;
            var doneUrls = await GetDone();
            var toDo = await GetToDo(doneUrls);
            var total = toDo.Count;
            StreamWriter sw = new StreamWriter("done.txt", append: true);
            List<Task> tasks = new List<Task>();

            for (int i = 0; i < 15; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    while (toDo.TryDequeue(out string line))
                    {
                        var items = line.Split(',');
                        var url = items[1];
                        var httpResponse = await _downloader.DownloadAsync(url);
                        if (string.IsNullOrWhiteSpace(httpResponse))
                        {
                            lock (_writelock)
                            {
                                sw.WriteLine(url);
                            }
                            Interlocked.Increment(ref count);
                            continue;
                        }

                            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);

                        try
                        {
                            await _takealotMapper.MapAndSaveFullAsync(jsonObject);
                            lock (_writelock)
                            {
                                sw.WriteLine(url);
                            }
                        }
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            toDo.Enqueue(line);
                            Interlocked.Decrement(ref count);
                        }
                        catch (Exception)
                        {
                            await sw.WriteLineAsync(url);
                        }
                        Interlocked.Increment(ref count);
                        RaiseOnProgress(count, total, startTime);
                    }
                    count++;
                })
                .ContinueWith(ctask =>
                {
                    lock (_lock)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"A task ended unexpectedly:{ctask.Exception.Message}");
                    }
                }, TaskContinuationOptions.OnlyOnFaulted)
                .ContinueWith(async ctask =>
                {
                    await DataAccess.LogError(ctask.Exception);
                }, TaskContinuationOptions.OnlyOnCanceled));
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            finally
            {
                sw.Close();
                sw.Dispose();
            }
        }

        private async Task<ConcurrentQueue<string>> GetToDo(HashSet<string> done)
        {
            HashSet<string> toDoUrls = new HashSet<string>();

            using (StreamReader sr = new StreamReader("downloadLinks.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    var url = line.Split(',')[1];
                    if (done.Contains(url))
                        continue;
                    toDoUrls.Add(line);
                }
            }
            return new ConcurrentQueue<string>(toDoUrls);
        }

        private async Task<HashSet<string>> GetDone()
        {
            HashSet<string> doneUrls = new HashSet<string>();
            if (File.Exists("done.txt"))
                using (StreamReader sr = new StreamReader("done.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        doneUrls.Add(await sr.ReadLineAsync());
                    }
                }
            return doneUrls;
        }

        public override string ToString()
        {
            return "Upload";
        }
    }
}
