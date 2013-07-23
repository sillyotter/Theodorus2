using System;

namespace Theodorus2.Interfaces
{
    interface IStatusReporter
    {
        IDisposable BeginMonitoredWork(string message);
        IDisposable BeginIndeterminateWork(string message);
        void ReportProgress(string message);
        void ReportProgress(int progress);
        void ReportProgress(string message, int progress);
    }
}
