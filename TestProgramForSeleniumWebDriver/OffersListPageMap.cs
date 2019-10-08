using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgramForSeleniumWebDriver
{
    partial class OffersListPage
    {
        public IWebElement ResultsLabel { get { return driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]")); } }

        public IWebElement AnnouncementsGrid { get { return this.driver.FindElement(By.XPath($"(//div[@id='search_results_div']//table)[5]")); } }

        public ReadOnlyCollection<IWebElement> Rows { get { return this.AnnouncementsGrid.FindElements(By.XPath("tbody/tr")); } }

        public ReadOnlyCollection<IWebElement> JobsLinks { get { return driver.FindElements(By.ClassName("joblink")); } }

        public ReadOnlyCollection<IWebElement> CompaniesLinks { get { return driver.FindElements(By.ClassName("company_link")); } }

        public ReadOnlyCollection<IWebElement> OffersDates { get { return driver.FindElements(By.ClassName("explainGray")); } }


        public bool HasAd => this.Rows.Count() == 16;
        
        

    }
}
