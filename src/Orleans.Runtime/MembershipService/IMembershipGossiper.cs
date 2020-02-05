using System.Collections.Generic;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Runtime.MembershipService
{
    internal interface IMembershipGossiper
    {
        Task GossipToRemoteSilos(
            List<SiloAddress> gossipPartners,
            MembershipTableSnapshot snapshot,
            SiloAddress updatedSilo,
            SiloStatus updatedStatus);
    }
}
