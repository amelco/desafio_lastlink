using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infra.Consumers
{
    public class ProductConsumerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProductConsumerWorker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory { HostName = "192.168.0.8" };
            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);

            await channel.QueueBindAsync(queue: "logs_queue", exchange: "logs", routingKey: string.Empty);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var productEvent = JsonConvert.DeserializeObject<ProductEvent>(message);
                if (productEvent != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var eventRepository = scope.ServiceProvider.GetRequiredService<IProductEventRepository>();
                    await eventRepository.Add(productEvent);
                }

                return;
            };

            await channel.BasicConsumeAsync("logs_queue", autoAck: true, consumer: consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
