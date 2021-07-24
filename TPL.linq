<Query Kind="Program">
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	Environment.ProcessorCount.Dump("CPU 核心数");

	Stopwatch sw = new Stopwatch();
	sw.Start();
	ShareMemory();
	sw.Stop();
	$"共享内存并发模型耗时：{sw.Elapsed}".Dump("耗时");
	sw.Reset();
	sw.Start();
	SingleRun();
	sw.Stop();
	$"{sw.Elapsed}".Dump("单线程循环相加耗时");
	sw.Reset();
	sw.Start();
	SingleMemory();
	sw.Stop();
	$"{sw.Elapsed}".Dump("多线程循环相加耗时");
}
//1. 数据并行
private void ShareMemory()
{
	var sum = 0;
	Parallel.For(1, 100_000 + 1,() => 0, (x, state, local) =>
	  {
		  var f = true;
		  if (x == 1) f = false;
		  for (int i = 2; i <= x / 2; i++)
		  {
			  if (x % i == 0) f = false;
		  }
		  if (f == true) local++;
		  return local;
	  }, local =>
	  {
		  Interlocked.Add(ref sum, local);
	  });
	$"1-100_000内质数的个数是{sum}".Dump("并行运行结果");
}
//参数1,2 表示数据并行要操作的对象；
//参数3localInit表示某线程内迭代的初始值，将会作为参数4body委托的第3个参数，只在线程第一次使用;
//参数4body表示每个迭代都需要经历的执行体, 这里以线程为单元处理迭代；
//参数5localFinally对每个线程的输出再做一次计算，入参是参数4的输出
private void SingleRun() {
	double sum = 0;
	for (int i = 0; i < 10000; i++)
	{
		for (int j = 0; j < 60000; j++)
		{
			sum+=i;
		}
	}
	sum.Dump("单线程sum结果");
}
private void SingleMemory()
{
	
	Parallel.For(0, 10000, item =>
	{
		for (int j = 0; j < 60000; j++)
		{
			double sum = 0;
			sum += item;
		}
	});
}

//2. 任务并行
//void System.Threading.Tasks.Parallel.Invoke(WatchMovieHaveDinner,ReadBook,WriteBlog);
//这段代码会创建指向每一个方法的委托。
//没有特定的执行顺序 Parallel.Invoke方法只有在4个方法全部完成之后才会返回。它至少需要4个硬件线程才足以让这4个方法并发运行。
//但并不保证这4个方法能够同时启动运行，如果一个或者多个内核处于繁忙状态，那么底层的调度逻辑可能会延迟某些方法的初始化执行。

//捕捉并行循环中发生的异常
//System.AggregateException