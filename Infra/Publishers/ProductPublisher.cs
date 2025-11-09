using Core.Interfaces;
using Infra.Services;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Text;

namespace Infra.Publishers
{
    public class ProductPublisher : IProductPublisher
    {
        public async Task Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var rabbit = new RabbitMqService();
            
            await rabbit.Connect("logs_queue");
            await rabbit.Publish(body);

        }
    }
}
