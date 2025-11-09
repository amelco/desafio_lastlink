using Api.Controllers;
using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRepository> _mockRepository;
        private readonly Mock<IProductPublisher> _mockPublisher;
        private readonly Mock<IProductEventHandler> _mockEventHandler;
        private readonly ProdutoController _controller;

        public ProductControllerTests()
        {
            _mockRepository = new Mock<IProductRepository>();
            _mockPublisher = new Mock<IProductPublisher>();
            _mockEventHandler = new Mock<IProductEventHandler>();
            _controller = new ProdutoController(_mockRepository.Object, _mockPublisher.Object, _mockEventHandler.Object);
        }

        [Fact]
        public void GetAll_ShouldReturnOkResult_WithListOfProducts()
        {
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Category = "Category 1", UnitCost = 10.00m, CreatedAt = DateTime.Now },
                new Product { Id = 2, Name = "Product 2", Category = "Category 2", UnitCost = 20.00m, CreatedAt = DateTime.Now }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(products);

            var result = _controller.GetAll();

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProducts = okResult.Value.Should().BeAssignableTo<IEnumerable<ProductDto>>().Subject;
            returnedProducts.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnOkResult()
        {
            var product = new Product { Id = 1, Name = "Product 1", Category = "Category 1", UnitCost = 10.00m, CreatedAt = DateTime.Now };
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(product);

            var result = await _controller.GetById(1);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProduct = okResult.Value.Should().BeOfType<ProductDto>().Subject;
            returnedProduct.Id.Should().Be(product.Id);
            returnedProduct.Name.Should().Be(product.Name);
            returnedProduct.Category.Should().Be(product.Category);
            returnedProduct.UnitCost.Should().Be(product.UnitCost);
        }

        [Fact]
        public async Task GetById_WithNonExistingId_ShouldReturnNotFound()
        {
            _mockRepository.Setup(repo => repo.GetById(1)).ReturnsAsync((Product)null!);

            var result = await _controller.GetById(1);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_WithValidData_ShouldReturnOkResult()
        {
            var createDto = new UpdateProductDto
            {
                Name = "New Product",
                Category = "New Category",
                UnitCost = 15.00m
            };

            var createdProduct = new Product
            {
                Id = 1,
                Name = createDto.Name,
                Category = createDto.Category,
                UnitCost = createDto.UnitCost.Value,
                CreatedAt = DateTime.Now
            };

            _mockRepository.Setup(repo => repo.Add(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            _mockEventHandler.Setup(handler => handler.Create(It.IsAny<Product>()))
                .Returns("serialized object");

            var result = await _controller.Create(createDto);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProduct = okResult.Value.Should().BeOfType<ProductDto>().Subject;
            returnedProduct.Id.Should().Be(createdProduct.Id);
            returnedProduct.Name.Should().Be(createDto.Name);
            returnedProduct.Category.Should().Be(createDto.Category);
            returnedProduct.UnitCost.Should().Be(createDto.UnitCost.Value);

            _mockPublisher.Verify(p => p.Publish(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Create_WithValidData_AndNoEvents_ShouldReturnOkResult()
        {
            var createDto = new UpdateProductDto
            {
                Name = "New Product",
                Category = "New Category",
                UnitCost = 15.00m
            };

            var createdProduct = new Product
            {
                Id = 1,
                Name = createDto.Name,
                Category = createDto.Category,
                UnitCost = createDto.UnitCost.Value,
                CreatedAt = DateTime.Now
            };

            _mockRepository.Setup(repo => repo.Add(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            _mockEventHandler.Setup(handler => handler.Create(It.IsAny<Product>()))
                .Returns("serialized object");

            var result = await _controller.Create(createDto);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProduct = okResult.Value.Should().BeOfType<ProductDto>().Subject;
            returnedProduct.Id.Should().Be(createdProduct.Id);

            _mockPublisher.Verify(p => p.Publish(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Update_WithValidData_ShouldReturnOkResult()
        {
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Category = "Updated Category",
                UnitCost = 25.00m
            };

            var updatedProduct = new Product
            {
                Id = 1,
                Name = updateDto.Name,
                Category = updateDto.Category,
                UnitCost = updateDto.UnitCost.Value,
                CreatedAt = DateTime.Now
            };

            _mockRepository.Setup(repo => repo.Update(1, It.IsAny<Product>()))
                .ReturnsAsync(updatedProduct);

            var result = await _controller.Update(1, updateDto);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returnedProduct = okResult.Value.Should().BeOfType<ProductDto>().Subject;
            returnedProduct.Id.Should().Be(updatedProduct.Id);
            returnedProduct.Name.Should().Be(updateDto.Name);
            returnedProduct.Category.Should().Be(updateDto.Category);
            returnedProduct.UnitCost.Should().Be(updateDto.UnitCost.Value);
        }

        [Fact]
        public async Task Update_WithNonExistingId_ShouldReturnNotFound()
        {
            var updateDto = new UpdateProductDto
            {
                Name = "Updated Product",
                Category = "Updated Category",
                UnitCost = 25.00m
            };

            _mockRepository.Setup(repo => repo.Update(1, It.IsAny<Product>()))
                .ReturnsAsync((Product)null!);

            var result = await _controller.Update(1, updateDto);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteById_WithExistingId_ShouldReturnNoContent()
        {
            _mockRepository.Setup(repo => repo.Delete(1))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteById(1);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteById_WithNonExistingId_ShouldReturnNotFound()
        {
            _mockRepository.Setup(repo => repo.Delete(1))
                .ThrowsAsync(new KeyNotFoundException());

            var result = await _controller.DeleteById(1);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
