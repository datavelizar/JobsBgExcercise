namespace TestProgramForSeleniumWebDriver
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using OfficeOpenXml;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using Utils;

    public class Program
    {
        public static void Main()
        {
            string keyWord = ".NET Test Automation";//"Automation QA .NET";//"Test Automation";//"Czech";//"QA";//"Чешки";//"";//".NET Test Automation";//
            int chosenCategory = 15;// 0=allCategories; 15=SW
            string textFileName1 = Utils.CreateNameFromDateTimeNow(keyWord + "_results.txt");
            string textFileName2 = Utils.CreateNameFromDateTimeNow(keyWord + "_results2.txt");
            string excelFileName = Utils.CreateNameFromDateTimeNow(keyWord + "_results3.xlsx"); //For writing in different sheets of the same excel file should pass only "results3.xlsx";
            string excelSheetName = Utils.CreateNameFromDateTimeNow(keyWord);

            StreamWriter streamWriter = new StreamWriter(@"..\..\Results\" + textFileName1);
            StreamWriter streamWriter2 = new StreamWriter(@"..\..\Results\" + textFileName2);
            ExcelPackage excelPackage = new ExcelPackage(new FileInfo(@"..\..\Results\" + excelFileName));

            using (excelPackage)
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(excelSheetName);

                using (streamWriter)
                {
                    using (streamWriter2)
                    {
                        IWebDriver driver = StartBrowser();
                        ////Whole Category "IT - Sowtware"; Keyword is String.Empty, Category is 15
                        driver.Url = ComposeURLInJobsbg("", chosenCategory, 0);
                        OffersListPage offersListPage = new OffersListPage(driver);

                        var totalAnnouncements = offersListPage.TotalNumberOfAnnouncements();

                        Console.WriteLine("Total: {0}; At: {1}", totalAnnouncements, DateTime.Now.ToString());
                        streamWriter.WriteLine("Total: {0}; At: {1}", totalAnnouncements, DateTime.Now.ToString());
                        streamWriter2.WriteLine("Total: {0}; At: {1}", totalAnnouncements, DateTime.Now.ToString());

                        int startingPage = 0;
                        int number = 1;
                        int count = 0;
                        int resultsOnPage = 15;
                        int rowNumber = 2;

                        int numberOfResultPages = offersListPage.NumberOfResultPages(totalAnnouncements, resultsOnPage);

                        for (int i = 0; i < numberOfResultPages; i++)
                        {
                            //Use the already declared keyword and category (SW sector is category=15) 
                            string resultURL = ComposeURLInJobsbg(keyWord, chosenCategory, startingPage);

                            driver.Url = (resultURL);
                            offersListPage = new OffersListPage(driver);

                            var rows = offersListPage.Rows;

                            var jobsLinks = offersListPage.JobsLinks;
                            var jobsLinksTexts = offersListPage.GetJobsLinksTexts();
                            var jobTitlesTexts = offersListPage.GetJobsTitlesTexts();

                            var companiesLinks = offersListPage.CompaniesLinks;
                            var companyLinksTexts = offersListPage.GetCompaniesLinksTexts();

                            var offersDates = offersListPage.OffersDates;
                            var offersDatesTexts = offersListPage.GetOffersDatesTexts();

                            var rowCollection = new List<string>();

                            // TODO refactor to method that receives collection as parameter
                            foreach (var row in rows)
                            {
                                rowCollection.Add(row.Text);
                            }

                            ////Collection of the number of looks on each annoucement
                            var offers = new List<Announcement>();

                            for (int m = 0; m < jobTitlesTexts.Count; m++)
                            {
                                // TODO implement automatic page building when driver navigates
                                driver.Url = (resultURL);
                                offersListPage = new OffersListPage(driver);

                                var announcement = new Announcement
                                {
                                    CompanyName = jobTitlesTexts[m],
                                    CompanyOffer = companyLinksTexts[m],
                                    OfferLink = jobsLinksTexts[m],
                                    Date = offersDatesTexts[m]
                                };

                                //Showing the offer contents
                                //jobsLinks = driver.FindElements(By.ClassName("joblink"));
                                jobsLinks = offersListPage.JobsLinks; // TODO check if needed for Stale Element Reference Error?
                                jobsLinks[m].Click();

                                var announcementPage = new AnnouncementPage(driver);

                                announcement.OfferLooks = announcementPage.GetOfferLooks();
                                announcement.FullOfferText = announcementPage.FullOfferText;

                                offers.Add(announcement);

                                driver.Url = (resultURL);

                                ////TODO better typing
                                streamWriter2.WriteLine(String.Format("{0} | {1} | {2} | {3} | {4}",
                                    announcement.Date,
                                    announcement.CompanyOffer,
                                    announcement.CompanyName,
                                    announcement.OfferLooks,
                                    announcement.OfferLink
                                    ));

                                ////Filling Excel cells
                                excelWorksheet.Cells["A" + rowNumber].Value = announcement.Date;
                                excelWorksheet.Cells["B" + rowNumber].Value = announcement.CompanyOffer;
                                excelWorksheet.Cells["C" + rowNumber].Value = announcement.CompanyName;
                                excelWorksheet.Cells["D" + rowNumber].Value = announcement.OfferLooks;
                                excelWorksheet.Cells["E" + rowNumber].Value = announcement.OfferLink;
                                excelWorksheet.Cells["F" + rowNumber].Value = announcement.FullOfferText;

                                rowNumber++;
                            }

                            totalAnnouncements = offersListPage.TotalNumberOfAnnouncements();
                            numberOfResultPages = offersListPage.NumberOfResultPages(totalAnnouncements);

                            // TODO extract in separate method
                            // Printing the footer of the page for console and streamWriter1
                            Console.WriteLine("--------------------------");
                            Console.WriteLine("Last 28 days in sector IT - Software Development and Maintenence");
                            Console.WriteLine("Containing key word \"{0}\" : {1}", keyWord, totalAnnouncements);
                            streamWriter.WriteLine("--------------------------");
                            streamWriter.WriteLine("Last 28 days in sector IT - Software Development and Maintenence");
                            streamWriter.WriteLine("Containing key word \"{0}\" : {1}", keyWord, totalAnnouncements);
                            // End of printing footer

                            // TODO extract in a method
                            // Printing the console and streamWriter1 -> results.txt
                            for (int j = 0; j < rowCollection.Count; j++)
                            {
                                if (rowCollection[j].IndexOf("Абонирайте се за новите обяви с избраните критерии по имейл") == -1)
                                {
                                    Console.Write(number + " ");
                                    streamWriter.WriteLine(number + " ");

                                    Console.WriteLine(rowCollection[j]);
                                    streamWriter.WriteLine(rowCollection[j]);
                                    count++;

                                    Console.WriteLine("-----");
                                    Console.WriteLine();
                                    streamWriter.WriteLine("-----");
                                    streamWriter.WriteLine();

                                }
                                else
                                {
                                    number--;
                                }

                                number++;
                            }
                            // End of printing the console and streamWriter1

                            startingPage += resultsOnPage;
                            count += resultsOnPage;
                        }

                        //Whole IT software sector
                        driver.Url = ComposeURLInJobsbg(keyWord, 15, 0);

                        totalAnnouncements = offersListPage.TotalNumberOfAnnouncements();

                        Console.WriteLine("Total: {0}", totalAnnouncements);
                        streamWriter.WriteLine("Total: {0}", totalAnnouncements);

                        ////TODO Find better way
                        //Whole SW sector today
                        driver.Url = (@"https://www.jobs.bg/front_job_search.php?zone_id=0&is_region=0&cities%5B%5D=1&categories%5B%5D=15&all_position_level=1&all_company_type=1&keyword=&last=2");

                        totalAnnouncements = offersListPage.TotalNumberOfAnnouncements();

                        Console.WriteLine("Today total: {0}", totalAnnouncements);
                        streamWriter.WriteLine("Today total: {0}", totalAnnouncements);

                        //Key word "Czech"
                        //driver.Url = (@"https://www.jobs.bg/front_job_search.php?frompage=0&zone_id=0&is_region=0&all_cities=0&categories%5B0%5D=15&all_position_level=1&all_company_type=1&keyword=Czech&last=0#paging");
                        //driver.Url = (@"https://www.jobs.bg/front_job_search.php?zone_id=0&is_region=0&all_cities=0&all_categories=0&all_position_level=1&all_company_type=1&keyword=Czech&last=0");

                        keyWord = "Czech";//"Automation QA .NET";//"QA";//"Чешки";//"";//".NET Test Automation";//"Test Automation";//
                        driver.Url = ComposeURLInJobsbg(keyWord, 0, 0);

                        totalAnnouncements = offersListPage.TotalNumberOfAnnouncements();

                        Console.WriteLine("Containing key word {0}: {1}", keyWord, totalAnnouncements);
                        Console.WriteLine("---------------------------");
                        streamWriter.WriteLine("Containing key word {0}: {0}", keyWord, totalAnnouncements);
                        streamWriter.WriteLine("---------------------------");

                        ////Saving Excel file
                        excelPackage.Save();

                        driver.Dispose();
                        driver.Quit();
                    }
                }
            }
        }

        private static IWebDriver StartBrowser()
        {
            IWebDriver driver = new ChromeDriver(); //new FirefoxDriver(); // new PhantomJSDriver();//
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            return driver;
        }

        private static string ComposeURLInJobsbg(string keyWord, int categoryNumber, int startingPage)
        {
            return "https://www.jobs.bg/front_job_search.php?frompage=" +
                startingPage.ToString() +
                "&zone_id=0&is_region=0&all_cities=0&" +
                "categories[0]=" +
                categoryNumber +
                "&all_position_level=1&all_company_type=1&keyword=" +
                keyWord +
                "&last=0#paging";
        }
    }

    public static class WebDriverExtensions
    {
        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElement(by));
            }
            return driver.FindElement(by);
        }
    }
}



