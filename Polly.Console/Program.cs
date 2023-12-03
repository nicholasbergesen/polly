using Polly.Domain;
using Polly.Data;
using SimpleInjector;

namespace Polly.Console
{
    public class Program
    {
        private readonly static CancellationTokenSource source = new CancellationTokenSource();

        static Program()
        {
            _container = new Container();
            Domain.RegisterDI.Register(_container);
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
            { 1, new BuildFileSource() },
            { 2,
                new QueueLinks(new List<ILinkSource>()
                {
                    //new LootRobots(),
                    //new TakealotRobots(),
                    //new RefreshDatabase(),
                    new TakealotUrlList()
                },
                new Data.DownloadQueueFileRepository())
            },
            { 3,
                new QueueLinks(new List<ILinkSource>()
                {
                    //new RefreshDatabase()
                },
                new Data.DownloadQueueFileRepository())
            },
            //{ 3, _container.GetInstance<Upload>() },
            { 4, _container.GetInstance<CompressProductDescriptions>() },
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
            System.Console.Clear();
            var instance = MenuItems[GetMenuOption()];
            await instance.DoWorkAsync(source.Token);
            System.Console.WriteLine("Done.");
            System.Console.ReadLine();
        }

        public static int GetMenuOption()
        {
            System.Console.WriteLine("1.Create file source from sitemap.");
            System.Console.WriteLine("2.Update products. (file source)");
            System.Console.WriteLine("3.Update products. (database source)");
            System.Console.WriteLine("4.Clean product descriptions.");
            System.Console.WriteLine("5.Test.");

            if (int.TryParse(System.Console.ReadLine(), out int menuOption)
                && menuOption > 0
                && menuOption < 6)
                return menuOption;

            System.Console.Clear();
            return GetMenuOption();
        }
    }
}
