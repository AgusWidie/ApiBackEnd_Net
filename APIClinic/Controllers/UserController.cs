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
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        [Route("GetUser")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> GetUser([FromQuery] UserSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userService.GetUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get users."));
            }
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully get users.", param.Page, param.PageSize, result));
        }

        [Route("CreateUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> CreateUser([FromBody] UserRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userService.CreateUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create user."));
            }
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully create user.", result.First()));
        }

        [Route("UpdateUser")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> UpdateUser([FromBody] UserRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userService.UpdateUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update user."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<UsersResponse>.Create("User Id : " + param.Id.ToString(), result.First()));
            }
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully update user.", result.First()));
        }
    }
}
