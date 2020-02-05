using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Streams.Core
{
    public interface IStreamSubscriptionObserver
    {
        Task OnSubscribed(IStreamSubscriptionHandleFactory handleFactory);
    }
}
