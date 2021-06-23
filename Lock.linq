<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	{
		//关键字lock的使用,lock块内应尽量短,防止锁竞争
		var t1 = Task.Run(() => add("t1"));
		var t2 = Task.Run(() => decrease("t2"));
		var t3 = Task.Run(() => decrease("t3"));
		Task.WaitAll(t1, t2, t3);
		num.Dump();
	}
	{
		//使用线程安全的Interlocked类达到对数字double的增减等操作
		int n = 200;
		var t1 = Task.Run(() => interlockedAdd(ref n, "t1"));
		var t2 = Task.Run(() => interlockedDecrement(ref n, "t2"));
		var t3 = Task.Run(() => interlockedDecrement(ref n, "t3"));
		Task.WaitAll(t1,t2,t3);
		n.Dump();
	}
}
private readonly object lockObj = new Object();
private int num = 0;
private int add(string name)
{
	lock (lockObj)
	{
		num++;
	}
	num.Dump(name);
	return num;
}

private int decrease(string name)
{
	lock (lockObj)
	{
		num--;
	}
	num.Dump(name);
	return num;
}

private void interlockedAdd(ref int num, string name)
{
	Interlocked.Increment(ref num);
	num.Dump(name);
}
private void interlockedDecrement(ref int num, string name)
{
	Interlocked.Decrement(ref num);
	num.Dump(name);
}
// You can define other methods, fields, classes and namespaces here
