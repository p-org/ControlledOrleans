using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Runtime
{
    interface IGrainCancellationTokenRuntime
    {
        Task Cancel(Guid id, CancellationTokenSource tokenSource, ConcurrentDictionary<GrainId, GrainReference> grainReferences);
    }
}
