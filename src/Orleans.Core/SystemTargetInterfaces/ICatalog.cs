using System.Collections.Generic;
using Nekara.Client; using Nekara.Models; 


namespace Orleans.Runtime
{
    /// <summary>
    /// Remote interface to grain and activation state
    /// </summary>
    internal interface ICatalog : ISystemTarget
    {
        /// <summary>
        /// Delete activations from this silo
        /// </summary>
        /// <param name="activationAddresses"></param>
        /// <returns></returns>
        Task DeleteActivations(List<ActivationAddress> activationAddresses);
    }
}
