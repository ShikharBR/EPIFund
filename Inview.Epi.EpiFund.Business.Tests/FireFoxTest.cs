using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace Selenium.Test
{
    [TestFixture]
    public abstract class FireFoxTest : MainSeleniumTest
    {
        [TestFixtureSetUp]
        public void SetupTests()
        {
            selenium = new DefaultSelenium("localhost", 4444, "*firefox", "http://www.reliantwebservices.com/"); //You can set your localhost here
            selenium.Start();
        }
    }
}