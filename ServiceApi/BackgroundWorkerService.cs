using ServiceApi;
using System.Threading;

// Reference https://www.youtube.com/watch?v=8Sy69b6-nj0
public class BackgroundWorkerService : BackgroundService
{
    private Task _backgroundTask;
    readonly ILogger<BackgroundWorkerService> _logger;


    // DI for the logger and background queue
    public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IBackgroundTaskQueue backgroundTaskQueue)
    {
        _logger = logger;
        _backgroundQueue = backgroundTaskQueue;
    }

    public IBackgroundTaskQueue _backgroundQueue { get; }


    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            static void ThreadProc1()
            {
                // Get the current Thread and show the ID
                var th = Thread.CurrentThread;
            }

            Action tstAction = () =>
            {
                Thread thread1 = new Thread(new ThreadStart(ThreadProc1));
                Console.WriteLine("Managed Thread #{0}", thread1.ManagedThreadId);
            };

            Task t1 = new Task(tstAction);

            _backgroundQueue.QueueBackgroundWorkItem(t1);

            // just to stop it spamming
            await Task.Delay(1000, stoppingToken);

            _backgroundQueue.DequeueAsync(stoppingToken, DateTime.Now);

        }
    }
}