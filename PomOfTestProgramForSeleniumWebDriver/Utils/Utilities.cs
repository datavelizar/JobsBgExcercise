using System;
using System.Collections.Generic;
using System.Text;

namespace PomOfTestProgramForSeleniumWebDriver.Utils
{
    public class Utilities
    {
        public static string CreateNameFromDateTimeNow(string endOfName)
        {
            var now = DateTime.Now;
            var year = now.Year.ToString();
            var month = now.Month;
            var day = now.Day;
            var hour = now.Hour;
            var minutes = now.Minute;
            var seconds = now.Second;

            var monthStr = MakeDoubleDigitString(month);
            var dayStr = MakeDoubleDigitString(day);
            var hourStr = MakeDoubleDigitString(hour);
            var minutesStr = MakeDoubleDigitString(minutes);
            var secondsStr = MakeDoubleDigitString(seconds);

            var pathStr = year + monthStr + dayStr + "_" + hourStr + minutesStr + secondsStr + "_" + endOfName;

            return pathStr;
        }

        private static string MakeDoubleDigitString(int number)
        {
            string doubleDigitString;
            if (number < 10)
            {
                doubleDigitString = "0" + number.ToString();
            }
            else
            {
                doubleDigitString = number.ToString();
            }

            return doubleDigitString;
        }
    }
}
