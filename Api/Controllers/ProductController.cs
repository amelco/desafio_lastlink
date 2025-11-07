using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("produto")]
    public class ProdutoController : Controller
    {
        private readonly IProductRepository _repository;

        public ProdutoController(IProductRepository repository)
        {
            _repository = repository;
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
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _repository.GetAll();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromBody] ProductDto produto)
        {
            var productEntity = produto.Adapt<Product>();
            if (productEntity == null)
            {
                return BadRequest(); // TODO: proper checking of values
            }

            var createdProduct = await _repository.Add(productEntity);
            
            return Ok(createdProduct);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ProductDto>> DeleteById([FromRoute] int id)
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
        public async Task<ActionResult<ProductDto>> Update([FromRoute] int id, [FromBody] UpdatePoductDto produto)
        {
            try
            {
                var updatedProduct = await _repository.Update(id, produto);
                return Ok(updatedProduct);
            } 
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
