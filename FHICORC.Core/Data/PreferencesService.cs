using System;
using Xamarin.Essentials;

namespace FHICORC.Core.Data
{
    public class PreferencesService : IPreferencesService
    {
        public void SetUserPreference(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public void SetUserPreference(string key, bool value)
        {
            Preferences.Set(key, value);
        }

        public void SetUserPreference(string key, int value)
        {
            Preferences.Set(key, value);
        }

        public void SetUserPreference(string key, long value)
        {
            Preferences.Set(key, value);
        }

        public bool GetUserPreferenceAsBoolean(string key)
        {
            return Preferences.Get(key, false);
        }

        public string GetUserPreferenceAsString(string key)
        {
            return Preferences.Get(key, string.Empty);
        }

        public int GetUserPreferenceAsInt(string key)
        {
            return Preferences.Get(key, -1);
        }


        public long GetUserPreferenceAsLong(string key)
        {
            return Preferences.Get(key, (long)0);
        }

        public void ClearUserPreference(string key)
        {
            Preferences.Clear(key);
        }

        public void ClearAllUserPreferences()
        {
            Preferences.Clear();
        }
    }
}
