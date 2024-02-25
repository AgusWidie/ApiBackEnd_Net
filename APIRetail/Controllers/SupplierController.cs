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
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplier _supplierService;

        public SupplierController(ILogger<SupplierController> logger, ISupplier supplierService)
        {
            _logger = logger;
            _supplierService = supplierService;
        }

        [Route("GetSupplier")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SupplierResponse>> GetSupplier([FromQuery] SupplierRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _supplierService.GetSupplier(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get supplier."));
            }
            return Ok(ResponseHelper<SupplierResponse>.CreatePaging("Successfully get supplier.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateSupplier")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<SupplierResponse>> CreateSupplier([FromBody] SupplierAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _supplierService.CreateSupplier(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create supplier."));
            }
            return Ok(ResponseHelper<SupplierResponse>.Create("Successfully create supplier.", result));
        }

        [Route("UpdateSupplier")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<SupplierResponse>> UpdateSupplier([FromBody] SupplierUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _supplierService.UpdateSupplier(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update supplier."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<SupplierResponse>.Create("Supplier Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<SupplierResponse>.Create("Successfully update supplier.", result));
        }
    }
}
