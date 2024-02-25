using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface ITransactionDetailPatientRegistrationService
    {
        Task<List<TransactionDetailPatientResponse>> GetTrDetailPatientRegistration(TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken);
    }
}
