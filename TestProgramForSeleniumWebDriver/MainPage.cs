using OpenQA.Selenium;

namespace TestProgramForSeleniumWebDriver
{
    class MainPage
    {
        private readonly IWebDriver driver;

        public MainPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement ResultsLabel => driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
    }
}
