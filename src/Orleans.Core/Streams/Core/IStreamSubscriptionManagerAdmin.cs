using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Streams.Core
{
    public interface IStreamSubscriptionManagerAdmin
    {
        IStreamSubscriptionManager GetStreamSubscriptionManager(string managerType);
    }

    public static class StreamSubscriptionManagerType
    {
        public readonly static string ExplicitSubscribeOnly = "ExplicitSubscribeOnly";
    }
}
