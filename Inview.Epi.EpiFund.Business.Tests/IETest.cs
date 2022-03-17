using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace Selenium.Test
{
    [TestFixture]
    public abstract class IETest : MainSeleniumTest
    {
        [TestFixtureSetUp]
        public void SetupTests()
        {
            selenium = new DefaultSelenium("localhost", 4444, "*iehta", "http://www.reliantwebservices.com/");
            selenium.Start();
        }
    }
}
