using System;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BubelSoft.SeleniumTests.Tests.LoginPage
{
    [TestFixture]
    public class LoginPageTests
    {
        [Test]
        public void SuccessLogIn()
        {
            IWebDriver driver = new ChromeDriver(@"D:\Projekty\Bubelsoft\Tests\BubelSoft.SeleniumTests\BubelSoft.SeleniumTests\bin\Debug\netcoreapp2.0\");
            driver.Navigate().GoToUrl("http://localhost:10448/#user/logIn");

            driver.FindElement(By.CssSelector("input[type=text]")).SendKeys("macbub");
            driver.FindElement(By.CssSelector("input[type=password]")).SendKeys("qwe");
            driver.FindElement(By.CssSelector("button")).Click();

            while (driver.Url == "http://localhost:10448/#user/logIn")
            {
                Thread.Sleep(1000);
            }

            var nav = driver.FindElements(By.CssSelector("nav"));

            Assert.That(nav.Count, Is.EqualTo(1));
        }

        [Test]
        public void UnsuccessLogIn()
        {
            IWebDriver driver = new ChromeDriver(@"D:\Projekty\Bubelsoft\Tests\BubelSoft.SeleniumTests\BubelSoft.SeleniumTests\bin\Debug\netcoreapp2.0\");
            driver.Navigate().GoToUrl("http://localhost:10448/#user/logIn");

            driver.FindElement(By.CssSelector("input[type=text]")).SendKeys("macbub");
            driver.FindElement(By.CssSelector("input[type=password]")).SendKeys("qwe");
            driver.FindElement(By.CssSelector("button")).Click();

            while (driver.Url == "http://localhost:10448/#user/logIn")
            {
                Thread.Sleep(1000);
            }

            var nav = driver.FindElements(By.CssSelector("nav"));

            Assert.That(nav.Count, Is.EqualTo(1));
        }


    }
}