using System;
using System.Globalization;
using FHICORC.Enums;
using FHICORC.Services;

namespace FHICORC.Utils
{
    public static class DateUtils
    {
        public static string DateToScannerResultFormat(this DateTime date)
        {
            return date.ToString("dd. MMM. yyyy", CultureInfo.InvariantCulture).ToLower();
        }

        public static string ToLocaleDateFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.CreateSpecificCulture(LocaleService.Current.GetLanguage().ToISOCode()));
        }

        public static string ToLocaleTimeFormat(this DateTime date)
        {
            return date.ToString("t", CultureInfo.CreateSpecificCulture(LocaleService.Current.GetLanguage().ToISOCode()));
        }

        public static string ToLocaleDateTimeFormat(this DateTime date)
        {
            return date.ToString("f", CultureInfo.CreateSpecificCulture(LocaleService.Current.GetLanguage().ToISOCode()));
        }
    }
}
