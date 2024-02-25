using APIRetail.Helper;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        public readonly IConfiguration _configuration;
        private readonly ILogin _loginService;

        public LoginController(ILogger<LoginController> logger, ILogin loginService, IConfiguration configuration)
        {
            _logger = logger;
            _loginService = loginService;
            _configuration = configuration;
        }

        [Route("LoginUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<LoginResponse>> LoginUser([FromBody] LoginRequest param, CancellationToken cancellationToken = default)
        {
            if(param.ApiKey == null || param.ApiKey == "") {
                return BadRequest(ResponseHelper.CreateError(401, "Unauthorize login user."));
            }

            if(param.ApiKey != _configuration.GetValue<string>("Jwt:ApiKey")) {
                return BadRequest(ResponseHelper.CreateError(401, "Unauthorize login user."));
            }

            var result = await _loginService.LoginUser(param, cancellationToken);
            if (result == null) {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request login user."));
            }
            return Ok(ResponseHelper<LoginResponse>.Create("Successfully login user.", result));
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
            return Ok(ResponseHelper<UsersResponse>.Create("Successfully change password user.", result));
        }
    }
}
