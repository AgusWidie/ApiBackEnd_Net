using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BranchController : ControllerBase
    {
        private readonly ILogger<BranchController> _logger;
        private readonly IBranchService _branchService;

        public BranchController(ILogger<BranchController> logger, IBranchService branchService)
        {
            _logger = logger;
            _branchService = branchService;
        }

        [Route("GetBranch")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<BranchResponse>> GetBranch([FromQuery] BranchSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _branchService.GetBranch(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get branch."));
            }
            return Ok(ResponseHelper<BranchResponse>.Create("Successfully get branch.", param.Page, param.PageSize, result));
        }

        [Route("CreateBranch")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<BranchResponse>> CreateBranch([FromBody] BranchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _branchService.CreateBranch(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create branch."));
            }
            return Ok(ResponseHelper<BranchResponse>.Create("Successfully create branch.", result.First()));
        }

        [Route("UpdateBranch")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<BranchResponse>> UpdateBranch([FromBody] BranchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _branchService.UpdateBranch(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update branch."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<BranchResponse>.Create("Data Branch Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<BranchResponse>.Create("Successfully update branch.", result.First()));
        }
    }
}
