using Core.DTOs;
using Core.Entities;
using Core.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers
{
    [ApiController]
    [Route("products/events")]
    public class ProductEventsController : Controller
    {
        private readonly IProductEventRepository _repository;

        public ProductEventsController(IProductEventRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductEvent>> GetAll()
        {
            var events = _repository.GetAll();
            return Ok(events);
        }
    }
}
