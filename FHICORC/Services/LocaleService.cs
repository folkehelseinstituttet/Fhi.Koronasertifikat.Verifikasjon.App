using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using FHICORC.Enums;

namespace FHICORC.Services
{
    public class LocaleService
    {

        public static LocaleService Current { get; set; } = new LocaleService();

        private Dictionary<string, string> _translations = new Dictionary<string, string>();
        private IList<string> _locales = new List<string> { "dk" };

        private string _notFoundSymbol = "$";
        private string _locale;
        private JsonKvpReader _reader = new JsonKvpReader();

        public LocaleService()
        {
        }

        public string this[string key]
         => Translate(key);

        public string Translate(string key, params object[] args)
        {
            if (_translations.ContainsKey(key))
                return args.Length == 0
                    ? _translations[key]
                    : string.Format(_translations[key], args);

            return $"{_notFoundSymbol}{key}{_notFoundSymbol}";
        }

        public LanguageSelection GetLanguage()
        {
            return _locale.FromISOCode();
        }

        public void LoadLocale(string isoCode, Stream localeStream)
        {
            _translations.Clear();
            try
            {
                _translations = _reader.Read(localeStream) ?? new Dictionary<string, string>();
            }
            catch (Exception)
            {
                throw new JsonReaderException();
            }

            _locale = isoCode;
        }

        public void Reset()
        {
            Current = new LocaleService();
        }
    }
}
