using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ITransactionLab
    {
        Task<IEnumerable<TransactionHeaderPatientLabResponse>> GetTrPatientLab(TransactionHeaderPatientLabSearchRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<TransactionHeaderPatientLabResponse>> CreateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken = default);
        Task<IEnumerable<TransactionHeaderPatientLabResponse>> UpdateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken = default);
    }
}
