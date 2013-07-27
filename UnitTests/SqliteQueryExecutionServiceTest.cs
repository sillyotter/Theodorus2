using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using Theodorus2.Support;
using Xunit;

namespace UnitTests
{
    public class SqliteQueryExecutionServiceTest
    {
        [Fact]
        public void TestExecuteNoConStr()
        {
            var mb = new MessageBus();
            var sr = new DefaultStatusReporter(mb);
            var target = new SqliteQueryExecutionService(sr);
            Assert.Throws<InvalidOperationException>(() => target.Execute("select * from sqlite_master"));
        }

        [Fact]
        public void TestExecuteWithConStr()
        {
            var mb = new MessageBus();
            var sr = new DefaultStatusReporter(mb);
            var target = new SqliteQueryExecutionService(sr) {ConnectionString = "Data Source=:memory:"};
            Assert.DoesNotThrow(() => target.Execute("select * from sqlite_master"));
        }

        [Fact]
        public async Task TestExecuteWithQueryConfirmWeGetAnswer()
        {
            var mb = new MessageBus();
            var sr = new DefaultStatusReporter(mb);
            var target = new SqliteQueryExecutionService(sr) {ConnectionString = "Data Source=:memory:"};
            var res = await target.Execute("select 1");
            Assert.Equal(1, res.Count());
            Assert.Equal("select 1", res.Single().Query);
            Assert.Equal(1, res.Single().Results.Tables.Count);
        }

        [Fact]
        public async Task TestExecuteWithQueryCaptureProgress()
        {
            var mb = new MessageBus();
            var sl = new DefaultStatusListener(mb);
            var sr = new DefaultStatusReporter(mb);
            var target = new SqliteQueryExecutionService(sr) { ConnectionString = "Data Source=:memory:" };
            var pr = new List<StatusReport>();
            using (sl.Status.ToCollection(pr))
            {
                var res = await target.Execute("select 1");
                Assert.True(res.Any());
            }
        }

    }
}