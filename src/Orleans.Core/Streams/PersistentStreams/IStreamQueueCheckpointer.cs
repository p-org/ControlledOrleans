﻿using System;
using Nekara.Client; using Nekara.Models; 

namespace Orleans.Streams
{
    public interface IStreamQueueCheckpointerFactory
    {
        Task<IStreamQueueCheckpointer<string>> Create(string partition);
    }
    
    public interface IStreamQueueCheckpointer<TCheckpoint>
    {
        bool CheckpointExists { get; }
        Task<TCheckpoint> Load();
        void Update(TCheckpoint offset, DateTime utcNow);
    }
}
