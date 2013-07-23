using System;
using Theodorus2.Support;

namespace Theodorus2.Interfaces
{
    public interface IStatusListener
    {
        IObservable<StatusReport> Status { get; }
    }
}