using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class TransactionDetailPatientRegistrationRepository : ITransactionDetailPatient
    {

        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public TransactionDetailPatientRegistrationRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<TransactionDetailPatientResponse>> GetTrDetailPatientRegistration(TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionDetailPatientResponse>? transactionDetailList = null;
            try
            {
                transactionDetailList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join tr in _context.TransactionHeaderPatient on branch.Id equals tr.BranchId
                                         join td in _context.TransactionDetailPatient on tr.TransactionNo equals td.TransactionNo
                                         join drug in _context.Drug on td.DrugId equals drug.Id
                                         where td.TransactionNo == param.TransactionNo
                                         select new TransactionDetailPatientResponse
                                         {
                                             Id = td.Id,
                                             TransactionNo = td.TransactionNo,
                                             DrugId = td.DrugId,
                                             DrugName = drug.DrugName,
                                             UnitType = td.UnitType,
                                             Qty = td.Qty,
                                             Price = td.Price,
                                             Subtotal = td.Subtotal,
                                             CreateBy = td.CreateBy,
                                             CreateDate = td.CreateDate,
                                             UpdateBy = td.UpdateBy,
                                             UpdateDate = td.UpdateDate
                                         }).OrderBy(x => x.DrugName).AsNoTracking();

                return transactionDetailList;

            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetTrDetailPatientRegistration";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return transactionDetailList;
            }
        }

    }
}
