using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;

using Nekara.Client; using Nekara.Models;

namespace TestExtensions
{
    public class DefaultClusterFixture : IDisposable, Nekara.Models.Xunit.IAsyncLifetime
    {
        static DefaultClusterFixture()
        {
            TestDefaultConfiguration.InitializeDefaults();
        }

        public DefaultClusterFixture()
        {
            var builder = new TestClusterBuilder();
            TestDefaultConfiguration.ConfigureTestCluster(builder);
            
            builder.AddSiloBuilderConfigurator<SiloHostConfigurator>();

            var testCluster = builder.Build();
            if (testCluster?.Primary == null)
            {
                testCluster?.Deploy();
            }

            this.HostedCluster = testCluster;
            this.Logger = this.Client?.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("Application");
        }
        
        public TestCluster HostedCluster { get; }

        public IGrainFactory GrainFactory => this.HostedCluster?.GrainFactory;

        public IClusterClient Client => this.HostedCluster?.Client;

        public ILogger Logger { get; }

        public virtual void Dispose()
        {
            this.HostedCluster?.StopAllSilos();
        }

        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            var cluster = this.HostedCluster;
            if (cluster != null)
            {
                await cluster.StopAllSilosAsync();
            }
        }

        public class SiloHostConfigurator : ISiloBuilderConfigurator
        {
            public void Configure(ISiloHostBuilder hostBuilder)
            {
                hostBuilder
                    .UseInMemoryReminderService()
                    .AddMemoryGrainStorageAsDefault()
                    .AddMemoryGrainStorage("MemoryStore");
            }
        }
    }
}
