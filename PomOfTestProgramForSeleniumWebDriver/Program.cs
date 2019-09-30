using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using PomOfTestProgramForSeleniumWebDriver.Core;
using System;
using System.IO;
using System.Reflection;

namespace PomOfTestProgramForSeleniumWebDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var driver = Driver.Initialize();
            driver.Url = "https://www.jobs.bg/front_job_search.php?frompage=";
            
            //aaa.browser.Url = "https://www.jobs.bg/front_job_search.php?frompage="; //ComposeURLInJobsbg("", 15, 0);

            Console.WriteLine("Hello World!");
        }
    }
}
