using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        public readonly IConfiguration _configuration;
        private readonly ILogin _loginService;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration, ILogin loginService)
        {
            _logger = logger;
            _configuration = configuration;
            _loginService = loginService;
        }

        [Route("LoginUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest param, CancellationToken cancellationToken = default)
        {
            if (param.ApiKey == null || param.ApiKey == "")
            {
                return BadRequest(ResponseHelper.CreateError(401, "Unauthorize login user."));
            }

            if (param.ApiKey != _configuration.GetValue<string>("Jwt:ApiKey"))
            {
                return BadRequest(ResponseHelper.CreateError(401, "Unauthorize login user."));
            }

            var result = await _loginService.LoginUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request login user."));
            }
            return Ok(ResponseHelper<LoginResponse>.Create("Successfully login user.", result.First()));
        }

        [Route("ChangePasswordUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<UsersResponse>> ChangePasswordUser([FromBody] ChangePasswordRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _loginService.ChangePasswordUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request change password user."));
            }
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully change password user.", result.First()));
        }
    }
}
