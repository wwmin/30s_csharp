<Query Kind="Expression">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>


static Thread thread1, thread2;
thread1 = new Thread(ThreadProc);
thread1.Name = "Thread1";
thread1.Start();

thread2 = new Thread(ThreadProc);
thread2.Name = "Thread2";
thread2.Start();


private static void ThreadProc()
{
    Console.WriteLine($"current thread: {Thread.CurrentThread.Name}");
    if (Thread.CurrentThread.Name == "Thread1" && thread2.ThreadState != ThreadState.Unstarted)
        if(thread2.Join(2000))
            "Thread2 has terminated.".Dump();
         else 
            "The timeout has elapsed and Thread1 will resume.".Dump();

    Thread.Sleep(4000);
    $"current thread: {Thread.CurrentThread.Name}".Dump();
    $"Thread1: {thread1.ThreadState}".Dump();
    $"Thread2: {thread2.ThreadState}".Dump();
}