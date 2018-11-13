using Polly.Data;
using System;
using System.Collections.Concurrent;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentQueue<long> queue;
            using (PollyDbContext db = new PollyDbContext())
            {
                var productIds = db.Product
                    .Where(x => x.PriceHistory
                    .Any(y => !y.DiscountAmount.HasValue && !y.PreviousPriceHistoryId.HasValue))
                    .OrderBy(x => x.Id)
                    .Select(x => x.Id);
                queue = new ConcurrentQueue<long>(productIds);
            }
            int prodCount = queue.Count;
            DateTime startTime = DateTime.Now;
            int taskCount = 10;
            int counter = 0;
            Task[] tasks = new Task[taskCount];

            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(async () =>
                {
                    while (queue.TryDequeue(out long productId))
                    {
                        using (PollyDbContext db = new PollyDbContext())
                        {
                            var priceHistories = await db.PriceHistory.Where(x => x.ProductId == productId).OrderBy(x => x.TimeStamp).ToListAsync();
                            PriceHistory previousPriceHistory = null;
                            foreach (var priceHistory in priceHistories)
                            {
                                if (priceHistory.OriginalPrice.HasValue)
                                {
                                    priceHistory.DiscountAmount = priceHistory.OriginalPrice - priceHistory.Price;
                                    priceHistory.DiscountPercentage = priceHistory.DiscountAmount / priceHistory.OriginalPrice * 100;
                                }

                                if (previousPriceHistory == null)
                                {
                                    previousPriceHistory = priceHistory;
                                    continue;
                                }

                                priceHistory.PreviousPriceHistoryId = previousPriceHistory.Id;
                                priceHistory.PriceChangeAmount = priceHistory.Price - previousPriceHistory.Price;
                                priceHistory.PriceChangePercent = priceHistory.PriceChangeAmount / previousPriceHistory.Price * 100;
                                db.Entry(priceHistory).State = EntityState.Modified;

                                previousPriceHistory = priceHistory;
                            }
                            await db.SaveChangesAsync();
                        }
                        WriteOutput(RaiseOnProgress(++counter, prodCount, startTime));
                    }
                });
            }

            var waitingTask = Task.WhenAll(tasks);
            //while (!waitingTask.IsCompleted)
            //{
            //}
            waitingTask.Wait();
        }

        public static string RaiseOnProgress(int requestCount, int totalSize, DateTime startTime)
        {
            double downloadRate = Math.Max(requestCount / Math.Max(DateTime.Now.Subtract(startTime).TotalSeconds, 1), 1);
            int itemsRemaining = totalSize - requestCount;
            return $"{requestCount} of {totalSize} { (requestCount * 1.00 / totalSize * 1.00 * 100):0.####}% { downloadRate:0.##}/s ETA:{ DateTime.Now.AddSeconds(itemsRemaining / downloadRate) }        ";
        }

        private static void WriteOutput(string output)
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            Console.WriteLine(output);
        }
    }
}
