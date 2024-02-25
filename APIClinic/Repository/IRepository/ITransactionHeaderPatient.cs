using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ITransactionHeaderPatient
    {
        Task<IEnumerable<TransactionHeaderPatientResponse>> GetTrPatientRegistration(TransactionHeaderPatientSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<TransactionHeaderPatientResponse>> CreateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<TransactionHeaderPatientResponse>> UpdateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken = default);
    }
}
