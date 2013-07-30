namespace Theodorus2.Interfaces
{
    public interface ISettingsStorageService
    {
        T GetValue<T>(string key);
        bool TryGetValue<T>(string key, out T value);
        void SetValue<T>(string key, T val);
    }
}