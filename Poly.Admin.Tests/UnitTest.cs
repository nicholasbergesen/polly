using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Poly.Admin.Tests
{
    /// <summary>
    /// Summary description for UnitTest
    /// </summary>
    [TestClass]
    public class UnitTest
    {
        public UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestTakealotProduct()
        {
            Assert.IsTrue(IsTakealotProduct(@"https://www.takealot.com/shampooheads-professional-strawberry-kiss-detangler-spray-200ml/PLID42966109"));
        }

        [TestMethod]
        public void TestLootProduct()
        {
            Assert.IsTrue(IsLootProduct(@"http://www.loot.co.za/product/hisense-hx49m2160nf-49-fhd-led-tv/ywhk-4448-g6a0"));
        }

        private bool IsTakealotProduct(string url)
        {
            var urlSections = url.Split('/');
            return urlSections.Length == 5;
        }

        private bool IsLootProduct(string url)
        {
            var sections = url.Split('/');
            return !url.Contains("?") && sections.Length == 6 && sections[3] == "product";
        }
    }
}
