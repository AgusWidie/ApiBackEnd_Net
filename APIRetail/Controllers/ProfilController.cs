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
    public class ProfilController : ControllerBase
    {
        private readonly ILogger<ProfilController> _logger;
        private readonly IProfil _profilService;

        public ProfilController(ILogger<ProfilController> logger, IProfil profilService)
        {
            _logger = logger;
            _profilService = profilService;
        }

        [Route("GetProfil")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilResponse>> GetProfil([FromQuery] ProfilRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.GetProfil(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get profil."));
            }
            return Ok(ResponseHelper<ProfilResponse>.Create("Successfully get profil.", result));
        }

        [Route("CreateProfil")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilResponse>> CreateProfil([FromBody] ProfilAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.CreateProfil(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create profil."));
            }
            return Ok(ResponseHelper<ProfilResponse>.Create("Successfully create profil.", result));
        }

        [Route("UpdateProfil")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilResponse>> UpdateProfil([FromBody] ProfilUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.UpdateProfil(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update profil."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProfilResponse>.Create("Profil Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<ProfilResponse>.Create("Successfully update profil.", result));
        }
    }
}
