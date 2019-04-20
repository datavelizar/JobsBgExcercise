using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomOfTestProgramForSeleniumWebDriver.Core
{
   public class Driver
    {

        IWebDriver driver = new ChromeDriver(); //new FirefoxDriver(); // new PhantomJSDriver();//


        //driver.Manage().Window.Maximize();
        //                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

    }
}
