namespace Theodorus2.Interfaces
{
    public interface ISettingsStorageService
    {
        T GetValue<T>(string key);
        void SetValue<T>(string key, T val);
    }
}