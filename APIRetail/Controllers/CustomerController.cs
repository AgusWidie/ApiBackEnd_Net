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
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomer _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomer customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [Route("GetCustomer")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<CustomerResponse>> GetCustomer([FromQuery] CustomerRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _customerService.GetCustomer(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get customer."));
            }
            return Ok(ResponseHelper<CustomerResponse>.CreatePaging("Successfully get customer.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateCustomer")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<CustomerResponse>> CreateCustomer([FromBody] CustomerAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _customerService.CreateCustomer(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create customer."));
            }
            return Ok(ResponseHelper<CustomerResponse>.Create("Successfully create customer.", result));
        }

        [Route("UpdateCustomer")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<CustomerResponse>> UpdateCustomer([FromBody] CustomerUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _customerService.UpdateCustomer(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update customer."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<CustomerResponse>.Create("Data Customer Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<CustomerResponse>.Create("Successfully update customer.", result));
        }
    }
}
