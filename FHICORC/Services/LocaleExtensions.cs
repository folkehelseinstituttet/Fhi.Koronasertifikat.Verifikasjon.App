namespace FHICORC.Services
{
    public static class LocaleExtensions
    {
        public static string Translate(this string key) => LocaleService.Current.Translate(key);
    }
}
