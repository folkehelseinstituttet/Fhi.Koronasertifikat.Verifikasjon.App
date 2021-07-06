using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using FHICORC.Enums;
using FHICORC.Configuration;
using FHICORC.Core.Data;
using FHICORC.Data;
using System.Diagnostics;
using System.Globalization;

namespace FHICORC.Services
{
    public class LocaleService
    {

        public static LocaleService Current { get; set; } = new LocaleService();

        private Dictionary<string, string> _translations = new Dictionary<string, string>();

        private string _notFoundSymbol = "$";
        private string _locale;
        private JsonKvpReader _reader = new JsonKvpReader();

        private bool _loadedFromEmbeddedFile { get; set; }
        public bool LoadedFromEmbeddedFile
        {
            get
            {
                return _loadedFromEmbeddedFile;
            }
            set
            {
                _loadedFromEmbeddedFile = value;
            }
        }

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

        public void LoadLocale(string isoCode, Stream localeStream, bool loadedFromEmbeddedFile)
        {
            _translations.Clear();
            try
            {
                _translations = _reader.Read(localeStream) ?? new Dictionary<string, string>();
                LoadedFromEmbeddedFile = loadedFromEmbeddedFile;
                IoCContainer.Resolve<IPreferencesService>().SetUserPreference(PreferencesKeys.LANGUAGE_SETTING, isoCode);
            }
            catch (Exception)
            {
                throw new JsonReaderException();
            }

            _locale = isoCode;
        }

        public T GetClassValueForKey<T>(string key) where T : class
        {
            if (_translations.ContainsKey(key))
            {
                string valueFromTextFile = _translations[key];
                try
                {
                    T value = (T)Convert.ChangeType(valueFromTextFile, typeof(T), CultureInfo.InvariantCulture);
                    return value;
                }
                catch (Exception e)
                {
                    Debug.Print($"{nameof(LocaleService)}.{nameof(GetClassValueForKey)}: Could not retrieve " +
                        $"the value from text file, caught {e} with message {e.Message}");
                    return null;
                }
            }
            return null;
        }

        public void Reset()
        {
            Current = new LocaleService();
        }
    }
}
