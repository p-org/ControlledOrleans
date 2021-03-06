using System;
using System.Threading;
using Nekara.Client; using Nekara.Models; 
using Microsoft.Extensions.Logging;

namespace Orleans.Runtime
{
    internal class SiloOptionsLogger : OptionsLogger, ILifecycleParticipant<ISiloLifecycle>
    {
        public SiloOptionsLogger(ILogger<SiloOptionsLogger> logger, IServiceProvider services)
            : base(logger, services)
        {
        }

        public void Participate(ISiloLifecycle lifecycle)
        {
            lifecycle.Subscribe<SiloOptionsLogger>(ServiceLifecycleStage.First, this.OnStart);
        }

        public Task OnStart(CancellationToken token)
        {
            this.LogOptions();
            return Task.CompletedTask;
        }
    }
}
