namespace ServiceApi
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Task workItem);

        Task<Task> DequeueAsync(CancellationToken cancellationToken, DateTime timeToRun);
    }
}
