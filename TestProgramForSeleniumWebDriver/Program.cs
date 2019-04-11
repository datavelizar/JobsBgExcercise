namespace TestProgramForSeleniumWebDriver
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using OfficeOpenXml;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using Utils;

    public class Program
    {
        public static void Main()
        {

            string keyWord = "Test Automation";//"Чешки";//"QA";//"Czech";////
            int category = 0;//0=allCategories;15=SW
            string pathStr = Utils.CreateNameFromDateTimeNow(keyWord + "_results.txt");
            string pathStr2 = Utils.CreateNameFromDateTimeNow(keyWord + "_results2.txt");
            string pathStr3 = "results3.xlsx";
            string excellSheetName = Utils.CreateNameFromDateTimeNow(keyWord);

            FileInfo newFile = new FileInfo(pathStr3);

            //if (newFile.Exists)
            //{
            //    newFile.Delete();
            //    //// ensures we create a new workbook 
            //    newFile = new FileInfo(pathStr3);
            //}
            ExcelPackage package = new ExcelPackage(newFile);

            ////TODO Save results in a different directory than "bin/debug"
            //StreamWriter writer = new StreamWriter(@"\..\" + pathStr);
            StreamWriter writer = new StreamWriter(pathStr);
            StreamWriter writer2 = new StreamWriter(pathStr2);
            //StreamWriter writer3 = new StreamWriter(pathStr3);

            using (package)
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(excellSheetName);

                //using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                //{
                //    // Or you can get the file content without saving it:
                //    string htmlCode = client.DownloadString("http://www.yahoo.com");
                //    System.IO.File.WriteAllText(@"C:\Users\Ryan\Desktop\test.txt", htmlCode);
                //    //...
                //}


                using (writer)
                {
                    using (writer2)
                    {
                        IWebDriver driver = new ChromeDriver(); //new FirefoxDriver(); // new PhantomJSDriver();//
                        driver.Manage().Window.Maximize();
                        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);

                        ////Whole Category "IT - Sowtware"; Keyword is String.Empty, Category is 15
                        driver.Url = ComposeURLInJobsbg("", 15, 0);

                        //finding the total result of the category
                        var resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
                        var splittedResultsLabel = resultsLabel.Text.Split(' ');
                        var totalAnnouncements = 0;

                        if (splittedResultsLabel.Length > 0)
                        {
                            totalAnnouncements = int.Parse(splittedResultsLabel[splittedResultsLabel.Length - 1]);
                        }

                        Console.WriteLine("Total: {0}; At: {1}", totalAnnouncements, DateTime.Now.ToString());
                        writer.WriteLine("Total: {0}; At: {1}", totalAnnouncements, DateTime.Now.ToString());
                        writer2.WriteLine("Total: {0}; At: {1}", totalAnnouncements, DateTime.Now.ToString());

                        int startingPage = 0;
                        int number = 1;
                        int count = 0;
                        int resultsOnPage = 15;
                        int excelRow = 2;

                        for (int i = 0; i < totalAnnouncements / resultsOnPage; i++)
                        {
                            //Use the already declared keyword and category (SW sector is category=15) 
                            string resultURL = ComposeURLInJobsbg(keyWord, category, startingPage);

                            driver.Url = (resultURL);

                            //Finding all rows with announcements and creating a collection with the title texts 
                            try
                            {
                                driver.FindElements(By.ClassName("offerslistRow"));
                                driver.FindElements(By.ClassName("joblink"));
                                driver.FindElements(By.ClassName("company_link"));
                            }
                            catch (Exception)
                            {
                                driver.Navigate().Refresh();
                            }

                            var currentPageRowsCollection = driver.FindElements(By.ClassName("offerslistRow"));
                            var currentPageJobLinksCollection = driver.FindElements(By.ClassName("joblink"));
                            var currentPageCompanyLinksCollection = driver.FindElements(By.ClassName("company_link"));
                            var currentPageOffersDatesCollection = driver.FindElements(By.ClassName("explainGray"));

                            var textsCollection = new List<string>();
                            var linksCollection = new List<string>();
                            var titlesCollection = new List<string>();
                            var companyTitlesCollection = new List<string>();
                            var datesCollection = new List<string>();

                            foreach (var row in currentPageRowsCollection)
                            {
                                textsCollection.Add(row.Text);
                            }

                            foreach (var item in currentPageJobLinksCollection)
                            {
                                titlesCollection.Add(item.Text);
                                linksCollection.Add(item.GetAttribute("href"));
                            }

                            foreach (var item in currentPageCompanyLinksCollection)
                            {
                                companyTitlesCollection.Add(item.Text);
                            }

                            foreach (var item in currentPageOffersDatesCollection)
                            {
                                datesCollection.Add(item.Text);
                            }


                            //Collection of the number of looks on each annoucement
                            var offers = new List<Announcement>();

                            for (int m = 0; m < titlesCollection.Count; m++)
                            {
                                driver.Url = (resultURL);

                                try
                                {
                                    driver.FindElements(By.ClassName("joblink"));
                                }
                                catch (Exception)
                                {
                                    driver.Navigate().Refresh();
                                }

                                var a = titlesCollection[m];
                                var b = companyTitlesCollection[m];
                                var e = linksCollection[m];
                                var f = datesCollection[m];

                                var announcement = new Announcement();

                                announcement.CompanyName = a;
                                announcement.CompanyOffer = b;
                                announcement.OfferLink = e;
                                announcement.Date = f;

                                //Showing the offer contents
                                currentPageJobLinksCollection = driver.FindElements(By.ClassName("joblink"));
                                currentPageJobLinksCollection[m].Click();

                                try
                                {
                                    driver.FindElement(By.Id("cnt_box"));
                                }
                                catch (Exception)
                                {
                                    driver.Navigate().Refresh();
                                }

                                var offerLooks = driver.FindElement(By.Id("cnt_box")).Text.Split(':');
                                announcement.OfferLooks = offerLooks[offerLooks.Length - 1];
                               // var fullOfferText = driver.FindElement(By.TagName("body")).Text;
                                var fullOfferText = driver.FindElement(By.XPath("/html/body/table[2]/tbody/tr/td/table/tbody")).Text;
                                

                                offers.Add(announcement);
                                driver.Url = (resultURL);

                                ////TODO better typing
                                writer2.WriteLine(String.Format("{0} | {1} | {2} | {3} | {4}",
                                    announcement.Date,
                                    announcement.CompanyOffer,
                                    announcement.CompanyName,
                                    announcement.OfferLooks,
                                    announcement.OfferLink
                                    ));

                                ////Filling Excel cells
                                worksheet.Cells["A" + excelRow].Value = announcement.Date;
                                worksheet.Cells["B" + excelRow].Value = announcement.CompanyOffer;
                                worksheet.Cells["C" + excelRow].Value = announcement.CompanyName;
                                worksheet.Cells["D" + excelRow].Value = announcement.OfferLooks;//int.Parse(announcement.OfferLooks.Replace(" ", String.Empty));//
                                worksheet.Cells["E" + excelRow].Value = announcement.OfferLink;
                                worksheet.Cells["F" + excelRow].Value = fullOfferText;

                                excelRow++;
                            }

                            //resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
                            resultsLabel = driver.FindElement(By.CssSelector(@"#search_results_div > table:nth-child(1) > tbody:nth-child(1) > tr:nth-child(1) > td:nth-child(1) > table:nth-child(2) > tbody:nth-child(1) > tr:nth-child(3) > td:nth-child(1)"));

                            splittedResultsLabel = resultsLabel.Text.Split(' ');
                            totalAnnouncements = 0;

                            if (splittedResultsLabel.Length > 0)
                            {
                                totalAnnouncements = int.Parse(splittedResultsLabel[splittedResultsLabel.Length - 1]);
                            }

                            Console.WriteLine("--------------------------");
                            Console.WriteLine("Last 28 days in sector IT - Software Development and Maintenence");
                            Console.WriteLine("Containing key word \"{0}\" : {1}", keyWord, totalAnnouncements);
                            writer.WriteLine("--------------------------");
                            writer.WriteLine("Last 28 days in sector IT - Software Development and Maintenence");
                            writer.WriteLine("Containing key word \"{0}\" : {1}", keyWord, totalAnnouncements);

                            for (int j = 0; j < textsCollection.Count; j++)
                            {
                                if (count % 4 == 1)
                                {
                                    Console.Write(number + " ");
                                    writer.WriteLine(number + " ");
                                    number++;
                                }
                                Console.WriteLine(textsCollection[j]);
                                writer.WriteLine(textsCollection[j]);
                                count++;
                            }

                            startingPage += resultsOnPage;
                            count += resultsOnPage;
                        }

                        //Whole IT software sector
                        driver.Url = ComposeURLInJobsbg(keyWord, 15, 0);

                        resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
                        splittedResultsLabel = resultsLabel.Text.Split(' ');
                        totalAnnouncements = 0;

                        if (splittedResultsLabel.Length > 0)
                        {
                            totalAnnouncements = int.Parse(splittedResultsLabel[splittedResultsLabel.Length - 1]);
                        }

                        Console.WriteLine("Total: {0}", totalAnnouncements);
                        writer.WriteLine("Total: {0}", totalAnnouncements);

                        ////TODO Find better way
                        //Whole SW sector today
                        driver.Url = (@"https://www.jobs.bg/front_job_search.php?zone_id=0&is_region=0&cities%5B%5D=1&categories%5B%5D=15&all_position_level=1&all_company_type=1&keyword=&last=2");

                        resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
                        splittedResultsLabel = resultsLabel.Text.Split(' ');
                        totalAnnouncements = 0;

                        if (splittedResultsLabel.Length > 0)
                        {
                            totalAnnouncements = int.Parse(splittedResultsLabel[splittedResultsLabel.Length - 1]);
                        }

                        Console.WriteLine("Today total: {0}", totalAnnouncements);
                        writer.WriteLine("Today total: {0}", totalAnnouncements);

                        //Key word "Czech"
                        //driver.Url = (@"https://www.jobs.bg/front_job_search.php?frompage=0&zone_id=0&is_region=0&all_cities=0&categories%5B0%5D=15&all_position_level=1&all_company_type=1&keyword=Czech&last=0#paging");
                        //driver.Url = (@"https://www.jobs.bg/front_job_search.php?zone_id=0&is_region=0&all_cities=0&all_categories=0&all_position_level=1&all_company_type=1&keyword=Czech&last=0");
                        driver.Url = ComposeURLInJobsbg("Czech", 0, 0);

                        resultsLabel = driver.FindElement(By.XPath("//*[@id=\"search_results_div\"]/table/tbody/tr/td[1]/table/tbody/tr[3]/td[1]"));
                        splittedResultsLabel = resultsLabel.Text.Split(' ');
                        totalAnnouncements = 0;

                        if (splittedResultsLabel.Length > 0)
                        {
                            totalAnnouncements = int.Parse(splittedResultsLabel[splittedResultsLabel.Length - 1]);
                        }

                        Console.WriteLine("Containing key word \"Czech\": {0}", totalAnnouncements);
                        Console.WriteLine("---------------------------");
                        writer.WriteLine("Containing key word \"Czech\": {0}", totalAnnouncements);
                        writer.WriteLine("---------------------------");

                        ////Saving Excel file
                        package.Save();

                        driver.Dispose();
                        driver.Quit();
                    }
                }
            }
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