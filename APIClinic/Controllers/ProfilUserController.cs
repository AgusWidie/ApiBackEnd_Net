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
    public class ProfilUserController : ControllerBase
    {
        private readonly ILogger<ProfilUserController> _logger;
        private readonly IProfilUserService _profilService;

        public ProfilUserController(ILogger<ProfilUserController> logger, IProfilUserService profilUserService)
        {
            _logger = logger;
            _profilService = profilUserService;
        }

        [Route("GetProfilUser")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilUserResponse>> GetProfilUser([FromQuery] ProfilUserSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.GetProfilUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get profil user."));
            }
            return Ok(ResponseHelper<ProfilUserResponse>.Create("Successfully get profil user.", param.Page, param.PageSize, result));
        }

        [Route("CreateProfilUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilUserResponse>> CreateProfilUser([FromBody] ProfilUserRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.CreateProfilUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create profil user."));
            }
            return Ok(ResponseHelper<ProfilUserResponse>.Create("Successfully create profil user.", result.First()));
        }

        [Route("UpdateProfilUser")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilUserResponse>> UpdateProfilUser([FromBody] ProfilUserRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.UpdateProfilUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update profil user."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProfilUserResponse>.Create("Profil User Id : " + param.Id.ToString() + " Not Found.", result.First()));
            }
            return Ok(ResponseHelper<ProfilUserResponse>.Create("Successfully update profil user.", result.First()));
        }
    }
}
