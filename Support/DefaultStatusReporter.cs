using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class DefaultStatusReporter : IStatusReporter
    {
        private readonly IMessageBus _bus;

        public DefaultStatusReporter(IMessageBus bus)
        {
            _bus = bus;
        }

        public IDisposable BeginMonitoredWork(string message)
        {
            _bus.SendMessage(new StatusReport(message, 0, ExecutionStatus.RunningMonitored), StatusReport.Channel);
            return Disposable.Create(
                () => _bus.SendMessage(
                    new StatusReport("", 0, ExecutionStatus.NotRunning), StatusReport.Channel)
                );
        }

        public IDisposable BeginIndeterminateWork(string message)
        {
            _bus.SendMessage(new StatusReport(message, 0, ExecutionStatus.RunningIndeterminate), "Progress");
            return Disposable.Create(
                () => _bus.SendMessage(
                    new StatusReport("", 0, ExecutionStatus.NotRunning), "Progress")
                );
        }

        public void ReportProgress(string message)
        {
            _bus.SendMessage(new StatusReport(message, null, null), StatusReport.Channel);
        }

        public void ReportProgress(int progress)
        {
            _bus.SendMessage(new StatusReport(null, progress, null), StatusReport.Channel);
        }

        public void ReportProgress(string message, int progress)
        {
            _bus.SendMessage(new StatusReport(message, progress, null), StatusReport.Channel);
        }
    }
}