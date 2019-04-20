using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProgramForSeleniumWebDriver.Pages
{
    public partial class MainPage
    {
        public IWebElement ResultsLabel { get { return driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]")); } }

        public IList<IWebElement> Rows { get { return driver.FindElements(By.ClassName("offerslistRow")); }}

        public IList<IWebElement> JobLinks { get { return driver.FindElements(By.ClassName("joblink")); } }

        public IList<IWebElement> CompanyLinks { get { return driver.FindElements(By.ClassName("company_link")); } }

        public IList<IWebElement> OffersDates { get { return driver.FindElements(By.ClassName("explainGray")); } }
    }
}
//// examples
//public IWebElement EmailInput { get { return this.Driver.FindElement(By.CssSelector("#email")); } }

//var currentPageRowsCollection = driver.FindElements(By.ClassName("offerslistRow"));
//var currentPageJobLinksCollection = driver.FindElements(By.ClassName("joblink"));
//var currentPageCompanyLinksCollection = driver.FindElements(By.ClassName("company_link"));
//var currentPageOffersDatesCollection = driver.FindElements(By.ClassName("explainGray"));

//var resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));