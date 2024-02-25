using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CryptoController : ControllerBase
    {
        private readonly ILogger<CryptoController> _logger;
        private readonly ICrypto _cryptoService;

        public CryptoController(ILogger<CryptoController> logger, ICrypto cryptoService)
        {
            _logger = logger;
            _cryptoService = cryptoService;
        }

        [Route("GetMD5Encryption")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetMD5Encryption([FromQuery] InputCrypto param, CancellationToken cancellationToken = default)
        {
            var result = _cryptoService.MD5Encryption(param);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get MD5 Encrypt."));
            }
            return Ok(result);
        }

        [Route("GetMD5Decrypt")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetMD5Decrypt([FromQuery] InputCrypto param, CancellationToken cancellationToken = default)
        {
            var result = _cryptoService.MD5Decrypt(param);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get MD5 Decrypt."));
            }
            return Ok(result);
        }
    }
}
