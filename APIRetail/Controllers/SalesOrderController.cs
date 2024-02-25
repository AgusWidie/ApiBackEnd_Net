using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ILogger<SalesOrderController> _logger;
        private readonly ISalesOrder _salesOrderService;

        public SalesOrderController(ILogger<SalesOrderController> logger, ISalesOrder salesOrderService)
        {
            _logger = logger;
            _salesOrderService = salesOrderService;
        }

        [Route("GetSalesOrderHeader")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SalesOrderResponse>> GetSalesorder([FromQuery] SalesOrderRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _salesOrderService.GetSalesOrderHeader(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get sales order header."));
            }
            return Ok(ResponseHelper<SalesOrderResponse>.CreatePaging("Successfully get sales order header.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("GetSalesOrderDetail")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<SalesOrderDetailResponse>> GetSalesOrderDetail([FromQuery] SalesOrderRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _salesOrderService.GetSalesOrderDetail(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get sales order detail."));
            }
            return Ok(ResponseHelper<SalesOrderDetailResponse>.Create("Successfully get sales order detail.", result));
        }

        [Route("CreateSalesOrder")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<SalesOrderResponse>> CreateSalesOrder([FromBody] SalesOrderHeaderAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _salesOrderService.CreateSalesOrder(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create sales order."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<SalesOrderResponse>.Create("Create sales order data not found or stock not found.", result));
            }
            return Ok(ResponseHelper<SalesOrderResponse>.Create("Successfully create sales order.", result));
        }

        [Route("UpdateSalesOrder")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<SalesOrderResponse>> UpdateSalesOrder([FromBody] SalesOrderHeaderUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _salesOrderService.UpdateSalesOrder(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update sales order."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<SalesOrderResponse>.Create("Create sales order data not found or stock not found.", result));
            }
            return Ok(ResponseHelper<SalesOrderResponse>.Create("Successfully update sales order.", result));
        }
    }
}
