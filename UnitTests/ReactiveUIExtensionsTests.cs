using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using Theodorus2.Support;
using Xunit;

namespace UnitTests
{
    public class ReactiveUIExtensionsTests
    {
        [Fact]
        public async Task TestReactiveCommandExecuteCompleted()
        {
            var count = 0;

            var cmd = new ReactiveCommand(Observable.Return(true));
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(200);
                Interlocked.Increment(ref count);
            });
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(200);
                Interlocked.Increment(ref count);
            });
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(200);
                Interlocked.Increment(ref count);
            });

            var compSub = 0;
            var sub = cmd.ExecuteCompleted().Subscribe(_ =>
            {
                Interlocked.Increment(ref compSub);
                Assert.Equal(3, count);
            });

            await cmd.ExecuteAsync();
            await Task.Yield();
            Assert.Equal(1, compSub);
            sub.Dispose();
        }

        [Fact]
        public async Task TestReactiveCommandExecuteAsyncLong()
        {
            var count = 0;

            var cmd = new ReactiveCommand(Observable.Return(true));
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(1000);
                Interlocked.Increment(ref count);
            });

            var sw = Stopwatch.StartNew();
            await cmd.ExecuteAsync();
            Assert.Equal(1, count);

            sw.Stop();
            Assert.InRange(sw.ElapsedMilliseconds, 950, 1100);
        }

        [Fact]
        public async Task TestReactiveCommandExecuteAsyncShort()
        {
            var count = 0;

            var cmd = new ReactiveCommand(Observable.Return(true));
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(0);
                Interlocked.Increment(ref count);
            });

            var sw = Stopwatch.StartNew();
            await cmd.ExecuteAsync();
            Assert.Equal(1, count);
            sw.Stop();
            Assert.InRange( sw.ElapsedMilliseconds, 0, 60);
        }

        [Fact]
        public async Task TestReactiveCommandExecuteAsyncMultiWithNonAsync()
        {
            var cmd = new ReactiveCommand(Observable.Return(true));
            var count = 0;
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(0);
                Interlocked.Increment(ref count);
            });
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(1000);
                Interlocked.Increment(ref count);
            });
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(600);
                Interlocked.Increment(ref count);
            });
            cmd.RegisterAsyncAction(_ =>
            {
                Task.Delay(500).Wait();
                Interlocked.Increment(ref count);
            });

            // the above tasks are all run in parallel, so it should take just around the longest time to finish, 1000

            var sw = Stopwatch.StartNew();
            await cmd.ExecuteAsync();
            Assert.Equal(4, count);
            sw.Stop();
            Assert.InRange(sw.ElapsedMilliseconds, 950, 1500);
        }

        [Fact]
        public async Task TestExecuteAsyncWithStandardSubs()
        {
            var cmd = new ReactiveCommand(Observable.Return(true));
            var count = 0;
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(500);
                Interlocked.Increment(ref count);
            });
            cmd.RegisterAsyncTask(async _ =>
            {
                await Task.Delay(1000);
                Interlocked.Increment(ref count);
            });
            cmd.Subscribe(x =>
            {
                Task.Delay(500).Wait();
                Interlocked.Increment(ref count);
            });

            cmd.Subscribe(x =>
            {
                Task.Delay(500).Wait();
                Interlocked.Increment(ref count);
            });

            cmd.Subscribe(x =>
            {
                Task.Delay(500).Wait();
                Interlocked.Increment(ref count);
            });

            // so, al of the async tasks are run in parallell, up to some limit I guess
            // meanign that ifI have a 1000 delay and a 500, it only takes 1000ms to run both of them
            // at the same time, resulting in a delay of 1000.
            // If ive done standard subscribes, they are as a group, one after the other, run, but in parallell with
            // the async ones.  So, in this case, i have a async that takes 1000, but 3 500ms standard ones, that take 1500, 
            // so the duration will be 1500.

            var sw = Stopwatch.StartNew();
            await cmd.ExecuteAsync();
            Assert.Equal(5, count);
            sw.Stop();
            Assert.InRange(sw.ElapsedMilliseconds, 950, 2000);
        }


        [Fact]
        public async Task CanExecuteTests()
        {
            var x = 0;

            var sub = new Subject<bool>();
            var cmd = new ReactiveCommand(sub);

            cmd.RegisterAsyncTask(async o =>
            {
                await Task.Delay(100);
                Interlocked.Increment(ref x);
            });
            // by default, canexecute is true
            await cmd.ExecuteAsync();
            Assert.Equal(1, x);

            sub.OnNext(false);
            Assert.Throws<InvalidOperationException>( () =>  cmd.ExecuteAsync().Wait());
            Assert.Equal(1, x);

            sub.OnNext(true);
            await cmd.ExecuteAsync();
            Assert.Equal(2, x);
        }

        [Fact]
        public async Task CanExecuteTestsInitialCondition()
        {
            var x = 0;

            var sub = new Subject<bool>();
            var cmd = ReactiveUIExtensions.CreateWithInitialCondition(sub, false);

            cmd.RegisterAsyncTask(async o =>
            {
                await Task.Delay(100);
                Interlocked.Increment(ref x);
            });

            Assert.Throws<InvalidOperationException>( () =>  cmd.ExecuteAsync().Wait());
            Assert.Equal(0, x);
            sub.OnNext(true);
            await cmd.ExecuteAsync();
            Assert.Equal(1, x);
        }

    }
}