﻿using Polly.Data;
using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Polly.ConsoleNet
{
    public class Compress : SimpleWorker
    {
        public Compress()
            : base()
        {
            ServicePointManager.DefaultConnectionLimit = 150;
        }

        protected override async Task DoWorkInternalAsync(CancellationToken token)
        {
            var startTime = DateTime.Now;
            int count = 0;
            long lastId = await DataAccess.LastId();
            int productCount = await DataAccess.ProductCount();

            long lastProcessedId = 3852;
            if (File.Exists("processed.txt"))
            {
                lastProcessedId = long.Parse(File.ReadAllText("processed.txt"));
                productCount -= int.Parse(lastProcessedId.ToString());
            }

            while (lastProcessedId <= lastId)
            {
                var nextProducts = await DataAccess.GetNextProduct(lastProcessedId);

                foreach (var nextProduct in nextProducts)
                {
                    lastProcessedId = nextProduct.Id;

                    if (nextProduct == default || nextProduct.Description == default)
                    {
                        File.WriteAllText("processed.txt", (lastProcessedId).ToString());
                        RaiseOnProgress(++count, productCount, startTime);
                        continue;
                    }

                    int imgIndex = nextProduct.Description.IndexOf("<img");
                    string secondHalf = null;
                    int imgEndIndex = -1;
                    if (imgIndex > -1)
                    {
                        secondHalf = nextProduct.Description.Substring(imgIndex, nextProduct.Description.Length - imgIndex);
                        imgEndIndex = secondHalf.IndexOf(">") + imgIndex;
                    }

                    if (imgIndex - imgEndIndex == 1)
                        throw new Exception("not good");

                    bool needssave = false;
                    while (imgIndex > -1 && imgEndIndex > -1)
                    {
                        needssave = true;
                        nextProduct.Description = nextProduct.Description.Remove(imgIndex, imgEndIndex - imgIndex);

                        imgIndex = nextProduct.Description.IndexOf("<img");
                        if (imgIndex > -1)
                        {
                            secondHalf = nextProduct.Description.Substring(imgIndex, nextProduct.Description.Length - imgIndex - 1);
                            imgEndIndex = secondHalf.IndexOf(">") + imgIndex;
                        }
                    }

                    int curLength = nextProduct.Description.Length;
                    nextProduct.Description = nextProduct.Description.Replace("    ", " ");
                    int reducedLength = nextProduct.Description.Length;
                    while (curLength != reducedLength)
                    {
                        needssave = true;
                        curLength = nextProduct.Description.Length;
                        nextProduct.Description = nextProduct.Description.Replace("    ", " ");
                        reducedLength = nextProduct.Description.Length;
                    }

                    if (nextProduct.Description.Length > 8000)
                    {
                        needssave = true;
                        nextProduct.Description = nextProduct.Description.Substring(0, 8000);
                    }

                    if (needssave)
                        await DataAccess.UpdateDescription(nextProduct);

                    File.WriteAllText("processed.txt", (lastProcessedId).ToString());
                    RaiseOnProgress(++count, productCount, startTime);
                }
            }
        }

        public override string ToString()
        {
            return "Compress";
        }
    }
}
