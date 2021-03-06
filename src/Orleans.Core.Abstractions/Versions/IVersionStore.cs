using System.Collections.Generic;
using Nekara.Client; using Nekara.Models; 
using Orleans.Versions.Compatibility;
using Orleans.Versions.Selector;

namespace Orleans.Versions
{
    public interface IVersionStore : IVersionManager
    {
        bool IsEnabled { get; }
        Task<Dictionary<int, CompatibilityStrategy>> GetCompatibilityStrategies();
        Task<Dictionary<int, VersionSelectorStrategy>> GetSelectorStrategies();
        Task<CompatibilityStrategy> GetCompatibilityStrategy();
        Task<VersionSelectorStrategy> GetSelectorStrategy();
    }
}