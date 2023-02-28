using Rabbit.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

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

int messagesToSend = 60;
for (int i = 0; i < messagesToSend; i++)
{
    var widget = new Widget($"Widget #{i}");
    var message = JsonSerializer.Serialize(widget);
    Console.WriteLine($"Message Sent: {message}");
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes(message);
    channel.BasicPublish(ConfigurationConstants.ExchangeName, ConfigurationConstants.RoutingKey, null, messageBodyBytes);
    Thread.Sleep(1000);
}

channel.Close();
connection.Close();
Console.ReadLine();