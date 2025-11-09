using Core.Entities;
using FluentAssertions;
using Xunit;
using Infra.EventHandler;

namespace Infra.Tests.EventHandler
{
    public class ProductEventHandlerTests
    {
        private readonly ProductEventHandler _handler;

        public ProductEventHandlerTests()
        {
            _handler = new ProductEventHandler();
        }

        [Fact]
        public void Create_WithValidProduct_ShouldReturnSerializedProductEvent()
        {
            var product = new Product { Id = 1 };

            var result = _handler.Create(product);

            result.Should().NotBeNull();
            result.Should().Contain("product.created");
            result.Should().Contain("\"ProductId\":1");
        }

        [Fact]
        public void Create_ShouldProduceValidJsonWithTimestamp()
        {
            var product = new Product { Id = 42 };

            var json = _handler.Create(product);

            json.Should().NotBeNullOrWhiteSpace();
            json.Should().Contain("product.created");
            json.Should().Contain("\"ProductId\":42");
            json.Should().Contain("CreatedAt");
        }
    }
}