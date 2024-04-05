using Flyr.Infrastructure.Model;
using FlySupermarket.Infrastructure.Context;
using Microsoft.AspNetCore.Mvc;

namespace FlySupermarket.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly FlyrContext _flyrContext;

        public ProductsController(
            ILogger<ProductsController> logger,
            FlyrContext flyrContext)
        {
            _logger = logger;
            _flyrContext = flyrContext;
        }

        [HttpGet]
        public ActionResult<List<Product>> GetProducts()
        {
            List<Product> products = _flyrContext.GetProducts();
            return Ok(products);
        }
    }
}
