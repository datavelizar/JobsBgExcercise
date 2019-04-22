using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace PomOfTestProgramForSeleniumWebDriver.Core
{
    public static class Driver
    {
        public static IWebDriver Initialize()
        {
            var driver = new ChromeDriver(Environment.CurrentDirectory);//new FirefoxDriver(); // new PhantomJSDriver();//
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

            return driver;
        }

       
    }
}

//IWebDriver driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));//working
//IWebDriver driver = new ChromeDriver(Environment.CurrentDirectory);//working without reflection libraries
//IWebDriver driver = new ChromeDriver(); //new FirefoxDriver(); // new PhantomJSDriver();//
//driver.Manage().Window.Maximize();
//driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

////Whole Category "IT - Sowtware"; Keyword is String.Empty, Category is 15
//driver.Url = "https://www.jobs.bg/front_job_search.php?frompage=";
