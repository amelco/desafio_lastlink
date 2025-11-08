using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IProductPublisher _publisher;

        public ProdutoController(IProductRepository repository, IProductPublisher publisher)
        {
            _repository = repository;
            _publisher = publisher;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ProductDto>> GetById([FromRoute] int id)
        {
            var product = await _repository.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            // WARNING: REMOVE!!!!!!!!!!!!!!!!!!!!!
            await _publisher.Publish("", $"Produto {product.Name} de id {product.Id} foi criado em {product.CreatedAt}.");

            var productDto = product.Adapt<ProductDto>();
            return Ok(productDto);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> GetAll()
        {
            var products = _repository.GetAll();
            var productsDto = products.Adapt<IEnumerable<ProductDto>>();
            return Ok(productsDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] UpdateProductDto productDto)
        {
            var product = productDto.Adapt<Product>();
            var createdProduct = await _repository.Add(product);

            //await _publisher.Publish("", $"Produto {product.Name} de id {product.Id} foi criado em {product.CreatedAt}.");

            var createdProductDto = createdProduct.Adapt<ProductDto>();
            return Ok(createdProduct);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteById([FromRoute] int id)
        {
            try
            {
                await _repository.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException) {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ProductDto>> Update([FromRoute] int id, [FromBody] UpdateProductDto productDto)
        {
            var product = productDto.Adapt<Product>();
            var updatedProduct = await _repository.Update(id, product);
            if (updatedProduct == null) 
            {
                return NotFound();
            }
            var updatedProductDto = updatedProduct.Adapt<ProductDto>();
            return Ok(updatedProductDto);
        }
    }
}
