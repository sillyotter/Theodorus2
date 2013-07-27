using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReactiveUI;
using Theodorus2.Support;
using Xunit;

namespace UnitTests
{
    public static class Helpers
    {
        public static IDisposable ToCollection<T>(this IObservable<T> @this, ICollection<T> target)
        {
            return @this.Subscribe(target.Add);
        }
    }

    public class StatusReporterTests
    {
        
        [Fact]
        public void SimpleMessageTest()
        {
            var mb = new MessageBus();
            var dsl = new DefaultStatusListener(mb);
            var dsr = new DefaultStatusReporter(mb);
            var reports = new List<StatusReport>();
            using (dsl.Status.ToCollection(reports)) {
                dsr.ReportProgress("aasd");
                Assert.Equal(1, reports.Count);
                Assert.Null(reports.Single().CurrentStatus);
                Assert.Equal("aasd", reports.Single().Message);
            }
        }

        [Fact]
        public void SimpleProgressTest()
        {
            var mb = new MessageBus();
            var dsl = new DefaultStatusListener(mb);
            var dsr = new DefaultStatusReporter(mb);
            var reports = new List<StatusReport>();
            using (dsl.Status.ToCollection(reports))
            {
                dsr.ReportProgress(40);
                Assert.Equal(1, reports.Count);
                Assert.Null( reports.Single().CurrentStatus);
                Assert.Equal(40, reports.Single().Progress);
            }
        }

        [Fact]
        public void SimpleProgressAndMessageTest()
        {
            var mb = new MessageBus();
            var dsl = new DefaultStatusListener(mb);
            var dsr = new DefaultStatusReporter(mb);
            var reports = new List<StatusReport>();
            using (dsl.Status.ToCollection(reports))
            {
                dsr.ReportProgress("ASDF", 40);
                Assert.Equal(1, reports.Count);
                Assert.Null( reports.Single().CurrentStatus);
                Assert.Equal(40, reports.Single().Progress);
                Assert.Equal("ASDF", reports.Single().Message);
            }
        }

        [Fact]
        public void MonitoredWorkTest()
        {
            var mb = new MessageBus();
            var dsl = new DefaultStatusListener(mb);
            var dsr = new DefaultStatusReporter(mb);
            var reports = new List<StatusReport>();
            using (dsl.Status.ToCollection(reports))
            {
                using (dsr.BeginMonitoredWork("ASDF"))
                {
                    Assert.Equal(1, reports.Count);
                    Assert.Equal("ASDF", reports.First().Message);      
                    Assert.Equal(0, reports.First().Progress);
                    Assert.Equal(ExecutionStatus.RunningMonitored, reports.First().CurrentStatus);
                }
                Assert.Equal(2, reports.Count);
                Assert.Equal("", reports.Last().Message);      
                Assert.Equal(0, reports.Last().Progress);
                Assert.Equal(ExecutionStatus.NotRunning, reports.Last().CurrentStatus);
            }
        }

        [Fact]
        public void MUnonitoredWorkTest()
        {
            var mb = new MessageBus();
            var dsl = new DefaultStatusListener(mb);
            var dsr = new DefaultStatusReporter(mb);
            var reports = new List<StatusReport>();
            using (dsl.Status.ToCollection(reports))
            {
                using (dsr.BeginIndeterminateWork("ASDF"))
                {
                    Assert.Equal(1, reports.Count);
                    Assert.Equal("ASDF", reports.First().Message);
                    Assert.Equal(0, reports.First().Progress);
                    Assert.Equal(ExecutionStatus.RunningIndeterminate, reports.First().CurrentStatus);
                }
                Assert.Equal(2, reports.Count);
                Assert.Equal("", reports.Last().Message);
                Assert.Equal(0, reports.Last().Progress);
                Assert.Equal(ExecutionStatus.NotRunning, reports.Last().CurrentStatus);
            }
        }

       
    }
}