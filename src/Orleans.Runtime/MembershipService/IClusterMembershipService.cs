using System.Collections.Generic;
using Nekara.Client;
using Nekara.Models; 

namespace Orleans.Runtime
{
    internal interface IClusterMembershipService
    {
        ClusterMembershipSnapshot CurrentSnapshot { get; }

        IAsyncEnumerable<ClusterMembershipSnapshot> MembershipUpdates { get; }

        System.Threading.Tasks.ValueTask Refresh(MembershipVersion minimumVersion = default);
    }
}
