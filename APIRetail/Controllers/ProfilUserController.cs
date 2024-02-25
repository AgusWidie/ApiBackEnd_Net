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
    public class ProfilUserController : ControllerBase
    {
        private readonly ILogger<ProfilUserController> _logger;
        private readonly IProfilUser _profilService;

        public ProfilUserController(ILogger<ProfilUserController> logger, IProfilUser profilUserService)
        {
            _logger = logger;
            _profilService = profilUserService;
        }

        [Route("GetProfilUser")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilUserResponse>> GetProfilUser([FromQuery] ProfilUserRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.GetProfilUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get profil user."));
            }
            return Ok(ResponseHelper<ProfilUserResponse>.CreatePaging("Successfully get profil user.", param.TotalPageSize, param.Page, param.PageSize, result));
        }

        [Route("CreateProfilUser")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilUserResponse>> CreateProfilUser([FromBody] ProfilUserAddRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.CreateProfilUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create profil user."));
            }
            return Ok(ResponseHelper<ProfilUserResponse>.Create("Successfully create profil user.", result));
        }

        [Route("UpdateProfilUser")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<ProfilUserResponse>> UpdateProfilUser([FromBody] ProfilUserUpdateRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _profilService.UpdateProfilUser(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update profil user."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<ProfilUserResponse>.Create("Profil User Id : " + param.Id.ToString() + " Not Found.", result));
            }
            return Ok(ResponseHelper<ProfilUserResponse>.Create("Successfully update profil user.", result));
        }
    }
}
