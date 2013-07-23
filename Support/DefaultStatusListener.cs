using System;
using ReactiveUI;
using Theodorus2.Interfaces;

namespace Theodorus2.Support
{
    public class DefaultStatusListener : IStatusListener
    {
        private readonly IObservable<StatusReport> _receiver;

        public DefaultStatusListener(IMessageBus bus)
        {
            _receiver = bus.Listen<StatusReport>(StatusReport.Channel);
        }

        public IObservable<StatusReport> Status { get { return _receiver; } }
    }
}