<Query Kind="Program">
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//channel demo
async Task Main()
{
	CancellationTokenSource produceCancelTokenSource = new CancellationTokenSource();
	//5秒钟后发送取消事件
	produceCancelTokenSource.CancelAfter(1000 * 5);
	CancellationToken produceCancelToken = produceCancelTokenSource.Token;
	CancellationTokenSource consumeCancelTokenSource = new CancellationTokenSource();
	CancellationToken consumeCancelToken = consumeCancelTokenSource.Token;

	await Task.Run(async () =>
	{
		//单个生产消费模式
		//有限容量channel
		var channel = Channel.CreateBounded<string>(new BoundedChannelOptions(10)
		{
			FullMode = BoundedChannelFullMode.Wait
		});

		var producer1 = new Producer(channel.Writer, 1, produceCancelToken);
		var produceTask1 = producer1.ProduceData(1111.ToString());

		var producer2 = new Producer(channel.Writer, 2, produceCancelToken);

		var producerTask2 = producer2.ProduceData(2222.ToString());

		var consumer1 = new Consumer(channel.Reader, 1, consumeCancelToken);
		Task consumerTask1 = consumer1.ConsumeData(); // begin consuming

		await consumerTask1;
	});
}


class Producer
{
	private readonly ChannelWriter<string> _writer;
	private readonly int _identifier;
	private readonly CancellationToken _cancellationToken;
	public Producer(ChannelWriter<string> writer, int identifier, CancellationToken cancellationToken)
	{
		_writer = writer;
		_identifier = identifier;
		_cancellationToken = cancellationToken;
	}

	public async Task ProduceData(string data)
	{
		while (true)
		{

			Console.WriteLine($"PRODUCER ({_identifier}): {data}");
			while (await _writer.WaitToWriteAsync(_cancellationToken))
			{
				await _writer.WriteAsync(data, _cancellationToken);
				await Task.Delay(10);
				Console.WriteLine("生产中.... data:" + data);
				_cancellationToken.ThrowIfCancellationRequested();
			}
		}
	}
}

class Consumer
{
	private readonly ChannelReader<string> _reader;
	private readonly int _identifier;
	private readonly CancellationToken _cancelToken;
	public Consumer(ChannelReader<string> reader, int identifier, CancellationToken cancelToken)
	{
		_reader = reader;
		_identifier = identifier;
		_cancelToken = cancelToken;
	}

	public async Task ConsumeData()
	{
		Console.WriteLine($"CONSUMER ({_identifier}): Starting");

		while (await _reader.WaitToReadAsync(_cancelToken))
		{
			if (_reader.TryRead(out var data))
			{
				Console.WriteLine($"CONSUMER ({_identifier}): Consuming {data}");
				await Task.Delay(1000);
			}
			_cancelToken.ThrowIfCancellationRequested();
		}

		Console.WriteLine($"CONSUMER ({_identifier}): Completed");
	}
}
