using APIRetail.CacheList;
using APIRetail.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionJobController : ControllerBase
    {
        private readonly ILogger<TransactionJobController> _logger;
        private readonly ITransactionJobList _transJobService;

        public TransactionJobController(ILogger<TransactionJobController> logger, ITransactionJobList transJobService)
        {
            _logger = logger;
            _transJobService = transJobService;
        }

        [Route("ExecuteTransJobList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> ExecuteTransJobList()
        {
            _transJobService.ThreadJobTransactionStart();
            return Ok(ResponseHelper.Create("Successfully execute transaction job list."));
        }

        [Route("StopTransactionJobList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> StopTransactionJobList()
        {
            _transJobService.ThreadJobTransactionStop();
            return Ok(ResponseHelper.Create("Successfully stop transaction job list."));
        }
    }
}
