using System.Threading.Channels;
await Run();
private static async Task Run()
{
    await SingleProducerSingleConsumer();
}
public static async Task SingleProducerSingleConsumer()
{
    var channel = Channel.CreateUnbounded<string>();

    var consumer1 = new Consumer(channel.Reader, 1);

    Task consumerTask1 = consumer1.ConsumeData(); // begin consuming

    await consumerTask1;
}

class Consumer
{
    private readonly ChannelReader<string> _reader;
    private readonly int _identifier;
    public Consumer(ChannelReader<string> reader, int identifier)
    {
        _reader = reader;
        _identifier = identifier;
    }

    public async Task ConsumeData()
    {
        Console.WriteLine($"CONSUMER ({_identifier}): Starting");

        while (await _reader.WaitToReadAsync())
        {
            if (_reader.TryRead(out var timeString))
            {
                Console.WriteLine($"CONSUMER ({_identifier}): Consuming {timeString}");
            }
        }

        Console.WriteLine($"CONSUMER ({_identifier}): Completed");
    }

}