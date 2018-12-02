using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Polly.ConsoleCore
{
    public class Program
    {
        static Program()
        {
            _container = new Container();
            Domain.RegisterDI.Register(_container);
            Data.RegisterDI.Register(_container);
        }

        private static Container _container;
        private static Dictionary<int, IAsyncWorker> MenuItems => new Dictionary<int, IAsyncWorker>()
        {
            { 1, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() },
            { 2, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() },
            { 3, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() },
            { 4, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() },
            { 5, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() },
            { 6, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() },
            { 7, _container.GetInstance<PopulateAndDownloadQueueFromRobots>() }
        };

        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            await MenuItems[GetMenuOption()].DoWorkAsync();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        public static int GetMenuOption()
        {
            Console.WriteLine("1.Populate download queue from robots.");
            Console.WriteLine("2.Populate download queue with new products from robots.");
            Console.WriteLine("3.Populate download queue with products older than 2 days.");
            Console.WriteLine("4.Run Downloader (PriceOnly).");
            Console.WriteLine("5.Run Downloader (PriceAndProduct).");
            Console.WriteLine("6.Check for new price where older than 2 days.");
            Console.WriteLine("7.Get daily deal prices.");

            if (int.TryParse(Console.ReadLine(), out int menuOption)
                && menuOption > 0
                && menuOption < 7)
                return menuOption;

            Console.Clear();
            return GetMenuOption();
        }
    }
}
