using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TestProgramForSeleniumWebDriver
{
    partial class OffersListPage
    {
        private readonly IWebDriver driver;

        public OffersListPage(IWebDriver driver)
        {
            this.driver = driver;
        }

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

        public IList<string> GetJobsTitlesTexts() //ICollection<IWebElement> jobsLinks)
        {
            var jobsTitlesTexts = new List<string>();

            foreach (var item in JobsLinks)// jobsLinks)
            {
                jobsTitlesTexts.Add(item.GetAttribute("href"));
            }

            return jobsTitlesTexts;
        }

        public IList<string> GetJobsLinksTexts()//ICollection<IWebElement> jobsLinks)
        {
            var jobsLinksTexts = new List<string>();

            foreach (var item in JobsLinks)
            {
                jobsLinksTexts.Add(item.Text);
            }

            return jobsLinksTexts;
        }

        public IList<string> GetCompaniesLinksTexts()//(ReadOnlyCollection<IWebElement> companiesLinks)
        {
            var companiesLinksTexts = new List<string>();

            foreach (var item in CompaniesLinks)
            {
                companiesLinksTexts.Add(item.Text);
            }

            return companiesLinksTexts;
        }

        public IList<string> GetOffersDatesTexts()//(ReadOnlyCollection<IWebElement> offersDates)
        {
            var offersDatesTexts = new List<string>();

            foreach (var item in OffersDates)
            {
                offersDatesTexts.Add(item.Text);
            }

            return offersDatesTexts;
        }
    }
}
