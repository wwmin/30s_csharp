<Query Kind="Program">
  <Namespace>System.Collections.Concurrent</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	//生成随机数却得到相同的随机数,原因是"伪随机"和"线程不安全"
	#region 系统默认种子
	{

		//以当前系统时间作为默认种子构建一个随机序列
		Random r1 = new Random();
	}
	#endregion
	#region 自定义种子
	{
		//自定义一个种子,通常会使用时间Ticks
		var seed = unchecked((int)DateTime.Now.Ticks);
		Random rs1 = new Random(seed);
		Random rs2 = new Random(seed);
		//下面两个随机数是相同的
		rs1.Next().Dump();
		rs2.Next().Dump();
	}
	#endregion
	#region 伪随机
	{
		Action Bad_Random =() =>
			  {
				  for (int i = 0; i < 100; i++)
				  {
					  var r = new Random();
					  var val = r.Next(1, 100);
					  val.Dump();
				  }
			  };
		//Bad_Random();
		//这种随机数已经在.net core中得到优化,得到的是随机数
	}
	{
		GoodRandomInSingleThread();
		GoodRandomInMultThreads();
	}
	#endregion
}

private static string GenerateRandomStr(Random random)
{
	string source = "ABCDEFGHIKLMNOPQRTUVWXYZabcdefghiklmnopqrtuvwxyz";
	var list = Enumerable.Repeat(source, 100).Select(s => s[random.Next(s.Length)]).ToArray();
	return new string(list);
}

public static ConcurrentBag<string> GoodRandomInSingleThread()
{
	var r = new Random();
	ConcurrentBag<string> list = new ConcurrentBag<string>();
	for (int i = 0; i < 20000; i++)
	{
		var val = GenerateRandomStr(r);
		list.Add(val);
	}
	return list;
}

//使用ThreadLocal这个类可以保证它所包含的对象只能线程内独享
//Interlocked 更简单的锁lock关键字的实现
private static int seed = 100;
private readonly static object lockObj = new object();
private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>(() =>
{
	int num = 0;
	//int num = Interlocked.Increment(ref seed);
	lock (lockObj)
	{
		num = seed + 1;
	}
	num.Dump("线程安全的num,每个线程都会执行一次");
	return new Random(num);
});

public static void GoodRandomInMultThreads()
{
	ConcurrentBag<string> list = new ConcurrentBag<string>();
	var t1 = Task.Run(() =>
	{
		for (int i = 0; i < 10000; i++)
		{

			var val = GenerateRandomStr(threadLocal.Value);
			list.Add(val);
		}
	});
	var t2 = Task.Run(() =>
	{
		for (int i = 0; i < 10000; i++)
		{
			var val = GenerateRandomStr(threadLocal.Value);
			list.Add(val);
		}
	});

	Task.WaitAll(t1, t2);
	$"[ThreadLocal模式]线程1和线程2的重复数据有：{20000 - list.Distinct().Count()}".Dump();
}