using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface ITransactionHeaderPatientLabService
    {
        Task<List<TransactionHeaderPatientLabResponse>> GetTrPatientLabRegistration(TransactionHeaderPatientLabSearchRequest param, CancellationToken cancellationToken);
        Task<List<TransactionHeaderPatientLabResponse>> CreateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken);
        Task<List<TransactionHeaderPatientLabResponse>> UpdateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken);
    }
}
