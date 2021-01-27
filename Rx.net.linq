<Query Kind="Statements">
  <Output>DataGrids</Output>
  <NuGetReference>System.Reactive</NuGetReference>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Reactive.Linq</Namespace>
  <Namespace>System.Reactive.Concurrency</Namespace>
  <Namespace>System.Reactive.Subjects</Namespace>
  <Namespace>System.Reactive.Disposables</Namespace>
  <Namespace>System.Net.Http</Namespace>
</Query>

//Rx.net

//不使用Rx.net示例,使用委托实现
{
	Heater heater = new Heater();
	heater.BoilWater();
}

//使用Rx.net示例
{
	var observable = Enumerable.Range(1, 10).ToObservable(NewThreadScheduler.Default);//申明可观察序列
	Subject<int> subject = new Subject<int>();//申明Subject
	subject.Subscribe(temperature => $"当前温度{temperature}".Dump("subject-1"));//订阅subject
	subject.Subscribe(temperature => $"嘟嘟,当前水温{temperature}".Dump("subject-2"));//订阅subject
	observable.Subscribe(subject);//订阅observable
}



//query with linq
{
	var numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
	var result = numbers
	.Where(x => x % 2 == 1)
	.Where(x => x > 0)
	.Select(x => x + 2)
	.Distinct()
	.OrderBy(x => x);
	result.Dump("Linq method");

	var result2 = from number in numbers
				  where number % 2 == 1
				  where number > 0
				  orderby number
				  select number + 2;
	result2 = result2.Distinct();
	result2.Dump("Linq operators");

}



//Using the ToEnumerable operator
{

	var observable = Observable.Create<string>(o =>
	{
		o.OnNext("Observable");
		o.OnNext("To");
		o.OnNext("Enumerable");
		o.OnCompleted();
		return Disposable.Empty;
	});
	var enumerable = observable.ToEnumerable();
	enumerable.Dump("ToEnumerable");
}

//Using the ToList operator
{
	var observable = Observable.Create<string>(o =>
	{
		o.OnNext("Observable");
		o.OnNext("To");
		o.OnNext("List");
		o.OnCompleted();
		return Disposable.Empty;
	});

	IObservable<IList<string>> listObservable = observable.ToList();
	listObservable
		.Select(lst => string.Join(",", lst))
		.Subscribe(lst => lst.Dump());
}

//Using the ToDictionary operator
{
	IEnumerable<string> cities = new[] { "北京", "上海", "天津", "重庆" };
	var dictionaryObservable = cities.ToObservable().ToDictionary(c => c[..1]);
	dictionaryObservable.Select(d => string.Join(",", d)).Subscribe(d => d.Dump());
}

//Using the ToLookup operator
{
	IEnumerable<string> cities = new[] { "北京", "上海", "天津", "重庆", "北海" };
	var lookupObservable = cities.ToObservable().ToLookup(c => c[..1]);
	lookupObservable.Select(lookup =>
	{
		var groups = new StringBuilder();
		foreach (var grp in lookup)
		{
			groups.AppendFormat("[Key:{0}=>{1}]", grp.Key, grp.Count());
		}
		return groups.ToString();
	}).Subscribe(lookup => lookup.Dump("lookup"));
}

//Generating an observable loop
{
	IObservable<int> observable = Observable.Generate(
			0,            //Initial state
			i => i < 10,  //Condition (false means terminate)
			i => i + 1,   //Next iteration step
			i => i * 2    //The value in each iteration
		);
	observable.Subscribe(o => o.Dump("Generating an observable loop"));

	//to simpler
	IObservable<int> observable2 = Observable.Range(0, 10).Select(i => i * 2);
	observable2.Subscribe(o => o.Dump("Generating an observable loop 2"));
}

//Reading a file
{
	IObservable<string> lines = Observable.Generate(
		File.OpenText(@"D:\data\code\README.md"),
		s => !s.EndOfStream,
		s => s,
		s => s.ReadLine())!;
	lines.Subscribe(l => l.Dump("read a file"));
}

