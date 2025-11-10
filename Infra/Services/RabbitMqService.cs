using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infra.Services
{
    internal class RabbitMqService : IDisposable
    {
        public ConnectionFactory? factory { get; set; }
        public IConnection? connection;
        public IChannel? channel;
        public string queue = String.Empty;

        public RabbitMqService()
        {
            var hostname = Environment.GetEnvironmentVariable("RabbitMQ__HostName") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("RabbitMQ__Port") ?? "5672";
            var username = Environment.GetEnvironmentVariable("RabbitMQ__Username") ?? "guest";
            var password = Environment.GetEnvironmentVariable("RabbitMQ__Password") ?? "guest";
            factory = new ConnectionFactory
            {
                HostName = hostname,
                Port = Convert.ToInt32(port),
                UserName = username,
                Password = password
            };
        }

        public async Task Connect(string queue)
        {
            this.queue = queue;
            connection = await factory!.CreateConnectionAsync();
            channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);
            await channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queue: queue, exchange: "logs", routingKey: string.Empty);
        }

        public async Task Publish(ReadOnlyMemory<byte> body)
        {
            if (channel is null)
            {
               throw new Exception("Error publishing in rabbit. Must call Connect first");
            }
            await channel.BasicPublishAsync(exchange: "logs", routingKey: string.Empty, body: body);
        }

        public async Task Consume(AsyncEventingBasicConsumer consumer)
        {
            if (channel is null)
            {
               throw new Exception("Error consuming from rabbit. Must call Connect first");
            }
            await channel.BasicConsumeAsync(this.queue, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            if (channel is not null) channel.Dispose();
            if (connection is not null) connection.Dispose();
        }
    }
}
