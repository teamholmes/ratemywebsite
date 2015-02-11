using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OP.General.Extensions
{
    public static class DateTimeExtensions
    {

        public static string FriendlyString(this DateTime? date, string prefixtext, string postfixtext, Boolean ifNullAddPlacedholder, Boolean hidetime = false)
        {

            if (!date.HasValue && !ifNullAddPlacedholder) return string.Empty;

            if (!date.HasValue && ifNullAddPlacedholder) return "--";

            if (!string.IsNullOrEmpty(prefixtext))
            {
                prefixtext += " ";
            }

            if (!string.IsNullOrEmpty(postfixtext))
            {
                postfixtext = " " + postfixtext;
            }

            if (hidetime)
            {
                return string.Format("{0}{1}{2}", prefixtext, date.Value.ToShortDateString(), postfixtext);
            }

            return string.Format("{2}{0} at {1}{3}", date.Value.ToShortDateString(), date.Value.ToShortTimeString(), prefixtext, postfixtext);
        }





        public static string ZeroFormattedDD(this DateTime date)
        {
            return date.ToString("dd");
        }

        public static string ZeroFormattedMM(this DateTime date)
        {
            return date.ToString("MM");
        }

        public static string FormattedYYYY(this DateTime date)
        {
            return date.ToString("yyyy");
        }

        public static DateTime BeginningOfDay(this DateTime date)
        {
            return date.Date;
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// To the short friendly string.
        /// </summary>
        /// <param name="dt">The datetime.</param>
        /// <returns></returns>
        public static String ToFriendlyTimeSpan(this DateTime value)
        {
            var ts = new TimeSpan(DateTime.UtcNow.Ticks - value.Ticks);

            double delta = ts.TotalSeconds;
            if (delta < 60)
            {
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
            }
            if (delta < 120)
            {
                return "a minute ago";
            }
            if (delta < 2700) // 45 * 60
            {
                return ts.Minutes + " minutes ago";
            }
            if (delta < 5400) // 90 * 60
            {
                return "an hour ago";
            }
            if (delta < 86400) // 24 * 60 * 60
            {
                return ts.Hours + " hours ago";
            }
            if (delta < 172800) // 48 * 60 * 60
            {
                return "yesterday";
            }
            if (delta < 2592000) // 30 * 24 * 60 * 60
            {
                return ts.Days + " days ago";
            }
            if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "one month ago" : months + " months ago";
            }
            var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "one year ago" : years + " years ago";
        }


        public static DateTime LastDateOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }


        public static string FormatToDatePickerDate(this DateTime date)
        {

            //new Date (2014,10,1)
            //return string.Format("new Date ({0},{1},{2})", date.Year, date.Month - 1, date.Day);
            return string.Format("{0}/{1}/{2}", date.Day, date.Month, date.Year);
            //return string.Format("'{0}-{1}-{2}'", date.Year, date.Month, date.Day);
        }


        public static string FormattoJavascriptDate(this DateTime date)
        {

            //new Date (2014,10,1)
            //return string.Format("new Date ({0},{1},{2})", date.Year, date.Month - 1, date.Day);
            return string.Format("{0}/{1}/{2}", date.Day, date.Month, date.Year);
            //return string.Format("'{0}-{1}-{2}'", date.Year, date.Month, date.Day);
        }




    }
}
