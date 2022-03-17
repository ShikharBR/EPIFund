using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using Selenium;

namespace Selenium.Test
{
    [TestFixture]
    public abstract class MainSeleniumTest
    {
        #region global
        protected ISelenium selenium;

        [SetUp]
        public void RunBeforeAnyTests()
        {
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
        }

        [TestFixtureTearDown]
        public void TeardownTest()
        {
            try
            {
                selenium.Stop();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        #endregion

        [Test]
        public void ReliantTest_01() //Basic links
        {
            selenium.SetSpeed("100");
			selenium.Open("/Default.aspx");
			selenium.Click("link=Services");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("link=Contact Us");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("link=Team");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("link=Our Work");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("link=Home");
			selenium.WaitForPageToLoad("30000");
        }

        [Test]
        public void ReliantTest_02() //More specific location
        {
			selenium.Open("/Default.aspx");
            Assert.IsTrue(selenium.IsTextPresent("Reliant Programming"));
			selenium.Click("xpath=(//a[contains(text(),'Services')])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(//a[contains(text(),'Contact Us')])[2]");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(//a[contains(text(),'Team')])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(//a[contains(text(),'Our Work')])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(//a[contains(text(),'Home')])[2]");
			selenium.WaitForPageToLoad("30000");
        }
		
		[Test]
        public void ReliantTest_03() //A different way to specify the location on the page
        {
            selenium.Open("http://www.reliantwebservices.com/Default.aspx"); //You can set your localhost
			selenium.Click("xpath=(.//*[@id='ctl00_mainMenu_LinkList']/a[2])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(.//*[@id='ctl00_mainMenu_LinkList']/a[3])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(.//*[@id='ctl00_mainMenu_LinkList']/a[4])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(.//*[@id='ctl00_mainMenu_LinkList']/a[5])");
			selenium.WaitForPageToLoad("30000");
			selenium.Click("xpath=(.//*[@id='ctl00_mainMenu_LinkList']/a[1])");
			selenium.WaitForPageToLoad("30000");
        }
    }
}
