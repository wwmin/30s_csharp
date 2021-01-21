<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//自定义TaskScheduler

using System.Collections.Concurrent;

void Main()
{
	$"Main,ThreadID:{Thread.CurrentThread.ManagedThreadId}".Dump();
	List<Task> tasks = new List<Task>();
	//无参执行
	for (int i = 0; i < 5; i++)
	{
	    var t = new Task(() =>
	     {
	         var j = i;
	         $"Task: {j},ThreadID:{Thread.CurrentThread.ManagedThreadId}".Dump();
	     });
	    tasks.Add(t);
	    //    t.Start(MyTaskScheduler.Current);
	}
	foreach (var t in tasks)
	{
	    t.Start(MyTaskScheduler.Current);
	}
	
	//有参执行
	tasks.Clear();
	TaskFactory taskFactory = new TaskFactory();
	List<Task<int>> ts = new List<Task<int>>();
	for (int i = 0; i < 5; i++)
	{
	//    taskFactory.StartNew(new Action(t =>
	//    {
	//        Thread.Sleep(new Random().Next(0, 1000));
	//        $"线程ID为{Thread.CurrentThread.ManagedThreadId},参数为:{t}".Dump();
	//    }), i);
	}
}

//public class CustomTaskScheduler : TaskScheduler
//{

//    private readonly BlockingCollection<Task> tasksCollection = new BlockingCollection<Task>();
//    private readonly Thread mainThread;
//    public CustomTaskScheduler()
//    {

//        mainThread = new Thread(new ThreadStart(Execute));
//        if (!mainThread.IsAlive)
//        {
//            mainThread.Start();

//        }
//    }

//    private void Execute()
//    {
//        $"CustomTaskScheduler,ThreadID:{Thread.CurrentThread.ManagedThreadId}".Dump("CustomTaskScheduler");
//        foreach (var task in tasksCollection.GetConsumingEnumerable())
//        {
//            TryExecuteTask(task);
//        }
//    }

//    protected override IEnumerable<Task>? GetScheduledTasks()
//    {
//        return tasksCollection.ToArray();
//    }

//    protected override void QueueTask(Task task)
//    {
//        if (task != null)
//        {
//            tasksCollection.Add(task);
//        }
//    }

//    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
//    {
//        return false;
//    }
//}


//此示例可在vs中调试


public class MyTaskScheduler : TaskScheduler
{
    public static new TaskScheduler Current { get; } = new MyTaskScheduler();
    public static new TaskScheduler Default { get; } = Current;

    private readonly BlockingCollection<Task> m_queue = new BlockingCollection<Task>();

    public MyTaskScheduler()
    {
        Thread thread = new(Run);
        thread.IsBackground = true;//设为后台线程,当主线程结束时线程自动结束
        thread.Start();
    }



    private void Run()
    {
        Console.WriteLine($"MyTaskSchduler,ThreadID:{Thread.CurrentThread.ManagedThreadId}");
        while (m_queue.TryTake(out var t, Timeout.Infinite))
        {
            TryExecuteTask(t);//在当前线程执行Task
        }
    }
    protected override IEnumerable<Task> GetScheduledTasks()
    {
        return m_queue;
    }

    protected override void QueueTask(Task task)
    {
        m_queue.Add(task);//t.Start(MyTaskScheduler.Current)时，将Task加入到队列中

    }
    //当执行该函数时,程序正在尝试已同步的方式执行Task代码
    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
        return false;
    }
}
