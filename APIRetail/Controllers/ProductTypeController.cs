using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductTypeController : ControllerBase
    {
        private readonly ILogger<ProductTypeController> _logger;
        private readonly IProductType _productTypeService;

        public ProductTypeController(ILogger<ProductTypeController> logger, IProductType productTypeService)
        {
            _logger = logger;
            _productTypeService = productTypeService;
        }

        [Route("GetProductType")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProductTypeResponse>> GetProductType([FromQuery] ProductTypeRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _productTypeService.GetProductType(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get product type."));
            }
            return Ok(ResponseHelper<ProductTypeResponse>.CreatePaging("Successfully get product type.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateProductType")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProductTypeResponse>> CreateProductType([FromBody] ProductTypeAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _productTypeService.CreateProductType(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create product type."));
            }
            return Ok(ResponseHelper<ProductTypeResponse>.Create("Successfully create product type.", result));
        }

        [Route("UpdateProductType")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProductTypeResponse>> UpdateProductType([FromBody] ProductTypeUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _productTypeService.UpdateProductType(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update product type."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProductTypeResponse>.Create("Data Product Type Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<ProductTypeResponse>.Create("Successfully update product type.", result));
        }
    }
}
