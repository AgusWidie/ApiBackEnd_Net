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
    public class PurchaseOrderController : ControllerBase
    {
        private readonly ILogger<PurchaseOrderController> _logger;
        private readonly IPurchaseOrder _purchaseOrderService;

        public PurchaseOrderController(ILogger<PurchaseOrderController> logger, IPurchaseOrder purchaseOrderService)
        {
            _logger = logger;
            _purchaseOrderService = purchaseOrderService;
        }

        [Route("GetPurchaseOrderHeader")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<PurchaseOrderResponse>> GetPurchaseOrder([FromQuery] PurchaseOrderRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _purchaseOrderService.GetPurchaseOrderHeader(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get purchase order header."));
            }
            return Ok(ResponseHelper<PurchaseOrderResponse>.CreatePaging("Successfully get purchase order header.", param.TotalPageSize, param.Page, param.PageSize, result));

        }

        [Route("GetPurchaseOrderDetail")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<PurchaseOrderDetailResponse>> GetPurchaseOrderDetail([FromQuery] PurchaseOrderRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _purchaseOrderService.GetPurchaseOrderDetail(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get purchase order detail."));
            }
            return Ok(ResponseHelper<PurchaseOrderDetailResponse>.Create("Successfully get purchase order detail.", result));
        }

        [Route("CreatePurchaseOrder")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<PurchaseOrderResponse>> CreatePurchaseOrder([FromBody] PurchaseOrderHeaderAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _purchaseOrderService.CreatePurchaseOrder(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create purchase order."));
            }
            return Ok(ResponseHelper<PurchaseOrderResponse>.Create("Successfully create purchase order.", result));
        }

        [Route("UpdatePurchaseOrder")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<PurchaseOrderResponse>> UpdatePurchaseOrder([FromBody] PurchaseOrderHeaderUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _purchaseOrderService.UpdatePuchaseOrder(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update purchase order."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<PurchaseOrderResponse>.Create("Purchase Order Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<PurchaseOrderResponse>.Create("Successfully update purchase order.", result));
        }
    }
}