//Freeing resources with the Using operator
{
	IObservable<string> lines = Observable.Using(() => File.OpenText(@"D:\data\code\README.md"),
			stream => Observable.Generate(
				stream,
				s => !s.EndOfStream,
				s => s,
				s => s.ReadLine())
			)!;
	lines.Subscribe(l => l.Dump("read a file with freeing"));
}

//Primitive observables
{
	Observable.Return("Hello World!").Subscribe(o => o.Dump("Return"));
}

//Scheduling an emission with a timer
{

	//	IObservable<string> firstObservable = Observable.Interval(TimeSpan.FromSeconds(1)).Select(x => "first:" + x).Take(5);
	//	firstObservable.Dump("firstObservable");
	//	IObservable<string> secondObservable = Observable.Interval(TimeSpan.FromSeconds(2)).Select(x => "second:" + x).Take(5);
	//	secondObservable.Dump("secondObservable");
	//
	//	IObservable<IObservable<string>> immediateObservable = Observable.Return(firstObservable);
	//
	//	//Scheduling the second observable emission
	//	IObservable<IObservable<string>> scheduledObservable = Observable.Timer(TimeSpan.FromSeconds(5)).Select(x => secondObservable);
	//	immediateObservable.Merge(scheduledObservable).Switch().Timestamp().Subscribe(o => o.Dump("timer switch"));
}

//Adding side effects in the observable pipeline
{
	Observable.Range(1, 5).Do(x => Console.WriteLine($"{x} was emmitted"))
		.Where(x => x % 2 == 0)
		.Do(x => Console.WriteLine($"{x} survived the where()"))
		.Select(x => x * 3)
		.Subscribe(x => x.Dump());
}

//Subscribing the subject to multiple observables
{
	Subject<string> sbj = new Subject<string>();
	Observable.Interval(TimeSpan.FromSeconds(1))
		.Select(x => "First: " + x)
		.Take(5)
		.Subscribe(sbj);
	Observable.Interval(TimeSpan.FromSeconds(2))
		.Select(x => "Second: " + x)
		.Take(5)
		.Subscribe(sbj);
	sbj.Subscribe(s => s.Dump());
}

//Converting Task<T> to an observable by using AsyncSubject
{
	var tcs = new TaskCompletionSource<bool>();
	var task = tcs.Task;

	AsyncSubject<bool> sbj = new AsyncSubject<bool>();
	task.ContinueWith(t =>
	{
		switch (t.Status)
		{
			case TaskStatus.RanToCompletion:
				sbj.OnNext(t.Result);
				sbj.OnCompleted();
				break;
			case TaskStatus.Faulted:
				sbj.OnError(t?.Exception?.InnerException!);
				break;
			case TaskStatus.Canceled:
				sbj.OnError(new TaskCanceledException(t));
				break;
		}
	}, TaskContinuationOptions.ExecuteSynchronously);
	tcs.SetResult(true);
	sbj.Subscribe(s => s.Dump("AsyncSubject"));
}


//Task-Based Asynchronous Pattern
{
	var httpClient = new HttpClient();
	httpClient.GetAsync("http://www.baidu.com").ContinueWith(requestTask =>
	{
		$"the request was sent, status:{requestTask}".Dump();
		//requestTask.Result.Headers.Dump();
		var httpContent = requestTask.Result.Content;
		httpContent.ReadAsStringAsync().ContinueWith(contentTask =>
		{
			contentTask.Result.Dump();
		});
	});
}
//-------------------------------------实体类--------------------------------------------//

class Heater
{
	private delegate void TemperatureChanged(int temperature);
	private event TemperatureChanged? TemperatureChangedEvent;
	public void BoilWater()
	{
		TemperatureChangedEvent += ShowTemperature;
		TemperatureChangedEvent += MakeAlerm;
		Task.Run(() => Enumerable.Range(1, 10).ToList().ForEach(temperature => TemperatureChangedEvent(temperature)));
	}

	private void ShowTemperature(int temperature)
	{
		Console.WriteLine($"当前温度:{temperature}");
	}
	private void MakeAlerm(int temperature)
	{
		Console.WriteLine($"嘟嘟,当前水温{temperature}");
	}
}



class StringComparators
{
	public static bool CompareLength(string first, string second)
	{
		return first.Length == second.Length;
	}
	public bool CompareContent(string first, string second)
	{
		return first == second;
	}
}