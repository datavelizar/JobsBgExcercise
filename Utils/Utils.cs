using System;

namespace Utils
{
    public class Utils
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

            var pathStr = year + "_" + monthStr + "_" + dayStr + "_" + hourStr + "_" +
                minutesStr + "_" + secondsStr + "_" + endOfName;

            return pathStr;
        }

        public static string MakeDoubleDigitString(int number)
        {
            var doubleDigitString = "";
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
