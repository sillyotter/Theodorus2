using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Theodorus2.Support;
using Xunit;

namespace UnitTests
{
    public class ValidatableReactiveObjectTests
    {
        private class MyTestClass : ValidatableReactiveObject<MyTestClass>
        {
            public MyTestClass()
            {
                Prop = "ASDF";
                AddValidator(x => x.Prop, () => String.IsNullOrEmpty(Prop) ? "Null" :null);
            }

            private string _prop;

            public string Prop
            {
                get { return _prop; }
                set { this.RaiseAndSetIfChanged(ref _prop, value); }
            }
        }

        [Fact]
        public async Task  InitialTest()
        {
            var x = new MyTestClass();
            Assert.False(x.HasErrors);

            var errors = new List<string>();
            using (Observable.FromEventPattern<EventHandler<DataErrorsChangedEventArgs>, DataErrorsChangedEventArgs>(
                h => x.ErrorsChanged += h, h => x.ErrorsChanged -= h)
                .Select(a => x.GetErrors(a.EventArgs.PropertyName).Cast<string>())
                .SelectMany(a => a)
                .ToCollection(errors))
            {
                x.Prop = null;
                await Task.Delay(10);
                Assert.True(x.HasErrors);
                Assert.Equal(1, errors.Count);
            }

        }
    }
}