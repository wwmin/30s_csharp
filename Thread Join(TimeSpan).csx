TimeSpan waitTime = new TimeSpan(0,0,1);

Thread newThread = new Thread(Work);
newThread.Start();
//调用join 就可以等待另一个线程结束, 可带毫秒或者TimeSpan 超时
if(newThread.Join(waitTime + waitTime))
    "new thread terminated.".Dump();
else 
    "join timed out.".Dump();

void Work()
{
    //当等待Sleep或Join的时候,线程处于阻塞的状态
    Thread.Sleep(1000);//暂停当前的线程,并等一段时间
    Thread.Sleep(0);//这样调用会导致线程立即放弃本身当前的时间片,自动将CPU移交给其他线程
    Thread.Yield();//同Sleep(0),但是它只会把执行交给同一处理器上的其他线程
}