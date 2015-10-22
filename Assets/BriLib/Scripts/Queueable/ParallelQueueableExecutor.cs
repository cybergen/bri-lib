using System;
using System.Collections.Generic;

public class ParallelQueueableExecutor : IQueueable
{
    public Action<IQueueable> OnBegan { get; set; }
    public Action<IQueueable> OnEnded { get; set; }
    public Action<IQueueable> OnKilled { get; set; }

    public void Begin()
    {
        throw new NotImplementedException();
    }

    public void Kill()
    {
        throw new NotImplementedException();
    }
}