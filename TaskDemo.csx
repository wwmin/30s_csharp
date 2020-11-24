

{
    static async Task Run()
    {
        await Task.Run(() => "hello".Dump());
    }

    await Run();
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