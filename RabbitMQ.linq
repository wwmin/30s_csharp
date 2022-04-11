<Query Kind="Program">
  <NuGetReference>RabbitMQ.Client</NuGetReference>
  <Namespace>RabbitMQ.Client</Namespace>
  <Namespace>System.Threading.Channels</Namespace>
  <Namespace>RabbitMQ.Client.Events</Namespace>
</Query>

MqOption _option = null;
IModel channel = null;
private readonly string _queueName = "queueName";
private readonly string _exchangeName = "exchangeName";
private readonly string _routingKey = "web:event:log";

void Main()
{
	_option = new();
	_option.UserName = "guest";
	_option.Password = "guest";
	_option.VirtualHost = "/";
	_option.HostName = "localhost";
	_option.Port = 5672;
	_option.ClientProvidedName = "web:event:log";
	InitMQ();
}


private void InitMQ()
{
	ConnectionFactory connectionFactory = new ConnectionFactory();
	connectionFactory.UserName = _option.UserName;
	connectionFactory.Password = _option.Password;
	connectionFactory.VirtualHost = _option.VirtualHost;
	connectionFactory.HostName = _option.HostName;
	connectionFactory.Port = _option.Port;
	connectionFactory.ClientProvidedName = _option.ClientProvidedName;
	IConnection conn = connectionFactory.CreateConnection();
	channel = conn.CreateModel();
	channel.ExchangeDeclare(_exchangeName, ExchangeType.Direct);
	channel.QueueDeclare(_queueName, false, false, false, null);
	channel.QueueBind(_queueName, _exchangeName, _routingKey, null);
	
	Consumer();
}

public void Publish(string msg)
{
	byte[] messageBodyBytes = System.Text.Encoding.UTF8.GetBytes(msg);
	IBasicProperties props = channel.CreateBasicProperties();
	props.ContentType = "text/plain";
	props.DeliveryMode = 2;
	channel.BasicPublish(_exchangeName, _routingKey, props, messageBodyBytes);
}


public void Consumer(){
	channel.QueueDeclare(queue:_queueName,durable:false,exclusive:false,autoDelete:false,arguments:null);
	var consumer = new EventingBasicConsumer(channel);
	consumer.Received+=(model,ea)=>{
		var body = ea.Body.ToArray();
		var message = Encoding.UTF8.GetString(body);
		Console.WriteLine(" [x] Received {0}",message);
	};
	channel.BasicConsume(queue:_queueName,autoAck:true,consumer:consumer);
	Console.WriteLine("Press [enter] to exit.");
	Console.ReadLine();
}


/// <summary>
/// MQ Option
/// </summary>
public class MqOption
{
	/// <summary>
	/// 用户名
	/// </summary>
	public string UserName { get; set; }
	/// <summary>
	/// 用户密码
	/// </summary>
	public string Password { get; set; }
	/// <summary>
	/// VirtualHost
	/// </summary>
	public string VirtualHost { get; set; }
	/// <summary>
	/// HostName
	/// </summary>
	public string HostName { get; set; }
	/// <summary>
	/// Port
	/// </summary>
	public int Port { get; set; }
	/// <summary>
	/// ClientProvidedName
	/// </summary>
	public string ClientProvidedName { get; set; }
}