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
    public class ProfilController : ControllerBase
    {
        private readonly ILogger<ProfilController> _logger;
        private readonly IProfilService _profilService;

        public ProfilController(ILogger<ProfilController> logger, IProfilService profilService)
        {
            _logger = logger;
            _profilService = profilService;
        }

        [Route("GetProfil")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilResponse>> GetProfil([FromQuery] ProfilSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.GetProfil(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get profil."));
            }
            return Ok(ResponseHelper<ProfilResponse>.Create("Successfully get profil.", param.Page, param.PageSize, result));
        }

        [Route("CreateProfil")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilResponse>> CreateProfil([FromBody] ProfilRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.CreateProfil(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create profil."));
            }
            return Ok(ResponseHelper<ProfilResponse>.Create("Successfully create profil.", result.First()));
        }

        [Route("UpdateProfil")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilResponse>> UpdateProfil([FromBody] ProfilRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.UpdateProfil(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update profil."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProfilResponse>.Create("Profil Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ProfilResponse>.Create("Successfully update profil.", result.First()));
        }
    }
}
