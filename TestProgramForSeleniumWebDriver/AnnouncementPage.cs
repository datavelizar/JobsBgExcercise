using OpenQA.Selenium;
using System;

namespace TestProgramForSeleniumWebDriver
{
    class AnnouncementPage
    {
        private readonly IWebDriver driver;

        public AnnouncementPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement CountBox { get { return driver.FindElement(By.Id("cnt_box")); } }

        public string OfferFullText { get { return driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td/table/tbody")).Text; } }

        public string GetOfferLooks()
        {
            var offerLooks = this.CountBox.Text.Split(':');
            return offerLooks[offerLooks.Length - 1];
        }

    }
}
