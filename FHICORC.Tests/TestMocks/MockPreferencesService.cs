using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FHICORC.Core.Data;

namespace FHICORC.Tests.TestMocks
{
    public class MockPreferencesService : IPreferencesService
    {
        private Dictionary<string, object> _dict = new Dictionary<string, object>();

        public void ClearAllUserPreferences()
        {
            _dict = new Dictionary<string, object>();
        }

        public void ClearUserPreference(string key)
        {
            _dict.Remove(key);
        }

        public bool GetUserPreferenceAsBoolean(string key)
        {
            try
            {
                return (_dict[key] as bool?) ?? false;
            }
            catch
            {
                return false;
            }
        }

        public int GetUserPreferenceAsInt(string key)
        {
            try
            {
                return (_dict[key] as int?) ?? -1;
            }
            catch
            {
                return -1;
            }
        }

        public long GetUserPreferenceAsLong(string key)
        {
            try
            {
                return (_dict[key] as long?) ?? 0;
            }
            catch
            {
                return -1;
            }
        }

        public string GetUserPreferenceAsString(string key)
        {
            try
            {
                return _dict[key] as string;
            }
            catch
            {
                return "";
            }
        }

        public void SetUserPreference(string key, string value)
        {
            _dict[key] = value;
        }

        public void SetUserPreference(string key, bool value)
        {
            _dict[key] = value;
        }

        public void SetUserPreference(string key, int value)
        {
            _dict[key] = value;
        }

        public void SetUserPreference(string key, long value)
        {
            _dict[key] = value;
        }
    }
}
