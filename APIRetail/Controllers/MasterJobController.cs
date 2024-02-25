using APIRetail.CacheList;
using APIRetail.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class MasterJobController : ControllerBase
    {
        private readonly ILogger<MasterJobController> _logger;
        private readonly IMasterJobList _masterJobService;

        public MasterJobController(ILogger<MasterJobController> logger, IMasterJobList masterJobService)
        {
            _logger = logger;
            _masterJobService = masterJobService;
        }

        [Route("ExecuteMasterJobList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> ExecuteMasterJobList()
        {
            _masterJobService.ThreadJobMasterStart();
            return Ok(ResponseHelper.Create("Successfully execute master job list."));
        }

        [Route("StopMasterJobList")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult> StopMasterJobList()
        {
            _masterJobService.ThreadJobMasterStop();
            return Ok(ResponseHelper.Create("Successfully stop master job list."));
        }
    }
}
