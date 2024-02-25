using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BranchController : ControllerBase
    {

        private readonly ILogger<BranchController> _logger;
        private readonly IBranch _branchService;

        public BranchController(ILogger<BranchController> logger, IBranch branchService)
        {
            _logger = logger;
            _branchService = branchService;
        }

        [Route("GetBranch")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<BranchResponse>> GetBranch([FromQuery] BranchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _branchService.GetBranch(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get branch."));
            }
            return Ok(ResponseHelper<BranchResponse>.CreatePaging("Successfully get branch.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateBranch")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<BranchResponse>> CreateBranch([FromBody] BranchAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _branchService.CreateBranch(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create branch."));
            }
            return Ok(ResponseHelper<BranchResponse>.Create("Successfully create branch.", result));
        }

        [Route("UpdateBranch")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<BranchResponse>> UpdateBranch([FromBody] BranchUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _branchService.UpdateBranch(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update branch."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<BranchResponse>.Create("Data Branch Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<BranchResponse>.Create("Successfully update branch.", result));
        }
    }
}