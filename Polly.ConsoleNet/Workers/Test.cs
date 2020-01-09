using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class Test : SimpleWorker
    {
        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            long count = 1; //for first line
            using(StreamReader sr = new StreamReader("downloadLinks.txt"))
            {
                string firstLine = await sr.ReadLineAsync();
                while(!sr.EndOfStream)
                {
                    var line = await sr.ReadLineAsync();
                    count++;
                    using (HttpClient client = new HttpClient())
                    {
                        var res = await client.GetAsync(line.Split(',')[1]);
                        if(res.IsSuccessStatusCode)
                        {
                            Console.CursorTop = 0;
                            Console.CursorLeft = 0;
                            Console.WriteLine($"{count},{line}");
                            Console.ReadLine();
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}
