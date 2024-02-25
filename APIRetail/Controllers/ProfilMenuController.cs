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
    public class ProfilMenuController : ControllerBase
    {
        private readonly ILogger<ProfilMenuController> _logger;
        private readonly IProfilMenu _profilMenuService;

        public ProfilMenuController(ILogger<ProfilMenuController> logger, IProfilMenu profilMenuService)
        {
            _logger = logger;
            _profilMenuService = profilMenuService;
        }

        [Route("GetProfilMenu")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilMenuResponse>> GetProfilMenu([FromQuery] ProfilMenuRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilMenuService.GetProfilMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get profil menu."));
            }
            return Ok(ResponseHelper<ProfilMenuResponse>.CreatePaging("Successfully get profil menu.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateProfilMenu")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilMenuResponse>> CreateProfilMenu([FromBody] ProfilMenuAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilMenuService.CreateProfilMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create profil menu."));
            }
            return Ok(ResponseHelper<ProfilMenuResponse>.Create("Successfully create profil menu.", result));
        }

        [Route("UpdateProfilMenu")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilMenuResponse>> UpdateProfilMenu([FromBody] ProfilMenuUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilMenuService.UpdateProfilMenu(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update profil menu."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProfilMenuResponse>.Create("Profil Menu Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<ProfilMenuResponse>.Create("Successfully update profil menu.", result));
        }
    }
}
