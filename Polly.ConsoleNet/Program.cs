﻿using SimpleInjector;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    class Program
    {
        private static CancellationTokenSource source = new CancellationTokenSource();

        static Program()
        {
            _container = new Container();
            Domain.RegisterDI.Register(_container);
            Data.RegisterDI.Register(_container);
            _container.Register<Data.IDownloadQueueRepository, DownloadQueueFileRepository>();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            source.Cancel();
            e.Cancel = true;
        }

        private static Container _container;
        private static Dictionary<int, IAsyncWorker> MenuItems => new Dictionary<int, IAsyncWorker>()
        {
            { 1, _container.GetInstance<QueueTakealotLinks>() },
            { 2, _container.GetInstance<QueueLootLinks>() },
            //{ 2, _container.GetInstance<DownloadFromQueue>() },
            { 3, _container.GetInstance<Upload>() },
            { 4, _container.GetInstance<Compress>() },
            { 5, _container.GetInstance<Refresh>() },
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
            Console.WriteLine("1.Populate download queue from Takealot robots.");
            Console.WriteLine("2.Populate download queue from Loot robots.");
            Console.WriteLine("3.Update products from sitemap links");
            Console.WriteLine("4.Compress");
            Console.WriteLine("5.Refresh old products (2 weeks)");
            //Console.WriteLine("3.Populate download queue with products older than 2 days.");
            //Console.WriteLine("4.Run Downloader (PriceOnly).");
            //Console.WriteLine("5.Run Downloader (PriceAndProduct).");
            //Console.WriteLine("6.Check for new price where older than 2 days.");
            //Console.WriteLine("7.Get daily deal prices.");

            if (int.TryParse(Console.ReadLine(), out int menuOption)
                && menuOption > 0
                && menuOption < 6)
                return menuOption;

            Console.Clear();
            return GetMenuOption();
        }
    }
}
