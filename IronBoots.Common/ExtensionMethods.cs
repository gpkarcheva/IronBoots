﻿using System.Globalization;
using static IronBoots.Common.EntityValidationConstants.DateTimeValidation;

namespace IronBoots.Common
{
    public static class ExtensionMethods
    {
        public static bool IsDateValid(string date)
        {
            DateTime parsedDate;
            return DateTime.TryParseExact(date, RequiredDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
        }

        public static bool IsTimeValid(string time)
        {
            DateTime parsedTime;
            return DateTime.TryParseExact(time, TimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedTime);
        }
    }
}