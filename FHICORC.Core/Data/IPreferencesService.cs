using System;
namespace FHICORC.Core.Data
{
    public interface IPreferencesService
    {
        void SetUserPreference(string key, string value);
        void SetUserPreference(string key, bool value);
        void SetUserPreference(string key, int value);
        void SetUserPreference(string key, long value);
        bool GetUserPreferenceAsBoolean(string key);
        string GetUserPreferenceAsString(string key);
        int GetUserPreferenceAsInt(string key);
        long GetUserPreferenceAsLong(string key);
        void ClearUserPreference(string key);
        void ClearAllUserPreferences();
    }
}
