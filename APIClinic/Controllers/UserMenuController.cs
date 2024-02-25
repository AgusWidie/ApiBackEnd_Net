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
    public class UserMenuController : ControllerBase
    {
        private readonly ILogger<UserMenuController> _logger;
        private readonly IUserMenuService _userMenuService;

        public UserMenuController(ILogger<UserMenuController> logger, IUserMenuService userMenuService)
        {
            _logger = logger;
            _userMenuService = userMenuService;
        }

        [Route("GetUserMenuParent")]
        [HttpGet]
        public async Task<ActionResult<UserMenuParentResponse>> GetUserMenuParent([FromQuery] UserMenuParentRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userMenuService.GetUserMenuParent(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get user menu parent."));
            }
            return Ok(ResponseHelper<UserMenuParentResponse>.CreateData("Successfully get user menu parent.", result));
        }

        [Route("GetUserMenu")]
        [HttpGet]
        public async Task<ActionResult<UserMenuResponse>> GetUserMenu([FromQuery] UserMenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userMenuService.GetUserMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get user menu."));
            }
            return Ok(ResponseHelper<UserMenuResponse>.CreateData("Successfully get user menu.", result));
        }

        [Route("GetCheckUserMenu")]
        [HttpGet]
        public async Task<ActionResult<CheckUserMenuResponse>> GetCheckUserMenu([FromQuery] CheckUserMenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _userMenuService.GetCheckUserMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get check user menu."));
            }
            return Ok(ResponseHelper<CheckUserMenuResponse>.CreateData("Successfully get check user menu.", result));
        }
    }
}
