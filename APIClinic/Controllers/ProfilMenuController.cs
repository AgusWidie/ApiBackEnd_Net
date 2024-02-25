using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfilMenuController : ControllerBase
    {
        private readonly ILogger<ProfilMenuController> _logger;
        private readonly IProfilMenuService _profilMenuService;

        public ProfilMenuController(ILogger<ProfilMenuController> logger, IProfilMenuService profilMenuService)
        {
            _logger = logger;
            _profilMenuService = profilMenuService;
        }

        [Route("GetProfilMenu")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilMenuResponse>> GetProfilMenu([FromQuery] ProfilMenuSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilMenuService.GetProfilMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get profil menu."));
            }
            return Ok(ResponseHelper<ProfilMenuResponse>.Create("Successfully get profil menu.", param.Page, param.PageSize, result));
        }

        [Route("CreateProfilMenu")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilMenuResponse>> CreateProfilMenu([FromBody] ProfilMenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilMenuService.CreateProfilMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create profil menu."));
            }
            return Ok(ResponseHelper<ProfilMenuResponse>.Create("Successfully create profil menu.", result.First()));
        }

        [Route("UpdateProfilMenu")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilMenuResponse>> UpdateProfilMenu([FromBody] ProfilMenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilMenuService.UpdateProfilMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update profil menu."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProfilMenuResponse>.Create("Profil Menu Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ProfilMenuResponse>.Create("Successfully update profil menu.", result.First()));
        }
    }
}
