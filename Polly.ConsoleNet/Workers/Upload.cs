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
    public class Upload : SimpleWorker, IDisposable
    {
        readonly IDownloader _downloader;
        readonly ITakealotMapper _takealotMapper;
        private StreamWriter _doneTxt;
        private readonly BlockingCollection<string> _doneQueue;

        public Upload(IDownloader downloader, ITakealotMapper takealotMapper)
        {
            ServicePointManager.DefaultConnectionLimit = 150;
            _downloader = downloader;
            _takealotMapper = takealotMapper;
            _doneQueue = new BlockingCollection<string>(new ConcurrentQueue<string>());
        }

        private Task _writeToDoTask;
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
            _writeToDoTask = Task.Run(WriteToDo);

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
                            _doneQueue.Add(url);
                            continue;
                        }

                        try
                        {
                            Interlocked.Increment(ref count);
                            TakealotJson jsonObject = JsonConvert.DeserializeObject<TakealotJson>(httpResponse);
                            await _takealotMapper.MapAndSaveJsonAsync(jsonObject);
                        }
                        catch (System.Data.Entity.Core.EntityException)
                        {
                            toDo.Enqueue(line);
                            Interlocked.Decrement(ref count);
                        }
                        catch(JsonReaderException e)
                        {
                            _doneQueue.Add(url + "," + e.Message);
                            continue;
                        }

                        _doneQueue.Add(url);
                        if (0 == Interlocked.Exchange(ref progressLock, 1))
                        {
                            RaiseOnProgress(count, total, startTime);
                            Interlocked.Exchange(ref progressLock, 0);
                        }

                        if (token.IsCancellationRequested)
                            break;
                    }
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
                _doneQueue.CompleteAdding();
                await _writeToDoTask;
            }
            finally
            {
                _doneTxt.Close();
                _doneTxt.Dispose();
            }
        }

        public async Task WriteToDo()
        {
            while(!_doneQueue.IsCompleted)
            {
                var text = _doneQueue.Take();
                await _doneTxt.WriteLineAsync(text);
                await _doneTxt.FlushAsync();
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

        public void Dispose()
        {
            _doneTxt.Close();
            _doneTxt.Dispose();
            _downloader.Dispose();
        }
    }
}
