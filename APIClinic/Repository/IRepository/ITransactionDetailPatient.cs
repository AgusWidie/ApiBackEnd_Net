using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Repository.IRepository
{
    public interface ITransactionDetailPatient
    {
        Task<IEnumerable<TransactionDetailPatientResponse>> GetTrDetailPatientRegistration(TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken = default);
    }
}
