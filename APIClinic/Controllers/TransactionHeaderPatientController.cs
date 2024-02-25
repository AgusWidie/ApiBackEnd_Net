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
    public class TransactionHeaderPatientController : ControllerBase
    {
        private readonly ILogger<TransactionHeaderPatientController> _logger;
        private readonly ITransactionHeaderPatientService _transactionHeaderPatientService;

        public TransactionHeaderPatientController(ILogger<TransactionHeaderPatientController> logger, ITransactionHeaderPatientService transactionHeaderPatientService)
        {
            _logger = logger;
            _transactionHeaderPatientService = transactionHeaderPatientService;
        }

        [Route("GetTransactionHeaderPatient")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionHeaderPatientResponse>> GetTransactionHeaderPatient([FromQuery] TransactionHeaderPatientSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _transactionHeaderPatientService.GetTrPatientRegistration(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get ransaction header patient."));
            }
            return Ok(ResponseHelper<TransactionHeaderPatientResponse>.Create("Successfully get transaction header patient.", param.Page, param.PageSize, result));
        }

        [Route("CreateTransactionHeaderPatient")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionHeaderPatientResponse>> CreateTransactionHeaderPatient([FromBody] TransactionHeaderPatientRequest param, CancellationToken cancellationToken = default)
        {
            if (param.PaymentType != "BPJS" && param.PaymentType != "Incurance")
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create transaction payment type invalid."));
            }

            var result = await _transactionHeaderPatientService.CreateTrHeaderPatient(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create transaction header patient."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<TransactionHeaderPatientResponse>.Create("Stock Not Found. Pleaase Check.", result.First()));
            }
            return Ok(ResponseHelper<TransactionHeaderPatientResponse>.Create("Successfully create transaction header patient.", result.First()));
        }

        [Route("UpdateTransactionHeaderPatient")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionHeaderPatientResponse>> UpdateTransactionHeaderPatient([FromBody] TransactionHeaderPatientRequest param, CancellationToken cancellationToken = default)
        {
            if (param.PaymentType != "BPJS" && param.PaymentType != "Incurance")
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create transaction payment type invalid."));
            }

            var result = await _transactionHeaderPatientService.UpdateTrHeaderPatient(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request update transaction header patient."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<TransactionHeaderPatientResponse>.Create("Data Transaction Header Patient Id : " + param.Id.ToString() + " Not Found Or Stock Not Found.", result.First()));
            }
            return Ok(ResponseHelper<TransactionHeaderPatientResponse>.Create("Successfully update transaction header patient.", result.First()));
        }
    }
}
