using System;
using System.Collections.Generic;

public class SerialQueueableExecutor : IQueueable
{
    public Action<IQueueable> OnBegan { get; set; }
    public Action<IQueueable> OnEnded { get; set; }
    public Action<IQueueable> OnKilled { get; set; }

    private Queue<IQueueable> _queue = new Queue<IQueueable>();

    public SerialQueueableExecutor(IEnumerable<IQueueable> queue)
    {
        foreach (var entry in queue) { Queue(entry); }
    }

    public SerialQueueableExecutor(IQueueable[] queue)
    {
        foreach (var entry in queue) { Queue(entry); }
    }

    public SerialQueueableExecutor(IQueueable entry)
    {
        Queue(entry);
    }

    public SerialQueueableExecutor() { }

    public void Queue(IQueueable queueable)
    {
        _queue.Enqueue(queueable);
    }

    public void Begin()
    {
        throw new NotImplementedException();
    }

    public void Kill()
    {
        throw new NotImplementedException();
    }
}
