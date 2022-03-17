using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace Selenium.Test
{
    [TestFixture]
    public class ChromeTest : MainSeleniumTest
    {
        [TestFixtureSetUp]
        public void SetupTests()
        {
            //selenium = new DefaultSelenium("localhost", 4444, "*googlechrome C:\\Users\\ssquarepants\\AppData\\Local\\Google\\Chrome\\Application\\chrome.exe", "http://http://www.reliantwebservices.com/");
            selenium = new DefaultSelenium("localhost", 4444, "*googlechrome", "http://www.reliantwebservices.com/");
            selenium.Start();
        }
    }
}
