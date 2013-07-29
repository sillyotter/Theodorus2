using System.Collections.Generic;
using System.Linq;
using Theodorus2.Interfaces;
using Theodorus2.ViewModels;
using UnitTests.Support;
using Xunit;

namespace UnitTests.ViewModels
{
    public class ConnectionDialogViewModelTests
    {
        private class DummyFileSelectorService : IFileSelectionService
        {
            public string PromptToOpenFile(string defaultExtension, string filters)
            {
                return "test.db";
            }

            public string PromptToSaveFile(string defaultExtension, string filters)
            {
                return "test.db";
            }
        }

        [Fact]
        public void ConnectionDialogCancelTest()
        {
            using (var target = new ConnectionDialogViewModel(new DummyFileSelectorService()))
            {
                var res = new List<bool>();
                target.Results.ToCollection(res);
                target.CancelCommand.Execute(null);
                Assert.False(res.Single());
            }
        }

        [Fact]
        public void ConnectionDialogOkTest()
        {
            using(var target = new ConnectionDialogViewModel(new DummyFileSelectorService()))
            {
                var res = new List<bool>();
                target.Results.ToCollection(res);
                Assert.False(target.OkCommand.CanExecute(null));
                target.DataSource = ":memory:";
                Assert.True(target.OkCommand.CanExecute(null));

                target.OkCommand.Execute(null);
                Assert.True(res.Single());
            }
        }

        [Fact]
        public void TestBrowseCommand()
        {
            using(var target = new ConnectionDialogViewModel(new DummyFileSelectorService()))
            {
                target.BrowseCommand.Execute(null);
                Assert.Equal("test.db", target.DataSource);
            }
        }

        [Fact]
        public void ConnectionDialogAllValidators()
        {
            using (var target = new ConnectionDialogViewModel(new DummyFileSelectorService()))
            {
                var res = new List<bool>();
                target.Results.ToCollection(res);
                Assert.False(target.OkCommand.CanExecute(null));
                target.DataSource = ":memory:";
                Assert.True(target.OkCommand.CanExecute(null));

                target.CacheSize = -1;
                Assert.False(target.OkCommand.CanExecute(null));
                target.CacheSize = 4096;
                Assert.True(target.OkCommand.CanExecute(null));

                target.DefaultTimeout = -1;
                Assert.False(target.OkCommand.CanExecute(null));
                target.DefaultTimeout = 30;
                Assert.True(target.OkCommand.CanExecute(null));

                target.MaxPageCount = -1;
                Assert.False(target.OkCommand.CanExecute(null));
                target.MaxPageCount = 0;
                Assert.True(target.OkCommand.CanExecute(null));

                target.PageSize = -1;
                Assert.False(target.OkCommand.CanExecute(null));
                target.PageSize = 4095;
                Assert.False(target.OkCommand.CanExecute(null));
                target.PageSize = 4096;
                Assert.True(target.OkCommand.CanExecute(null));

                target.OkCommand.Execute(null);
                Assert.True(res.Single());
            }
        }
    }
}