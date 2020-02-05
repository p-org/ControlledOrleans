using System;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Runtime
{
    internal interface IGrainTimer : IDisposable
    {
        void Start();

        void Stop();

        Task GetCurrentlyExecutingTickTask();
    }
}