using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : Controller
    {
        private readonly IProductRepository _repository;
        private readonly IProductPublisher _publisher;
        private readonly IProductEventHandler _eventHandler;

        public ProdutoController(IProductRepository repository, IProductPublisher publisher, IProductEventHandler eventHandler)
        {
            _repository = repository;
            _publisher = publisher;
            _eventHandler = eventHandler;
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

            // TODO: REMOVE from here and put it on Create !!!!!!!!!!!!!!!!!!!!!
            var serializedEvent = _eventHandler.Create(product); // this function will return the serialized (JSON) ProductEvent 
            if (!serializedEvent.IsNullOrEmpty())
            {
                await _publisher.Publish(serializedEvent);
            }

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

            // TODO: put send event logic here

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
