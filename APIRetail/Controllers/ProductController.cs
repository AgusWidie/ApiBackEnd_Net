using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProduct _productService;

        public ProductController(ILogger<ProductController> logger, IProduct productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [Route("GetProduct")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProductResponse>> GetProduct([FromQuery] ProductRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _productService.GetProduct(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get product."));
            }
            return Ok(ResponseHelper<ProductResponse>.Create("Successfully get product.", result));
        }

        [Route("CreateProduct")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProductResponse>> CreateProduct([FromBody] ProductAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _productService.CreateProduct(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create product."));
            }
            return Ok(ResponseHelper<ProductResponse>.Create("Successfully create product.", result));
        }

        [Route("UpdateProduct")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProductResponse>> UpdateProduct([FromBody] ProductUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _productService.UpdateProduct(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update product."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProductResponse>.Create("Data Product Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<ProductResponse>.Create("Successfully update product.", result));
        }
    }
}
