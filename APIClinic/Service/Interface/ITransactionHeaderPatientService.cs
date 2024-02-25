using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface ITransactionHeaderPatientService
    {
        Task<List<TransactionHeaderPatientResponse>> GetTrPatientRegistration(TransactionHeaderPatientSearchRequest param, CancellationToken cancellationToken);
        Task<List<TransactionHeaderPatientResponse>> CreateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken);
        Task<List<TransactionHeaderPatientResponse>> UpdateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken);
        Task<List<TransactionDetailPatientResponse>> GetTrDetailPatientRegistration(TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken);
    }
}
