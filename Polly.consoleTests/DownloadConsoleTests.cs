﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Polly.DownloadConsole
{
    [TestClass]
    public class DownloadConsoleTests
    {
        [TestMethod]
        public void TestProgress()
        {
            string test = Program.RaiseOnProgress(10, 1000, DateTime.Now);
        }
    }
}