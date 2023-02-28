using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new()
{
    Uri = new Uri("amqp://guest:guest@localhost:5672"),
    ClientProvidedName = "Rabbit Sender App"
};

IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

string exchangeName = "DemoExchange";
string routingKey = "demo-routing-key";
string queueName = "DemoQueue";

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

byte[] messageBodyBytes = Encoding.UTF8.GetBytes("Hello World!");