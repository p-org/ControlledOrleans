using System;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Timers
{
    public interface ITimerRegistry
    {
        IDisposable RegisterTimer(Grain grain, Func<object, Task> asyncCallback, object state, TimeSpan dueTime, TimeSpan period);
    }
}