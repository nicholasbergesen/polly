using Newtonsoft.Json;
using Polly.ConsoleNet.Properties;
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
        private StreamWriter _doneTxt;

        public Upload(IDownloader downloader, ITakealotMapper takealotMapper)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloader = downloader;
            _takealotMapper = takealotMapper;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            int count = 0;
            var doneUrls = await GetDone();
            var toDo = await GetToDo(doneUrls);
            _doneTxt = new StreamWriter("done.txt", append: true);
            var total = toDo.Count;
            List<Task> tasks = new List<Task>();
            var startTime = DateTime.Now;
            int progressLock = 0;

            for (int i = 0; i < Settings.Default.ThreadCount; i++)
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
                            Interlocked.Increment(ref count);
                            await TryWrite(url);
                            continue;
                        }

                        try
                        {
                            Interlocked.Increment(ref count);
                            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);
                            await _takealotMapper.MapAndSaveFullAsync(jsonObject);
                        }
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            toDo.Enqueue(line);
                            Interlocked.Decrement(ref count);
                        }
                        catch(JsonReaderException e)
                        {
                            await TryWrite(url + "," + e.Message);
                            continue;
                        }

                        await TryWrite(url);
                        if (0 == Interlocked.Exchange(ref progressLock, 1))
                        {
                            RaiseOnProgress(count, total, startTime);
                            Interlocked.Exchange(ref progressLock, 0);
                        }

                        if (token.IsCancellationRequested)
                            break;
                    }
                    count++;
                })
                .ContinueWith(ctask =>
                {
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine($"Runnuing tasks remaining:{tasks.Select(x => !x.IsCompleted).Count()}{Environment.NewLine}");
                    Console.WriteLine($"A task ended unexpectedly:{ctask.Exception.ToString()}");
                }, TaskContinuationOptions.OnlyOnFaulted));
            }
            try
            {
                await Task.WhenAll(tasks);
            }
            finally
            {
                _doneTxt.Close();
                _doneTxt.Dispose();
            }
        }

        public async Task TryWrite(string text, int count = 0)
        {
            if (count > 0)
                await Task.Delay(100 * count);

            if (count == 4)
                throw new Exception("Max write attempt reached for done.txt");

            try
            {
                await _doneTxt.WriteLineAsync(text);
            }
            catch (InvalidOperationException)
            {
                await TryWrite(text, ++count);
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
