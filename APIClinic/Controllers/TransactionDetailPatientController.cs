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
    public class TransactionDetailPatientController : ControllerBase
    {
        private readonly ILogger<TransactionDetailPatientController> _logger;
        private readonly ITransactionHeaderPatientService _transactionPatientService;

        public TransactionDetailPatientController(ILogger<TransactionDetailPatientController> logger, ITransactionHeaderPatientService transactionPatientService)
        {
            _logger = logger;
            _transactionPatientService = transactionPatientService;
        }

        [Route("GetTransactionDetailPatient")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionDetailPatientResponse>> GetTransactionDetailPatient([FromQuery] TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _transactionPatientService.GetTrDetailPatientRegistration(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get branch."));
            }
            return Ok(ResponseHelper<TransactionDetailPatientResponse>.CreateData("Successfully get transaction detail patient.", result));
        }
    }
}
