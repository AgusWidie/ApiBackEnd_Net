using APIClinic.Helper;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIClinic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHeaderPatientlabController : ControllerBase
    {
        private readonly ILogger<TransactionHeaderPatientlabController> _logger;
        private readonly ITransactionHeaderPatientLabService _transactionHeaderPatientLabService;

        public TransactionHeaderPatientlabController(ILogger<TransactionHeaderPatientlabController> logger, ITransactionHeaderPatientLabService transactionHeaderPatientLabService)
        {
            _logger = logger;
            _transactionHeaderPatientLabService = transactionHeaderPatientLabService;
        }

        [Route("GetTransactionHeaderPatientLab")]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionHeaderPatientLabResponse>> GetTransactionHeaderPatientLab([FromQuery] TransactionHeaderPatientLabSearchRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _transactionHeaderPatientLabService.GetTrPatientLabRegistration(param, cancellationToken);
            if (result == null)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request get transaction header patient lab."));
            }
            return Ok(ResponseHelper<TransactionHeaderPatientLabResponse>.Create("Successfully get transaction header patient lab.", param.Page, param.PageSize, result));
        }

        [Route("CreateTransactionHeaderPatientLab")]
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionHeaderPatientLabResponse>> CreateTransactionHeaderPatientLab([FromBody] TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _transactionHeaderPatientLabService.CreateTrHeaderPatientLab(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create transaction header patient lab."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<TransactionHeaderPatientLabResponse>.Create("Stock Not Found. Pleaase Check.", result.First()));
            }
            return Ok(ResponseHelper<TransactionHeaderPatientLabResponse>.Create("Successfully create transaction header patient lab.", result.First()));
        }

        [Route("UpdateTransactionHeaderPatientLab")]
        [HttpPut]
        [Produces("application/json")]
        public async Task<ActionResult<TransactionHeaderPatientLabResponse>> UpdateTransactionHeaderPatientLab([FromBody] TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken = default)
        {
            var result = await _transactionHeaderPatientLabService.UpdateTrHeaderPatientLab(param, cancellationToken);
            if (result == null || result.Count() <= 0)
            {
                return BadRequest(ResponseHelper.CreateError(400, "Bad request create transaction header patient lab."));
            }

            if (result.Count() <= 0)
            {
                return Ok(ResponseHelper<TransactionHeaderPatientLabResponse>.Create("Stock Not Found. Pleaase Check.", result.First()));
            }
            return Ok(ResponseHelper<TransactionHeaderPatientLabResponse>.Create("Successfully update transaction header patient lab.", result.First()));
        }
    }
}
