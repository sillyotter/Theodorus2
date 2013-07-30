using System.Collections.Generic;
using System.Linq;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;
using UnitTests.Support;
using Xunit;

namespace UnitTests.ViewModels
{
    public class OptionsDialogViewModelTest
    {
        private readonly ISettingsStorageService _srv = new DummySettingsStorageService();

        [Fact]
        public void OptionsDialogVmTestCancel()
        {
            using (var target = new OptionsDialogViewModel(_srv))
            {
                Assert.False(target.HasErrors);
                var res = new List<bool>();
                using (target.Results.ToCollection(res))
                {
                    target.CancelCommand.Execute(null);
                    Assert.Equal(1, res.Count);
                    Assert.False(res.Single());
                }
            }
        }

        [Fact]
        public void OptionsDialogVmTestOk()
        {
            using (var target = new OptionsDialogViewModel(_srv))
            {
                Assert.False(target.HasErrors);
                var res = new List<bool>();
                using (target.Results.ToCollection(res))
                {
                    target.OkCommand.Execute(null);
                    Assert.Equal(1, res.Count);
                    Assert.True(res.Single());
                }
            }
        }

        [Fact]
        public void OptionsDialogVmTestCancelDoesntSave()
        {
            using (var target = new OptionsDialogViewModel(_srv))
            {
                var orig = target.ResultsLimit;
                target.ResultsLimit = orig*2;
                target.CancelCommand.Execute(null);
                var newTarget = new OptionsDialogViewModel(_srv);
                Assert.Equal(orig, newTarget.ResultsLimit);
            }
        }

        [Fact]
        public void OptionsDialogVmTestOkDoesSave()
        {
            using (var target = new OptionsDialogViewModel(_srv))
            {
                var orig = target.ResultsLimit;
                target.ResultsLimit = orig*2;
                target.OkCommand.Execute(null);
                var newTarget = new OptionsDialogViewModel(_srv);
                Assert.NotEqual(orig, newTarget.ResultsLimit);
                Assert.Equal(orig*2, newTarget.ResultsLimit);
            }
        }

        [Fact]
        public void OptionsDialogVmTestCanExecuteOk()
        {
            using (var target = new OptionsDialogViewModel(_srv))
            {
                Assert.False(target.HasErrors);
                target.ResultsLimit = -1;
                Assert.True(target.HasErrors);
                Assert.False(target.OkCommand.CanExecute(null));
                target.ResultsLimit = 1000;
                Assert.False(target.HasErrors);
            }
        }
    }

    internal class DummySettingsStorageService : ISettingsStorageService 
    {
        private readonly Dictionary<string,object> _storage = new Dictionary<string, object>();

        public DummySettingsStorageService()
        {
            _storage["ShowLineNumbers"] = false;
            _storage["ResultLimit"] = 100;
            _storage["FontSize"] = 13;
            _storage["Font"] = "Consolas";
            _storage["ResultStyle"] = "ASDF";
        }

        public T GetValue<T>(string key)
        {
            return (T)_storage[key];
        }

        public bool TryGetValue<T>(string key, out T value)
        {
            object v;
            var res = _storage.TryGetValue(key, out v);
            value = (T)v;
            return res;
        }

        public void SetValue<T>(string key, T val)
        {
            _storage[key] = val;
        }
    }
}