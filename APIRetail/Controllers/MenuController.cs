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
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenu _menuService;

        public MenuController(ILogger<MenuController> logger, IMenu menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        [Route("GetMenu")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<MenuResponse>> GetMenu([FromQuery] MenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _menuService.GetMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get menu."));
            }
            return Ok(ResponseHelper<MenuResponse>.Create("Successfully get menu.", result));
        }

        [Route("CreateMenu")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<MenuResponse>> CreateMenu([FromBody] MenuAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _menuService.CreateMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create menu."));
            }
            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<MenuResponse>.Create("Data Menu Already Exist.", result));
            }
            return Ok(ResponseHelper<MenuResponse>.Create("Successfully create menu.", result));
        }

        [Route("UpdateMenu")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<MenuResponse>> UpdateMenu([FromBody] MenuUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _menuService.UpdateMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update menu."));
            }
            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<MenuResponse>.Create("Data Menu Already Exist.", result));
            }
            return Ok(ResponseHelper<MenuResponse>.Create("Successfully update menu.", result));
        }
    }
}
