<Query Kind="Program">
  <Connection>
    <ID>9e9bb15b-e9d1-4d55-b6b8-166107188e3f</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>.</Server>
    <DisplayName>localhost_sqlserver</DisplayName>
    <Database>Eletcric_2</Database>
  </Connection>
  <Output>DataGrids</Output>
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Windows</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

static void Main()
{
	void FormWebClient()
	{
		var btn1 = new Button("button to download uri").Dump("button");
		var txtBox1 = new TextArea().Dump();
		btn1.Click += (Object sender, EventArgs e) =>
		{
			new Thread(() =>
			{
				var client = new WebClient();
				var s = client.DownloadString("https://www.baidu.com");
				//MessageBox.Show(s,"网页数据");
				txtBox1.Text = s;
			}).Start();
		}
		!;
	}
	//FormWebClient();

	void FormClick()
	{


		var btn2 = new Button("button to download with async").Dump("button");
		var txt2 = new TextArea().Dump();
		btn2.Click += (sender, e) =>
		{
			var client = new WebClient();
			client.DownloadStringCompleted += (ssender, ee) =>
			{
				txt2.Text = ee.Result;
			};
			client.DownloadStringAsync(new Uri("https://www.baidu.com"));
		};
	}
	//FormClick();

	void FormThreadReset()
	{


		//AutoResetEvent
		AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		Label lb1 = new Label("btn3 label1").Dump("label 1");
		Label lb2 = new Label("btn3 label2").Dump("label 2");
		var btn3 = new Button("先点我开始处理任务").Dump("button with AutoResetEvent");
		btn3.Click += (sender, e) =>
		{
			new Thread(() =>
			{
				lb1.Text = "线程1开启,等待信号....";
				autoResetEvent.WaitOne();//带有超时的线程阻塞等待
										 //todo: 处理一些工作
				lb1.Text = "继续1工作";
			}).Start();

			new Thread(() =>
			{
				lb2.Text = "线程2开启,等待信号...";
				autoResetEvent.WaitOne();
				//todo: 处理一些工作
				lb2.Text = "继续2工作";
			}).Start();
		};
		var btn4 = new Button("后点我,将上面的等待任务继续").Dump("需要点两次,使两个等待的Thread依次执行");
		btn4.Click += (sender, e) =>
		{
			autoResetEvent.Set();
		};
	}
	//FormThreadReset();

	//有关lock
	void AboutLock()
	{
		object lockObj = new object();
		AutoResetEvent autoResetEvent = new AutoResetEvent(false);
		List<string> list = Enumerable.Range(1, 6).Select(d => d.ToString()).ToList();
		List<string> res = new List<string>();
		new Thread(() =>
		{
			autoResetEvent.WaitOne();
			lock (lockObj)
			{
				foreach (var i in list)
				{
					Thread.Sleep(100);
					res.Add(i);
					i.Dump("加锁执行后的list");
				}

			}
		}).Start();

		new Thread(() =>
		{
			autoResetEvent.Set();//保证这个线程已经开始了才能执行上面的线程
			Thread.Sleep(200);
			lock (lockObj)
			{
				list.RemoveAt(0);//加锁移除一个元素
				string.Join(",", list).Dump("移除一个元素之后的list");
			}
		}).Start();
	}
	//AboutLock();

	void ForThread()
	{
		for (int i = 0; i < 10; i++)
		{
			new Thread(() =>
			{
				i.Dump("直接执行的Thread");
			}).Start();
			//consoleFn(i);
		}
		void consoleFn(int i)
		{
			new Thread(() => { i.Dump("循环外定义的thread"); }).Start();
		}
	}
	//ForThread();

	void DoHttp()
	{
		var cts = new CancellationTokenSource();
		using (HttpClient client = new HttpClient()
		{
			BaseAddress = new Uri("https://www.google.com"),
			Timeout = TimeSpan.FromSeconds(3)
		})
		{
			var result = client.GetStringAsync("/", cts.Token).ContinueWith(t =>
			 {
				 if (t.IsCanceled)
				 {
					 "线程被取消了".Dump();
				 }
				 if (t.IsFaulted)
				 {
					 "发生异常而被取消了".Dump();
				 }
				 if (t.IsCompleted)
				 {
					 return t.Result;
				 }
				 return null;
			 }).Result;
			result.Dump("do http");
		}
		cts.CancelAfter(1000);
	}
	//DoHttp();

	void DoTaskFactory()
	{
		var cts = new CancellationTokenSource();
		Task[] tasks = {
			Task.Factory.StartNew (() => {
				"我是异步线程1...".Dump();
				Thread.Sleep(1000);
			},cts.Token),
			Task.Factory.StartNew (() => {
				"我是异步线程2...".Dump();
				Thread.Sleep(1000);
			},cts.Token),
			Task.Factory.StartNew (() => {
				"我是异步线程3...".Dump();
				Thread.Sleep(1000);
			},cts.Token)
		};
		cts.Cancel();
		//cts.CancelAfter(100);
		Task.Factory.ContinueWhenAll(tasks, ts =>
		{
			$"线程被取消{ts.Count(t => t.IsCanceled)}次".Dump();
		});
	}
	//DoTaskFactory();

	void DoWaitAsync()
	{
		var sw = new Stopwatch();
		sw.Start();
		var task = MyMethod();
		Thread.Sleep(4000);
		var result = task.Result;
		sw.ElapsedMilliseconds.Dump("task elapsed");
	}
	async Task<string> MyMethod()
	{
		await Task.Delay(5000);
		return "aaa";
	}
	//DoWaitAsync();


	//并行计算 Parallel
	void DoParallel()
	{
		Parallel.Invoke(() =>
		{
			"线程1".Dump();
			Thread.Sleep(1000);
		},() =>
			   {
				   "线程2".Dump();
				   Thread.Sleep(1000);
			   },() =>
				{
					"线程3".Dump();
					Thread.Sleep(1000);
				});
		"-----------------------".Dump();
		Parallel.For(0, 4, i =>
		{
			i.Dump("invoke i");
		});
		"-----------------------".Dump();
		var list = new List<int>() { 1, 2, 3, 4, 5 };
		Parallel.ForEach(list, item =>
		{
			item.Dump("Parallel ForEach List");
		});

		"执行结束".Dump("查看此位置,可知Parallel是阻塞线程执行的");
	}
	//DoParallel();

	void DoPLinq()
	{
		var list = Enumerable.Range(0, 100).ToList();
		var query = from i in list select i;
		foreach (var i in query)
		{
			i.Dump("linq");
		}
		"-----------------------".Dump();
		var query2 = from i in list.AsParallel().AsOrdered() select i;
		foreach (var i in query2)
		{
			i.Dump("plinq");
		}
		"-----------------------".Dump();
		query2.ForAll(Console.WriteLine);
		foreach (var i in list.AsParallel().Take(3))
		{
			i.Dump("类似随机取3个");
		}
	}
	//DoPLinq();

	async Task DoThrowException()
	{
		await Task.CompletedTask;
		throw new Exception("线程内抛异常");
	}
	//DoThrowException();

	void DoTaskCatchException()
	{
		var t = Task.Run(() => throw new Exception("我在异步线程中抛出的异常"));
		try
		{
			t.Wait();//此处在主线程有阻塞
		}
		catch (Exception ex)
		{

			throw ex;
		}
	}
	//DoTaskCatchException();

	void DoTaskNoBlock()
	{
		Task.Run(() => throw new Exception("我在异步线程中抛异常")).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				foreach (var ex in t.Exception?.InnerExceptions)
				{
					$"异常类型,{ex.GetType()},{Environment.NewLine}异常源:{ex.Source},{Environment.NewLine}异常信息:{ex.Message}".Dump();
				}
			}
		});
	}
	//DoTaskNoBlock();


	//生产中建议使用这种方式捕获异步异常信息 
	void DoTaskThrowExceptionToMainThread()
	{
		CatchedAggregateExceptionEventHandler += (sender, e) =>
		{
			foreach (var ex in e.AggregateException.InnerExceptions)
			{
				$"异常类型,{ex.GetType()},{Environment.NewLine}异常源:{ex.Source},{Environment.NewLine}异常信息:{ex.Message}".Dump("DoTaskThrowExceptionToMainThread");
			}
		};

		var task = Task.Run(() =>
		{
			try
			{
				throw new Exception("我在异步线程中抛出的异常");
			}
			catch (Exception e)
			{
				CatchedAggregateExceptionEventHandler(null, new AggregateExceptionArgs()
				{
					AggregateException = new AggregateException(e)
				}
				);
			}
		});
	}

	//DoTaskThrowExceptionToMainThread();

	//并行方式异常处理
	void DoCatchParallelException()
	{
		var exs = new ConcurrentQueue<Exception>();
		try
		{
			Parallel.For(0, 2, i =>
			{
				try
				{
					throw new ArgumentException();
				}
				catch (Exception ex)
				{
					exs.Enqueue(ex);
				}
				if (exs.Any())
				{
					throw new AggregateException(exs);
				}
			});
		}
		catch (AggregateException e)
		{
			foreach (var ex in e.InnerExceptions)
			{
				$"异常类型,{ex.GetType()},{Environment.NewLine}异常源:{ex.Source},{Environment.NewLine}异常信息:{ex.Message}".Dump("DoCatchParallelException");
			}
		}
	}
	DoCatchParallelException();
}



//使用事件方式向主线程抛传递异常
static event EventHandler<AggregateExceptionArgs> CatchedAggregateExceptionEventHandler;//一般用static修饰
public class AggregateExceptionArgs : EventArgs
{
	public AggregateException AggregateException { get; set; }
}
