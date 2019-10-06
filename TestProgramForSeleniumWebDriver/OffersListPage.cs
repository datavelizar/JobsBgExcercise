using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TestProgramForSeleniumWebDriver
{
    class OffersListPage
    {
        private readonly IWebDriver driver;

        public OffersListPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public IWebElement AnnouncementsGrid
        {
            get { return this.driver.FindElement(By.XPath($"(//div[@id='search_results_div']//table)[5]")); }
        }

        public IReadOnlyCollection<IWebElement> Rows
        {
            // get {return this.Table.FindElements(By.XPath("//tr//td[@class='offerslistRow']")); }  }
            get { return this.AnnouncementsGrid.FindElements(By.XPath("tbody/tr")); }
        }

        public bool ValidateRow(int row)
        {
            bool hasAd = this.Rows.Count() == 16;

            if (hasAd && row == 8)
            {
                return false;
            }

            return row > 0 && row <= this.Rows.Count();
        }

        public IWebElement FirstCell(int row)
        {
            if (this.ValidateRow(row))
            {
                return this.AnnouncementsGrid.FindElement(By.XPath($"//tr[{row}]//td[@class='offerslistRow']"));
            }
            else if (row == 8)
            {
                return this.AnnouncementsGrid.FindElement(By.XPath($"//tr[{row}]//td[@class='offerslistRow']"));
            }
            else
            {
                throw new ArgumentException("There is no such row on page!");
            }
        }

        public IWebElement JobOfferLink(int row)
        {
            if (this.ValidateRow(row))
            {
                return this.AnnouncementsGrid.FindElement(By.XPath($"//tr[{row}]//td[@class='offerslistRow']/a[@class='joblink']"));
            }
            else if (row == 8)
            {
                return this.AnnouncementsGrid.FindElement(By.XPath($"//tr[{row}]//td[@class='offerslistRow']"));
            }
            else
            {
                throw new ArgumentException("There is no such row on page!");
            }
        }

        public IWebElement CompanyLink(int row)
        {
            if (this.ValidateRow(row))
            {
                return this.AnnouncementsGrid.FindElement(By.XPath($"//tr[{row}]//td[@class='offerslistRow']/a[@class='company_link']"));
            }
            else if (row == 8)
            {
                return this.AnnouncementsGrid.FindElement(By.XPath($"//tr[{row}]//td[@class='offerslistRow']"));
            }
            else
            {
                throw new ArgumentException("There is no such row on page!");
            }
        }

        //var resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
        public IWebElement ResultsLabel { get { return driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]")); } }

        public int TotalNumberOfAnnouncements()
        {
            var splittedResultsLabel = this.ResultsLabel.Text.Split(' ');

            if (splittedResultsLabel.Length > 0)
            {
                return int.Parse(splittedResultsLabel[splittedResultsLabel.Length - 1]);
            }
            else
            {
                return 0;
            }
        }

        public int NumberOfResultPages(int totalAnnouncements, int resultsOnPage = 15)
        {
            int numberOfResultPages = totalAnnouncements / resultsOnPage;
            if (totalAnnouncements % resultsOnPage == 0)
            {
                numberOfResultPages -= 1;
            }

            return numberOfResultPages + 1;
        }

    }
}
