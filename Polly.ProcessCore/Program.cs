using RobotsCoreParser;
using System;
using System.Threading.Tasks;

namespace Polly.ProcessCore
{
    public class Program
    {
        static void Main(string[] args)
        {
            var type = args[0];
            switch(type)
            {
                case "1":
                    new WebsiteRobots().DownloadWebsiteRobotsLinks(website: args[1], saveToFile: args[2]).Wait();
                    break;
                //case "2":
                //    new WebsiteRobots().DownloadContentToDatabase(website: args[1], saveToFile: args[2]).Wait();
                //    break;
                default:
                    throw new Exception($"unknown type {type}");
            }

            Console.ReadLine();
        }
    }
}
