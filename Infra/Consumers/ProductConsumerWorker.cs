using Core.Entities;
using Core.Interfaces;
using Infra.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

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
            var rabbit = new RabbitMqService();
            
            await rabbit.Connect("logs_queue");

            var consumer = new AsyncEventingBasicConsumer(rabbit.channel!);
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

            await rabbit.Consume(consumer);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);
            }
        }
    }
}
