using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class TransactionDetailPatientRegistrationService : ITransactionDetailPatientRegistrationService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ITransactionHeaderPatient _transHeaderPatientRepo;
        public readonly ITransactionDetailPatient _transDetailPatientRepo;

        public TransactionDetailPatientRegistrationService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ITransactionHeaderPatient transHeaderPatientRepo, ITransactionDetailPatient transDetailPatientRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _transHeaderPatientRepo = transHeaderPatientRepo;
            _transDetailPatientRepo = transDetailPatientRepo;
        }

        public async Task<List<TransactionDetailPatientResponse>> GetTrDetailPatientRegistration(TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listTransactionDetailPatient.Count() > 0)
                {
                    var resultList = GeneralList._listTransactionDetailPatient.Where(x => x.TransactionNo == param.TransactionNo);
                    return resultList.ToList();

                }
                else
                {
                    var resultList = await _transDetailPatientRepo.GetTrDetailPatientRegistration(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
