using System;

namespace IndustryTower.Helpers
{
    public static class TimeHelper
    {
        public static string TimeAgo(this DateTime date)
        {
            TimeSpan timeSince = DateTime.UtcNow.Subtract(date);

            if (timeSince.TotalMilliseconds < 1) return Resource.TimeNames.notYet;
            if (timeSince.TotalMinutes < 1) return Resource.TimeNames.justNow;
            if (timeSince.TotalMinutes < 2) return Resource.TimeNames.oneMinute;
            if (timeSince.TotalMinutes < 60) return string.Format(Resource.TimeNames.minutesAgo, timeSince.Minutes);
            if (timeSince.TotalMinutes < 120) return Resource.TimeNames.oneHour;
            if (timeSince.TotalHours < 24) return string.Format(Resource.TimeNames.hoursAgo, timeSince.Hours);
            if (timeSince.TotalDays < 2) return Resource.TimeNames.yesterday;
            if (timeSince.TotalDays < 7) return string.Format(Resource.TimeNames.daysAgo, timeSince.Days);
            if (timeSince.TotalDays < 14) return Resource.TimeNames.lastWeek;
            if (timeSince.TotalDays < 21) return Resource.TimeNames.twoWeeksAgo;
            if (timeSince.TotalDays < 28) return Resource.TimeNames.threeWeeksAgo;
            if (timeSince.TotalDays < 60) return Resource.TimeNames.lastMonth;
            if (timeSince.TotalDays < 365) return string.Format(Resource.TimeNames.monthsAgo, Math.Round(timeSince.TotalDays / 30));
            if (timeSince.TotalDays < 730) return Resource.TimeNames.lastYear; //last but not least...
            return string.Format(Resource.TimeNames.yearsAgo, Math.Round(timeSince.TotalDays / 365));
        }

        public static string TimeLater(this DateTime date)
        {
            TimeSpan timeTo = date.Subtract(DateTime.UtcNow);

            if (timeTo.TotalMilliseconds < 1) return Resource.TimeNames.notYet;
            if (timeTo.TotalMinutes < 1) return Resource.TimeNames.justNow;
            if (timeTo.TotalMinutes < 2) return Resource.TimeNames.oneMinuteLater;
            if (timeTo.TotalMinutes < 60) return string.Format(Resource.TimeNames.minutesLater, timeTo.Minutes);
            if (timeTo.TotalMinutes < 120) return Resource.TimeNames.oneHourLater;
            if (timeTo.TotalHours < 24) return string.Format(Resource.TimeNames.hoursLater, timeTo.Hours);
            if (timeTo.TotalDays < 2) return Resource.TimeNames.tomrrow;
            if (timeTo.TotalDays < 7) return string.Format(Resource.TimeNames.daysLater, timeTo.Days);
            if (timeTo.TotalDays < 14) return Resource.TimeNames.nextWeek;
            if (timeTo.TotalDays < 21) return Resource.TimeNames.twoWeeksLater;
            if (timeTo.TotalDays < 28) return Resource.TimeNames.threeWeeksLater;
            if (timeTo.TotalDays < 60) return Resource.TimeNames.nextMonth;
            if (timeTo.TotalDays < 365) return string.Format(Resource.TimeNames.monthsLater, Math.Round(timeTo.TotalDays / 30));
            if (timeTo.TotalDays < 730) return Resource.TimeNames.nextYear; //last but not least...
            return string.Format(Resource.TimeNames.yearsLater, Math.Round(timeTo.TotalDays / 365));
        }
    }
    
}