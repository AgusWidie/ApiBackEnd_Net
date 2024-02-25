using APIRetail.Helper;
using APIRetail.Models.DTO.Request.Crypto;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIRetail.Controllers
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

        [Route("GetEncryptTripleDESCrypto")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetEncryptTripleDESCrypto([FromQuery] InputCrypto param, CancellationToken cancellationToken = default)
        {
            var result = _cryptoService.MD5Encryption(param);
            return Ok(ResponseHelper.Create(result));
        }

        [Route("GetDecryptTripleDESCrypto")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<string>> GetDecryptTripleDESCrypto([FromQuery] InputCrypto param, CancellationToken cancellationToken = default)
        {
            var result = _cryptoService.MD5Decrypt(param);
            return Ok(ResponseHelper.Create(result));
        }
    }

   
}
