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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUser _userService;

        public UserController(ILogger<UserController> logger, IUser userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("GetUser")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> GetUser([FromQuery] UsersRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userService.GetUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get users."));
            }
            return Ok(ResponseHelper<UsersResponse>.CreatePaging("Successfully get users.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> CreateUser([FromBody] UsersAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userService.CreateUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create user."));
            }
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully create user.", result));
        }

        [Route("UpdateUser")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> UpdateUser([FromBody] UsersUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UpdateUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update user."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<UsersResponse>.Create("User Id : " + param.Id.ToString(), result));
            }
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully update user.", result));
        }
    }
}
