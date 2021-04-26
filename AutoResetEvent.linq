<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

//AutoResetEvent是.net线程简易同步方法中的一种。
//AutoResetEvent 常常被用来在两个线程之间进行信号发送
//两个线程共享相同的AutoResetEvent对象，线程可以通过调用AutoResetEvent对象的WaitOne()方法进入等待状态，然后另外一个线程通过调用AutoResetEvent对象的Set()方法取消等待的状态。
AutoResetEvent autoResetEvent = new AutoResetEvent(false);
async Task Main()
{
	var data = "";
	Task task = Task.Factory.StartNew(async () =>
	{
		data = await GetDataFromServerAsync();
	});
	await Task.Delay(1);
	//Put the current thread into waiting state until it receives the signal
	autoResetEvent.WaitOne();
	Console.WriteLine(data);

}

async Task<string> GetDataFromServerAsync()
{
	//Calling any webservice to get data
	Thread.Sleep(TimeSpan.FromSeconds(1));
	autoResetEvent.Set();
	return await Task.FromResult("Webservice data ....................!");
}