<Query Kind="Program">
  <Output>DataGrids</Output>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

ManualResetEvent manualResetEvent = new ManualResetEvent(false);
void Main()
{
	"one".Dump();
	SendManualSetSinal();
	//该方法阻塞当前线程并等待其他线程发送信号。如果收到信号，它将返回True，反之返回False
	//等待manualResetEvent.Set();执行且最多等待2秒
	manualResetEvent.WaitOne(TimeSpan.FromSeconds(2));
	"two".Dump();
	SendManualResetSignal();
	manualResetEvent.WaitOne(TimeSpan.FromSeconds(2));
	"three".Dump();

	//完整示例
	Task task = Task.Factory.StartNew(() =>
	{
		GetDataFromServer(1);
	});

	Task.Factory.StartNew(() => {
		GetDataFromServer(2);
	});
	//Send first signal to get first set of data from server 1 and server 2
	manualResetEvent.Set();
	manualResetEvent.Reset();
	
	Thread.Sleep(TimeSpan.FromSeconds(2));
	//Send second signal to get second set of data from server 1 and server 2
	manualResetEvent.Set();
}

//该方法用于给所有等待线程发送信号。 Set() 方法的调用使得ManualResetEvent对象的bool变量值为True，所有线程被释放并继续执行。
void SendManualSetSinal(){
	manualResetEvent.Set();
}
//  一旦我们调用了ManualResetEvent对象的Set()方法，它的bool值就变为true,我们可以调用Reset()方法来重置该值，Reset()方法重置该值为False
void SendManualResetSignal(){
	manualResetEvent.Reset();
}

void GetDataFromServer(int serverNumber)
{
	//Calling any webservice to get data
	Console.WriteLine("I get first data from server" + serverNumber);
	manualResetEvent.WaitOne();

	Thread.Sleep(TimeSpan.FromSeconds(2));
	Console.WriteLine("I get second data from server" + serverNumber);
	manualResetEvent.WaitOne();
	Console.WriteLine("All the data collected from server" + serverNumber);
}

