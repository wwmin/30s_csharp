<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//PeriodicTimer
//完全异步的“PeriodicTimer”, 更适合在异步场景中使用, 它有一个方法 WaitForNextTickAsync。
async Task Main()
{
	await TestPeriodicTimer();
}

#region PeriodicTimer
//await TestPeriodicTimer().ConfigureAwait(false);
async Task TestPeriodicTimer()
{
	CancellationTokenSource cts = new CancellationTokenSource();
	//超时取消
	cts.CancelAfter(TimeSpan.FromSeconds(3));
	CancellationToken cancellationToken = cts.Token;

	using (PeriodicTimer timer = new(TimeSpan.FromSeconds(1)))
	{
		try
		{
			while (await timer.WaitForNextTickAsync(cancellationToken))
			{
				Console.WriteLine(DateTime.Now);
			}
		}
		catch (Exception)
		{
			Console.WriteLine("task periodicTimer is canceled");
		}
	}
}
#endregion