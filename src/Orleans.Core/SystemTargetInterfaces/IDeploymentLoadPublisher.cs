using Nekara.Client; using Nekara.Models; 

namespace Orleans.Runtime
{
    internal interface IDeploymentLoadPublisher : ISystemTarget
    {
        Task UpdateRuntimeStatistics(SiloAddress siloAddress, SiloRuntimeStatistics siloStats);
    }
}
