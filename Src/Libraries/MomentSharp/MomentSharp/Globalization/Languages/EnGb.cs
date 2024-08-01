using System;

namespace MomentSharp.Globalization.Languages
{
    /// <summary>
    ///  Localization for English (UK) (En-GB)
    /// </summary>
    public class EnGb : ILocalize
    {
        /// <summary>
        /// English locazation implementation constructor
        /// </summary>
        public EnGb()
        {
            LongDateFormat = new LongDateFormat
            {
                Lt = "HH:mm",
                Lts = "HH:mm:ss",
                L = "dd/MM/yyyy",
                LL = "dd MMMM yyyy"
            };

            LongDateFormat.LLL = string.Format("dd MMMM yyyy {0}", LongDateFormat.Lt);
            LongDateFormat.LLLL = string.Format("dddd, dd MMMM yyyy {0}", LongDateFormat.Lt);
        }

        /// <summary>
        /// Localized short hand format strings. See http://momentjs.com/docs/#localized-formats
        /// </summary>
        public LongDateFormat LongDateFormat { get; set; }

        /// <summary>
        /// Localized <see cref="Calendar"/> parts for <paramref name="dateTime"/>
        /// </summary>
        /// <param name="calendar">Calendar Part</param>
        /// <param name="dateTime">DateTime to use in format string</param>
        /// <returns>Localized string e.g. Today at 09:00</returns>
        public string Translate(Calendar calendar, DateTime dateTime)
        {
            return calendar switch
            {
                Calendar.SameDay => string.Format("Today at {0}", dateTime.ToString(LongDateFormat.Lt)),
                Calendar.NextDay => string.Format("Tomorrow at {0}", dateTime.ToString(LongDateFormat.Lt)),
                Calendar.NextWeek => string.Format("{0} at {1}", dateTime.ToString("dddd"), dateTime.ToString(LongDateFormat.Lt)),
                Calendar.LastDay => string.Format("Yesterday at {0}", dateTime.ToString(LongDateFormat.Lt)),
                Calendar.LastWeek => string.Format("{0} at {1}", dateTime.ToString("dddd"), dateTime.ToString(LongDateFormat.Lt)),
                Calendar.SameElse => dateTime.ToString(LongDateFormat.L),
                _ => "",
            };
        }


        /// <summary>
        /// Localize <see cref="RelativeTime"/>. This is meant to emulate how MomentJs allows localization of RelativeTime
        /// </summary>
        /// <param name="relativeTime"><see cref="RelativeTime"/></param>
        /// <param name="number">Difference amount</param>
        /// <param name="showSuffix">Should suffix? e.g. "ago"</param>
        /// <param name="isFuture">Difference is in the future or not. e.g. Yesterday vs Tomorrow</param>
        /// <returns>Localized realtive time e.g.: 5 seconds ago</returns>
        public string Translate(RelativeTime relativeTime, int number, bool showSuffix, bool isFuture)
        {
            var results = string.Empty;
            switch (relativeTime)
            {
                case RelativeTime.Seconds:
                    results = "a few seconds";
                    break;
                case RelativeTime.Minute:
                    results = "a minute";
                    break;
                case RelativeTime.Minutes:
                    results = string.Format("{0} minutes", number);
                    break;
                case RelativeTime.Hour:
                    results = "an hour";
                    break;
                case RelativeTime.Hours:
                    results = string.Format("{0} hours", number);
                    break;
                case RelativeTime.Day:
                    results = "a day";
                    break;
                case RelativeTime.Days:
                    results = string.Format("{0} days", number);
                    break;
                case RelativeTime.Month:
                    results = "a month";
                    break;
                case RelativeTime.Months:
                    results = string.Format("{0} months", number);
                    break;
                case RelativeTime.Year:
                    results = "a year";
                    break;
                case RelativeTime.Years:
                    results = string.Format("{0} years", number);
                    break;
            }
            return !showSuffix ? results : string.Format(isFuture ? "in {0}" : "{0} ago", results);
        }
    }
}