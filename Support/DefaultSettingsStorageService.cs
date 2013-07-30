using System.Collections.Generic;
using System.Configuration;
using Theodorus2.Interfaces;
using Theodorus2.Properties;

namespace Theodorus2.Support
{
    public class DefaultSettingsStorageService : ISettingsStorageService
    {
        public T GetValue<T>(string key)
        {
            return (T) Settings.Default[key];
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            try
            {
                value = (T) Settings.Default[key];
                return true;
            }
            catch (SettingsPropertyNotFoundException)
            {

                value = default(T);
                return false;
            }
        }

        public void SetValue<T>(string key, T val)
        {
            Settings.Default[key] = val;
        }
    }
}