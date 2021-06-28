using System;
namespace FHICORC.Enums
{
    public enum LanguageSelection
    {
        Bokmal,
        Nynorsk,
        English
    }

    public static class LanguageSelectionExtensions
    {
        public static string ToISOCode(this LanguageSelection languageSelection)
        {
            return languageSelection switch
            {
                LanguageSelection.Bokmal => "nb",
                LanguageSelection.Nynorsk => "nn",
                LanguageSelection.English => "en",
                _ => "nb",
            };
        }

        public static LanguageSelection FromISOCode(this string isoCode)
        {
            return isoCode switch
            {
                "nb" => LanguageSelection.Bokmal,
                "nn" => LanguageSelection.Nynorsk,
                "en" => LanguageSelection.English,
                _ => LanguageSelection.Bokmal,
            };
        }
    }
}
