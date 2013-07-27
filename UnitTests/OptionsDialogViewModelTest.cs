using System.Collections.Generic;
using System.Linq;
using Theodorus2.ViewModels;
using Xunit;

namespace UnitTests
{
    public class OptionsDialogViewModelTest
    {
        [Fact]
        public void OptionsDialogVmTestCancel()
        {
            using (var target = new OptionsDialogViewModel())
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
            using (var target = new OptionsDialogViewModel())
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
            using (var target = new OptionsDialogViewModel())
            {
                var orig = target.ResultsLimit;
                target.ResultsLimit = orig*2;
                target.CancelCommand.Execute(null);
                var newTarget = new OptionsDialogViewModel();
                Assert.Equal(orig, newTarget.ResultsLimit);
            }
        }

        [Fact]
        public void OptionsDialogVmTestOkDoesSave()
        {
            using (var target = new OptionsDialogViewModel())
            {
                var orig = target.ResultsLimit;
                target.ResultsLimit = orig*2;
                target.OkCommand.Execute(null);
                var newTarget = new OptionsDialogViewModel();
                Assert.NotEqual(orig, newTarget.ResultsLimit);
                Assert.Equal(orig*2, newTarget.ResultsLimit);
            }
        }

        [Fact]
        public void OptionsDialogVmTestCanExecuteOk()
        {
            using (var target = new OptionsDialogViewModel())
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
}