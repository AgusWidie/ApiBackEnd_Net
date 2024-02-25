using APIClinic.CacheList;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class JobListController : ControllerBase
    {
        private readonly ILogger<JobListController> _logger;
        private readonly IMasterJobList _masterJobListService;
        private readonly ITransactionJobList _transactionJobListService;

        public JobListController(ILogger<JobListController> logger, IMasterJobList masterJobListService, ITransactionJobList transactionJobListService)
        {
            _logger = logger;
            _masterJobListService = masterJobListService;
            _transactionJobListService = transactionJobListService;
        }

        [Route("StartThreadJobMasterList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> StartThreadJobMasterList(CancellationToken cancellationToken = default)
        {
            _masterJobListService.ThreadJobMasterStart();
            return Ok();
        }

        [Route("StopThreadJobMasterList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> StopThreadJobMasterList(CancellationToken cancellationToken = default)
        {
            _masterJobListService.ThreadJobMasterStop();
            return Ok();
        }

        [Route("StartThreadJobTransactionList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> StartThreadJobTransactionList(CancellationToken cancellationToken = default)
        {
            _transactionJobListService.ThreadJobTransactionStart();
            return Ok();
        }

        [Route("StopThreadJobTransactionList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> StopThreadJobTransactionList(CancellationToken cancellationToken = default)
        {
            _transactionJobListService.ThreadJobTransactionStop();
            return Ok();
        }
    }
}
