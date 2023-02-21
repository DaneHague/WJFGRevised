using System.Collections.Concurrent;

namespace ServiceApi
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<Task> _workItems = new ConcurrentQueue<Task>();

        private SemaphoreSlim _signal = new SemaphoreSlim(0);
        // Not finished, supposed to be used to keep track of when tasks are ready
        List<TstTask> tstTasks = new List<TstTask>();

        public void QueueBackgroundWorkItem(Task workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            TstTask t = new TstTask();
            t.task = workItem;

            tstTasks.Add(t);
            // Not finished, used to check all the tasks
            Console.WriteLine(tstTasks.Count);
            _workItems.Enqueue(workItem);
            _signal.Release();
            
            
        }
        
        public async Task<Task> DequeueAsync(CancellationToken cancellationToken, DateTime timeToRun)
        {
            await _signal.WaitAsync(cancellationToken);
            
            _workItems.TryPeek(out Task workItem);

            workItem.Start();
            _workItems.TryDequeue(out Task tst);

            return workItem;
        }
    }
}
