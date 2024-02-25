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
    public class MenuController : ControllerBase
    {
        private readonly ILogger<MenuController> _logger;
        private readonly IMenuService _menuService;

        public MenuController(ILogger<MenuController> logger, IMenuService menuService)
        {
            _logger = logger;
            _menuService = menuService;
        }

        [Route("GetMenu")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<MenuResponse>> GetMenu([FromQuery] MenuSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _menuService.GetMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get menu."));
            }
            return Ok(ResponseHelper<MenuResponse>.Create("Successfully get menu.", param.Page, param.PageSize, result));
        }

        [Route("CreateMenu")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<MenuResponse>> CreateMenu([FromBody] MenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _menuService.CreateMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create menu."));
            }
            return Ok(ResponseHelper<MenuResponse>.Create("Successfully create menu.", result.First()));
        }

        [Route("UpdateMenu")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<MenuResponse>> UpdateMenu([FromBody] MenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _menuService.UpdateMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update menu."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<MenuResponse>.Create("Menu Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<MenuResponse>.Create("Successfully update menu.", result.First()));
        }
    }
}
