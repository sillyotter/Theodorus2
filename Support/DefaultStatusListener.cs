using System;
using ReactiveUI;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class DefaultStatusListener : IStatusListener
    {
        public DefaultStatusListener(IMessageBus bus)
        {
            Status = bus.Listen<StatusReport>(StatusReport.Channel);
        }

        public IObservable<StatusReport> Status { get; }
    }
}