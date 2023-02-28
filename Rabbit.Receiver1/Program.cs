using Rabbit.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new()
{
    Uri = new Uri(ConfigurationConstants.RabbitUri),
    ClientProvidedName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name
};

IConnection connection = factory.CreateConnection();
IModel channel = connection.CreateModel();

channel.ExchangeDeclare(ConfigurationConstants.ExchangeName, ExchangeType.Direct);
channel.QueueDeclare(ConfigurationConstants.QueueName, false, false, false, null);
channel.QueueBind(ConfigurationConstants.QueueName, ConfigurationConstants.ExchangeName, ConfigurationConstants.RoutingKey, null);
channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (sender, args) =>
{
    Task.Delay(TimeSpan.FromSeconds(5)).Wait();
    var body = args.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message Received: {message}");
    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(ConfigurationConstants.QueueName, false, consumer);
Console.ReadLine();

channel.BasicCancel(consumerTag);

channel.Close();
connection.Close();