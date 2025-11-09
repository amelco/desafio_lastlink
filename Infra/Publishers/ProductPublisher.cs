using Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;

namespace Infra.Publishers
{
    public class ProductPublisher : IProductPublisher
    {
        public async Task Publish(string message)
        {
            var hostname = System.Environment.GetEnvironmentVariable("RabbitMQ__HostName") ?? "localhost";
            var factory = new ConnectionFactory { HostName = hostname };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);
            await channel.QueueDeclareAsync(queue: "logs_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            await channel.QueueBindAsync(queue: "logs_queue", exchange: "logs", routingKey: string.Empty);

            if (message.IsNullOrEmpty()) return;

            var body = Encoding.UTF8.GetBytes(message);
            await channel.BasicPublishAsync(exchange: "logs", routingKey: string.Empty, body: body);
        }
    }
}
