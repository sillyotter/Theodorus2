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

        public void SetValue<T>(string key, T val)
        {
            Settings.Default[key] = val;
        }
    }
}