////Excel creation cheatsheet
//FileInfo newFile = new FileInfo(@"\sample1.xlsx");

//if (newFile.Exists)
//{
//    newFile.Delete();
//    // ensures we create a new workbook 
//    newFile = new FileInfo(@"\sample1.xlsx");
//}
//ExcelPackage package = new ExcelPackage(newFile);

//using (package)
//{
//    // add a new worksheet to the empty workbook
//    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Inventory");
//    //Add the headers 
//    worksheet.Cells[1, 1].Value = "ID";
//    worksheet.Cells[1, 2].Value = "Product";
//    worksheet.Cells[1, 3].Value = "Quantity";
//    worksheet.Cells[1, 4].Value = "Price";
//    worksheet.Cells[1, 5].Value = "Value";
//    //Add some items... 
//    worksheet.Cells["A2"].Value = "12001";
//    worksheet.Cells["B2"].Value = "Nails";
//    worksheet.Cells["C2"].Value = 37;
//    worksheet.Cells["D2"].Value = 3.99;
//    worksheet.Cells["A3"].Value = "12002";
//    worksheet.Cells["B3"].Value = "Hammer";
//    worksheet.Cells["C3"].Value = 5;
//    worksheet.Cells["D3"].Value = 12.10;
//    worksheet.Cells["A4"].Value = "12003";
//    worksheet.Cells["B4"].Value = "Saw";
//    worksheet.Cells["C4"].Value = 12;
//    worksheet.Cells["D4"].Value = 15.37;
//    //Add a formula for the value-column
//    worksheet.Cells["E2:E4"].Formula = "C2*D2";
//    //Ok now format the values; 
//    using (var range = worksheet.Cells[1, 1, 1, 5])
//    {
//        range.Style.Font.Bold = true;
//        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
//        range.Style.Fill.BackgroundColor.SetColor(Color.DarkBlue);
//        range.Style.Font.Color.SetColor(Color.White);
//    }
//    worksheet.Cells["A5:E5"].Style.Border.Top.Style = ExcelBorderStyle.Thin;
//    worksheet.Cells["A5:E5"].Style.Font.Bold = true;
//    worksheet.Cells[5, 3, 5, 5].Formula = string.Format("SUBTOTAL(9,{0})", new ExcelAddress(2, 3, 4, 3).Address);
//    worksheet.Cells["C2:C5"].Style.Numberformat.Format = "#,##0";
//    worksheet.Cells["D2:E5"].Style.Numberformat.Format = "#,##0.00";
//    //Create an autofilter for the range 
//    worksheet.Cells["A1:E4"].AutoFilter = true;
//    worksheet.Cells["A1:E5"].AutoFitColumns(0);
//    // lets set the header text
//    worksheet.HeaderFooter.OddHeader.CenteredText = "&24&U&\"Arial,Regular Bold\" Inventory";
//    // add the page number to the footer plus the total number of pages
//    worksheet.HeaderFooter.OddFooter.RightAlignedText = string.Format("Page {0} of {1}",
//        ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
//    // add the sheet name to the footer 
//    worksheet.HeaderFooter.OddFooter.CenteredText = ExcelHeaderFooter.SheetName;
//    // add the file path to the footer
//    worksheet.HeaderFooter.OddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
//    worksheet.PrinterSettings.RepeatRows = worksheet.Cells["1:2"];
//    worksheet.PrinterSettings.RepeatColumns = worksheet.Cells["A:G"];
//    // Change the sheet view to show it in page layout mode 
//    worksheet.View.PageLayoutView = true;
//    // set some document properties
//    package.Workbook.Properties.Title = "Invertory";
//    package.Workbook.Properties.Author = "Jan Källman";
//    package.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 workbook using EPPlus";
//    // set some extended property values
//    package.Workbook.Properties.Company = "AdventureWorks Inc.";
//    // set some custom property values 
//    package.Workbook.Properties.SetCustomPropertyValue("Checked by", "Jan Källman");
//    package.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "EPPlus");
//    // save our new workbook and we are done!
//    package.Save();
//}
////return newFile.FullName;

//========================================

//////SQLite cheatsheet
//                 SQLiteConnection.CreateFile("MyDatabase.sqlite");

//                 SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
//                 m_dbConnection.Open();

//                 string sql = "create table highscores (name varchar(20), score int)";

//                 SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
//                 command.ExecuteNonQuery();

//                 sql = "insert into highscores (name, score) values ('Me', 9001)";

//                 command = new SQLiteCommand(sql, m_dbConnection);
//                 command.ExecuteNonQuery();

//                 m_dbConnection.Close();

//=========================================