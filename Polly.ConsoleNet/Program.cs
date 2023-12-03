using Polly.Data;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    class Program
    {
        private readonly static CancellationTokenSource source = new CancellationTokenSource();

        static Program()
        {
            _container = new Container();
            RegisterDI.Register(_container);
            Data.RegisterDI.Register(_container);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            source.Cancel();
        }

        private static readonly Container _container;
        private static Dictionary<int, IAsyncWorker> MenuItems => new Dictionary<int, IAsyncWorker>()
        {
            { 1,
                new QueueLinks(new List<ILinkSource>()
                {
                    //new LootRobots(),
                    //new TakealotRobots(),
                    //new RefreshDatabase(),
                    new TakealotUrlList()
                },
                new Data.DownloadQueueFileRepository());
            },
            { 2, 
                new QueueLinks(new List<ILinkSource>()
                {
                    new RefreshDatabase()
                },
                new Data.DownloadQueueFileRepository())
            },
            { 3, _container.GetInstance<Upload>() },
            { 4, _container.GetInstance<Compress>() },
            { 5, new Test() }
            //{ 1, _container.GetInstance<QueueTakealotLinks>() },
            //{ 2, _container.GetInstance<QueueLootLinks>() },
            //{ 2, _container.GetInstance<DownloadFromQueue>() },
            //{ 4, _container.GetInstance<Refresh>() },
        };

        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.Clear();
            var instance = MenuItems[GetMenuOption()];
            await instance.DoWorkAsync(source.Token);
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        public static int GetMenuOption()
        {
            Console.WriteLine("1.Queue links.");
            Console.WriteLine("2.Queue links (database only).");
            Console.WriteLine("3.Upload from queue.");
            Console.WriteLine("4.Clean product descriptions.");
            Console.WriteLine("5.Test.");

            if (int.TryParse(Console.ReadLine(), out int menuOption)
                && menuOption > 0
                && menuOption < 6)
                return menuOption;

            Console.Clear();
            return GetMenuOption();
        }
    }
}
