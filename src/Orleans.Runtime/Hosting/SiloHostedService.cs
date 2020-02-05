using System;
using System.Collections.Generic;
using System.Threading;
using Nekara.Client;
using Nekara.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Orleans.Hosting
{
    internal class SiloHostedService  : IHostedService
    {
        private readonly ILogger logger;
        private readonly ISiloHost siloHost;

        public SiloHostedService(
            ISiloHost siloHost,
            IEnumerable<IConfigurationValidator> configurationValidators,
            ILogger<SiloHostedService> logger)
        {
            this.ValidateSystemConfiguration(configurationValidators);
            this.siloHost = siloHost;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Starting Orleans Silo.");
            await this.siloHost.StartAsync(cancellationToken).ConfigureAwait(false);
            this.logger.LogInformation("Orleans Silo started.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Stopping Orleans Silo");
            await this.siloHost.StopAsync(cancellationToken).ConfigureAwait(false);
            this.logger.LogInformation("Orleans Silo stopped.");
        }

        System.Threading.Tasks.Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        System.Threading.Tasks.Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private void ValidateSystemConfiguration(IEnumerable<IConfigurationValidator> configurationValidators)
        {
            foreach (var validator in configurationValidators)
            {
                validator.ValidateConfiguration();
            }
        }
    }
}