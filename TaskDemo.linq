<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

async Task Main()
{
	{
		static async Task Run()
		{
			await Task.Run(() => "hello".Dump());
		}

		await Run();
	}
	{
		await TestTaskMore();
	}
	{
		Task<int> task = Task.Factory.StartNew(() =>
		{
			Thread.Sleep(100);
			"Foo".Dump();
			return 1;
		}, TaskCreationOptions.LongRunning);
		var res = await task;
		res.Dump();
	}
	Task<int> GetPrimeNumber()
	{
		Task<int> primeNumberTask = Task.Run(() => Enumerable.Range(2, 100).Count(n => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
		return primeNumberTask;
	}
	{

		"Task running ...".Dump();
		$"Then answer is {GetPrimeNumber().Result}".Dump();
	}
	{

		Task task = Task.Run(() => { throw null!; });
		try
		{
			task.Wait();
		}
		catch (AggregateException aex)
		{
			if (aex.InnerException is NullReferenceException)
			{
				"Null".Dump();
			}
			else
			{
				throw;
			}
		}
	}
	{
		var i = await GetPrimeNumber().ContinueWith<int>(task =>
		 {
			 int result = task.Result;
			 result.Dump();
			 return ++result;
		 }).ContinueWith<object>(task =>
		 {
			 var result = task.Result;
			 result.Dump();
			 return ++result!;
		 });
		i.Dump();
	}
	{
		//TaskCompletionSource 让你在稍后开始和结束的任意操作中创建Task
		var tcs = new TaskCompletionSource<int>();
		new Thread(() =>
		{
			Thread.Sleep(100);
			tcs.SetResult(11);
		})
		{
			IsBackground = true
		}.Start();

		Task<int> task = tcs.Task;
		task.Result.Dump();

		//实现类似Task.Run 方法
		Task<TResult> Run<TResult>(Func<TResult> function)
		{
			var tcs = new TaskCompletionSource<TResult>();
			new Thread(() =>
			{
				try
				{
					tcs.SetResult(function());
				}
				catch (Exception ex)
				{
					tcs.SetException(ex);
				}

			}).Start();
			return tcs.Task;
		}
		var i = await Run<int>(() =>
		{
			Thread.Sleep(100);
			return 1;
		});
		i.Dump();

		//Timer task
		Task<int> GetAnswerToLife()
		{
			var tcs = new TaskCompletionSource<int>();
			var timer = new System.Timers.Timer(1000) { AutoReset = false };
			timer.Elapsed += delegate { timer.Dispose(); tcs.SetResult(42); };
			timer.Start();
			return tcs.Task;
		}
		var awaiter = GetAnswerToLife().GetAwaiter();
		awaiter.OnCompleted(() =>
		{
			awaiter.GetResult().Dump();
		});
		await Task.Delay(3000);//等待3000 让awaiter内容执行并输出

		//继续封装成Delay
		Task Delay(int milliseconds)
		{
			var tcs = new TaskCompletionSource<object>();
			var timer = new System.Timers.Timer(milliseconds) { AutoReset = false };
			timer.Elapsed += delegate
			{
				timer.Dispose();
				tcs.SetResult(null!);
			};
			timer.Start();
			return tcs.Task;
		}


		Delay(500).GetAwaiter().OnCompleted(() => 111.Dump());
		await Task.Delay(1000);//Task.Delay 相当于异步版本的Thread.Sleep
	}
	{
		Func<Task> unnamed = async () =>
		{
			await Task.Delay(200);
			"unnamed".Dump();
		};
		await unnamed();
	}

	{
		Task Foo(IProgress<int> onProgressPercentChanged)
		{

			return Task.Run(() =>
			{
				for (int i = 0; i < 10; i++)
				{
					if (i % 2 == 0) onProgressPercentChanged.Report(i / 2);
					Task.Delay(100);
				}
			});
		}

		var progress = new Progress<int>(i => $"{i:00}".Dump());
		await Foo(progress);
	}




	{

		static async Task<TResult> WithTimeout<TResult>(Task<TResult> task, TimeSpan timeout)//可加this 做成类扩展
		{
			var cancelSource = new CancellationTokenSource();
			var delay = Task.Delay(timeout, cancelSource.Token);
			Task winner = await Task.WhenAny(task, delay).ConfigureAwait(false);
			if (winner == task)
				cancelSource.Cancel();
			else
				throw new TimeoutException();
			return await task.ConfigureAwait(false);//unwrap result/re-throw;
		}

		static async Task<TResult> WithCancellation<TResult>(Task<TResult> task, CancellationToken cancelToken)//可加this 做成类扩展
		{
			var tcs = new TaskCompletionSource<TResult>();
			var reg = cancelToken.Register(() => tcs.TrySetCanceled());
			await task.ContinueWith(ant =>
			 {
				 reg.Dispose();
				 if (ant.IsCanceled)
					 tcs.TrySetCanceled();
				 else if (ant.IsFaulted)
					 tcs.TrySetException(ant.Exception?.InnerException!);
				 else
					 tcs.TrySetResult(ant.Result);
			 });
			return await tcs.Task;
		}

		CancellationTokenSource s_cts = new CancellationTokenSource();
		var task = Task.Run<int>(async () =>
				{
					await Task.Delay(1000);
					s_cts.Cancel();
					return 11;
				});

		var i = await WithTimeout<int>(task, TimeSpan.FromSeconds(3));
		i.Dump("WithTimeout i");
		try
		{
			var j = await WithCancellation<int>(task, s_cts.Token);
			j.Dump("WithTimeout j");
		}
		catch (Exception ex)
		{
			ex.Message.Dump();
		}

	}


	//task 执行方式
	{
		List<Task> tasks = new List<Task>();
		TaskFactory taskFactory = new TaskFactory();
		for (int i = 0; i < 5; i++)
		{
			tasks.Add(taskFactory.StartNew(new Action(() =>
		   {
			   Thread.Sleep(new Random().Next(0, 1000));
			   $"方式1，线程ID为{Thread.CurrentThread.ManagedThreadId}".Dump();
		   })));
		}
		Task.WaitAny(tasks.ToArray());//同步阻塞方式
		Console.WriteLine("task wait any");
		await Task.WhenAll(tasks);
	}
	{
		List<Task> tasks = new List<Task>();
		TaskFactory taskFactory = new TaskFactory();
		for (int i = 0; i < 5; i++)
		{
			tasks.Add(taskFactory.StartNew(new Action<object?>(t =>
			{
				Thread.Sleep(new Random().Next(0, 1000));
				string value = GetValue((int)t!);
				$"方式2，线程ID为{Thread.CurrentThread.ManagedThreadId:00},参数为:{t:00}|{value:00}".Dump();
			}), i));
		}
		await Task.WhenAll(tasks);
	}
	{
		List<Task> tasks = new List<Task>();
		for (int i = 0; i < 5; i++)
		{
			tasks.Add(Task.Run(new Action(() =>
			{
				Thread.Sleep(new Random().Next(0, 1000));
				$"方式3，线程ID为{Thread.CurrentThread.ManagedThreadId}".Dump();
			})));
		}
		await Task.WhenAll(tasks);
	}

	string GetValue(int i)
	{
		return i.ToString();
	}

	#region 定义一个长时间运行线程 并在合适时机取消
	{
		//表示线程同步事件在一个等待线程释放后收到信号时自动重置
		//AutoResetEvent 常常被用来在两个线程之间进行信号发送
		//两个线程共享相同的AutoResetEvent对象，线程可以通过调用AutoResetEvent对象的WaitOne()方法进入等待状态，
		//然后另外一个线程通过调用AutoResetEvent对象的Set()方法取消等待的状态。
		AutoResetEvent Pause = new AutoResetEvent(false);

		//定义可取消任务的token
		var cts = new CancellationTokenSource();

		ConcurrentQueue<Tuple<string, string>> LogQueue = new ConcurrentQueue<Tuple<string, string>>();
		DateTime now = DateTime.Now;
		LogQueue.Enqueue(new Tuple<string, string>(@"D:/1.log", now.ToLongTimeString() + "这是测试1.1"));
		LogQueue.Enqueue(new Tuple<string, string>(@"D:/1.log", now.ToLongTimeString() + "这是测试1.2"));
		LogQueue.Enqueue(new Tuple<string, string>(@"D:/2.log", now.ToLongTimeString() + "这是测试2"));
		//定义Task 在何时时机启动
		var writeTask = new Task(() =>
		{
			while (true)
			{
				Pause.WaitOne(1000, true);
				List<string[]> temp = new List<string[]>();
				foreach (var queue in LogQueue)
				{
					string logPath = queue.Item1;
					string logMergeContent = string.Concat(queue.Item2, Environment.NewLine, "------------", Environment.NewLine);
					string[] logArr = temp.FirstOrDefault(d => d[0].Equals(logPath))!;
					if (logArr != null)
					{
						logArr[1] = string.Concat(logArr[1], logMergeContent);
					}
					else
					{
						logArr = new[] { logPath, logMergeContent };
						temp.Add(logArr);
					}
					LogQueue.TryDequeue(out Tuple<string, string> _);
				}
				foreach (var item in temp)
				{
					Util.WriteText(item[0], item[1]);
				}
			}
		}, cts.Token, TaskCreationOptions.LongRunning);
		//参数说明:TaskCreationOptions.LongRunning
		//指定任务将是长时间运行的、粗粒度的操作，涉及比细化的系统更少、更大的组件。 
		//它会向 TaskScheduler 提示，过度订阅可能是合理的。 
		//可以通过过度订阅创建比可用硬件线程数更多的线程。 
		//它还将提示任务计划程序：该任务需要附加线程，以使任务不阻塞本地线程池队列中其他线程或工作项的向前推动。
		await Task.Delay(1000 * 1);
		cts.Cancel();//取消任务 , 注意此任务为不带参数的,若带参数任务取消不掉???
		if (!writeTask.IsCanceled)
		{
			writeTask.Start();
		}
	}
	#endregion

	{
		//WaitAsync on Task
		//可以更轻松地等待异步任务执行, 如果超时会抛出 “TimeoutException”
		try
		{
			Task operationTask = DoSomethingLongAsync();
			await operationTask.WaitAsync(TimeSpan.FromSeconds(5));
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message.ToString());
		}
	}
}

public async Task TestTaskMore()
{
	IProgress<int> progress = new Progress<int>(n =>
{
	$"当前进度: {n}%".Dump("进度报告");
});

	CancellationTokenSource ts = new CancellationTokenSource();
	CancellationToken cancellationToken = ts.Token;

	Task t = new Task(() =>
	{
		Thread.Sleep(1000);
		"Hello,Task".Dump();
	}, cancellationToken);

	t.Status.Dump("Created");

	t.Start();

	//if you're writing app-level code, do not use ConfigureAwait(false)
	//if you're writing general-purpose library code, use ConfigureAwait(false) 
	await t.ConfigureAwait(true);
	t.Status.Dump("Waiting");
	await Task.WhenAny(new[]{ DoWork(progress, cancellationToken), t.ContinueWith(x => t.Status.Dump("Continue")).ContinueWith(x =>
	{
		Thread.Sleep(1000);
		ts.Cancel();
		ts.Token.IsCancellationRequested.Dump("ts");
	})});

	string.Join(Environment.NewLine, Enum.GetNames(typeof(TaskStatus))).Dump("TaskStatus");
}

public async Task<Task> DoWork(IProgress<int> progress, CancellationToken cancellationToken)
{
	int i = 0;
	int max = 10;
	while (!cancellationToken.IsCancellationRequested && i < max)
	{
		await Task.Yield();
		Thread.Sleep(1000);
		progress.Report((int)Math.Ceiling((double)(i + 1) / max * 100));
		i++;
	}
	if (!cancellationToken.IsCancellationRequested) { return Task.CompletedTask; }

	return Task.FromCanceled(cancellationToken);
}

public static class Util
{
	public static void WriteText(string logPath, string logContent)
	{
		try
		{
			if (!File.Exists(logPath))
			{
				File.CreateText(logPath).Close();
			}
			using var sw = File.AppendText(logPath);
			sw.Write(logContent);
		}
		catch (Exception e)
		{
			throw e;
		}
	}
}

async Task DoSomethingLongAsync()
{
	Console.WriteLine("DoSomethingLongAsync start");
	await Task.Delay(TimeSpan.FromSeconds(10));
	Console.WriteLine("DoSomethingLongAsync ended.");
}