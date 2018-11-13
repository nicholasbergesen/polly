using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.SchedulerConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var website = DataAccess.GetWebsiteById(1);
            Scheduler scheduler;
            if (website.Name == "Takealot")
                scheduler = new TakealotScheduler(website);
            else
                throw new Exception("No website found");

            scheduler.OnProgress += Scheduler_OnProgress;
            scheduler.OnStart += Scheduler_OnStart;
            scheduler.OnEnd += Scheduler_OnEnd;
            scheduler.Start();
            Console.ReadLine();
        }

        private static void Scheduler_OnEnd(object sender, EventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now}] Stopped");
            Console.WriteLine($"Press any key to close console.");
        }

        private static void Scheduler_OnStart(object sender, EventArgs e)
        {
            Console.WriteLine($"[{DateTime.Now}] Started");
        }

        private static void Scheduler_OnProgress(object sender, ProgressEventArgs e)
        {
            Console.WriteLine(e.ProgressString);
            Console.CursorLeft = 0;
            Console.CursorTop = Math.Max(Console.CursorTop - 1, 0);
        }
    }
}
