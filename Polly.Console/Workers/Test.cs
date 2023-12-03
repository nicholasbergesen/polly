using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.Console
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
                            System.Console.CursorTop = 0;
                            System.Console.CursorLeft = 0;
                            System.Console.WriteLine($"{count},{line}");
                            System.Console.ReadLine();
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
