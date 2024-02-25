using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class TransactionHeaderPatientLabRepository : ITransactionLab
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public TransactionHeaderPatientLabRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<TransactionHeaderPatientLabResponse>> GetTrPatientLab(TransactionHeaderPatientLabSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHeaderPatientLabResponse>? transactionHeaderLabList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.PaymentType == "" || param.PaymentType == null)
                {
                    transactionHeaderLabList = (from branch in _context.Branch
                                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                                join pat in _context.PatientRegistrationLab on exa.QueueNo equals pat.QueueNo
                                                join lab in _context.Laboratorium on pat.LaboratoriumId equals lab.Id
                                                join tr in _context.TransactionLab on exa.Id equals tr.ExaminationLabId
                                                where tr.ClinicId == param.ClinicId && tr.BranchId == param.BranchId
                                                      && tr.TransactionDate >= param.TransactionDateFrom && tr.TransactionDate <= param.TransactionDateTo
                                                select new TransactionHeaderPatientLabResponse
                                                {
                                                    Id = exa.Id,
                                                    TransactionNo = tr.TransactionNo,
                                                    TransactionDate = tr.TransactionDate,
                                                    ClinicId = clinic.Id,
                                                    ClinicName = clinic.Name,
                                                    BranchId = branch.Id,
                                                    BranchName = branch.Name,
                                                    ExaminationLabId = exa.Id,
                                                    LaboratoriumId = lab.Id,
                                                    LaboratoriumName = lab.LaboratoriumName,
                                                    QueueNo = exa.QueueNo,
                                                    Ktpno = pat.Ktpno,
                                                    FamilyCardNo = pat.FamilyCardNo,
                                                    Name = pat.Name,
                                                    PaymentType = pat.PaymentType,
                                                    Bpjsno = pat.Bpjsno,
                                                    InsuranceName = pat.InsuranceName,
                                                    InsuranceNo = pat.InsuranceNo,
                                                    CreateBy = exa.CreateBy,
                                                    CreateDate = exa.CreateDate,
                                                    UpdateBy = exa.UpdateBy,
                                                    UpdateDate = exa.UpdateDate
                                                }).OrderBy(x => x.TransactionDate).AsNoTracking().ToList();
                }

                if (param.PaymentType != null && param.PaymentType != "")
                {
                    transactionHeaderLabList = (from branch in _context.Branch
                                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                                join pat in _context.PatientRegistrationLab on exa.QueueNo equals pat.QueueNo
                                                join lab in _context.Laboratorium on pat.LaboratoriumId equals lab.Id
                                                join tr in _context.TransactionLab on exa.Id equals tr.ExaminationLabId
                                                where tr.ClinicId == param.ClinicId && tr.BranchId == param.BranchId
                                                      && tr.TransactionDate >= param.TransactionDateFrom && tr.TransactionDate <= param.TransactionDateTo
                                                      && tr.PaymentType == param.PaymentType
                                                select new TransactionHeaderPatientLabResponse
                                                {
                                                    Id = exa.Id,
                                                    TransactionNo = tr.TransactionNo,
                                                    TransactionDate = tr.TransactionDate,
                                                    ClinicId = clinic.Id,
                                                    ClinicName = clinic.Name,
                                                    BranchId = branch.Id,
                                                    BranchName = branch.Name,
                                                    ExaminationLabId = exa.Id,
                                                    LaboratoriumId = lab.Id,
                                                    LaboratoriumName = lab.LaboratoriumName,
                                                    QueueNo = exa.QueueNo,
                                                    Ktpno = pat.Ktpno,
                                                    FamilyCardNo = pat.FamilyCardNo,
                                                    Name = pat.Name,
                                                    PaymentType = pat.PaymentType,
                                                    Bpjsno = pat.Bpjsno,
                                                    InsuranceName = pat.InsuranceName,
                                                    InsuranceNo = pat.InsuranceNo,
                                                    CreateBy = exa.CreateBy,
                                                    CreateDate = exa.CreateDate,
                                                    UpdateBy = exa.UpdateBy,
                                                    UpdateDate = exa.UpdateDate
                                                }).OrderBy(x => x.TransactionDate).AsNoTracking().ToList();
                }

                var TotalPageSize = Math.Ceiling((decimal)transactionHeaderLabList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = transactionHeaderLabList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetTrPatientLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return transactionHeaderLabList;
            }
        }

        public async Task<IEnumerable<TransactionHeaderPatientLabResponse>> CreateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHeaderPatientLabResponse>? transactionHeaderLabList = null;
            TransactionLab transactionHeaderLabAdd = new TransactionLab();

            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    transactionHeaderLabAdd.ClinicId = param.ClinicId;
                    transactionHeaderLabAdd.BranchId = param.BranchId;
                    transactionHeaderLabAdd.TransactionDate = param.TransactionDate;
                    transactionHeaderLabAdd.TransactionNo = param.TransactionNo;
                    transactionHeaderLabAdd.ExaminationLabId = param.ExaminationLabId;
                    transactionHeaderLabAdd.PaymentType = param.PaymentType;
                    transactionHeaderLabAdd.Bpjsno = param.Bpjsno;
                    transactionHeaderLabAdd.InsuranceName = param.InsuranceName;
                    transactionHeaderLabAdd.InsuranceNo = param.InsuranceNo;
                    transactionHeaderLabAdd.Total = param.Total;
                    transactionHeaderLabAdd.CreateBy = param.CreateBy;
                    transactionHeaderLabAdd.CreateDate = DateTime.Now;
                    _context.TransactionLab.Add(transactionHeaderLabAdd);
                    await _context.SaveChangesAsync();

                    transactionHeaderLabList = (from branch in _context.Branch
                                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                                join pat in _context.PatientRegistrationLab on exa.QueueNo equals pat.QueueNo
                                                join lab in _context.Laboratorium on pat.LaboratoriumId equals lab.Id
                                                join tr in _context.TransactionLab on exa.Id equals tr.ExaminationLabId
                                                where tr.ClinicId == param.ClinicId && tr.BranchId == param.BranchId
                                                      && tr.TransactionDate == param.TransactionDate
                                                      && tr.PaymentType == param.PaymentType && tr.ExaminationLabId == param.ExaminationLabId
                                                select new TransactionHeaderPatientLabResponse
                                                {
                                                    Id = exa.Id,
                                                    TransactionNo = tr.TransactionNo,
                                                    TransactionDate = tr.TransactionDate,
                                                    ClinicId = clinic.Id,
                                                    ClinicName = clinic.Name,
                                                    BranchId = branch.Id,
                                                    BranchName = branch.Name,
                                                    ExaminationLabId = exa.Id,
                                                    LaboratoriumId = lab.Id,
                                                    LaboratoriumName = lab.LaboratoriumName,
                                                    QueueNo = exa.QueueNo,
                                                    Ktpno = pat.Ktpno,
                                                    FamilyCardNo = pat.FamilyCardNo,
                                                    Name = pat.Name,
                                                    PaymentType = pat.PaymentType,
                                                    Bpjsno = pat.Bpjsno,
                                                    InsuranceName = pat.InsuranceName,
                                                    InsuranceNo = pat.InsuranceNo,
                                                    CreateBy = exa.CreateBy,
                                                    CreateDate = exa.CreateDate,
                                                    UpdateBy = exa.UpdateBy,
                                                    UpdateDate = exa.UpdateDate
                                                }).Take(1).AsNoTracking();

                    return transactionHeaderLabList;
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "CreateTrHeaderPatientLab";
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                    return transactionHeaderLabList;
                }
            }

        }

        public async Task<IEnumerable<TransactionHeaderPatientLabResponse>> UpdateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHeaderPatientLabResponse>? transactionHeaderLabList = null;
            try
            {
                var transactionHeaderLabUpdate = await _context.TransactionLab.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (transactionHeaderLabUpdate != null)
                {
                    transactionHeaderLabUpdate.TransactionDate = param.TransactionDate;
                    transactionHeaderLabUpdate.TransactionNo = param.TransactionNo;
                    transactionHeaderLabUpdate.ExaminationLabId = param.ExaminationLabId;
                    transactionHeaderLabUpdate.PaymentType = param.PaymentType;
                    transactionHeaderLabUpdate.Bpjsno = param.Bpjsno;
                    transactionHeaderLabUpdate.InsuranceName = param.InsuranceName;
                    transactionHeaderLabUpdate.InsuranceNo = param.InsuranceNo;
                    transactionHeaderLabUpdate.Total = param.Total;
                    transactionHeaderLabUpdate.CreateBy = param.CreateBy;
                    transactionHeaderLabUpdate.CreateDate = DateTime.Now;
                    _context.TransactionLab.Update(transactionHeaderLabUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    transactionHeaderLabList = (from branch in _context.Branch
                                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                                join pat in _context.PatientRegistrationLab on exa.QueueNo equals pat.QueueNo
                                                join lab in _context.Laboratorium on pat.LaboratoriumId equals lab.Id
                                                join tr in _context.TransactionLab on exa.Id equals tr.ExaminationLabId
                                                where tr.Id == param.Id
                                                select new TransactionHeaderPatientLabResponse
                                                {
                                                    Id = exa.Id,
                                                    TransactionNo = tr.TransactionNo,
                                                    TransactionDate = tr.TransactionDate,
                                                    ClinicId = clinic.Id,
                                                    ClinicName = clinic.Name,
                                                    BranchId = branch.Id,
                                                    BranchName = branch.Name,
                                                    ExaminationLabId = exa.Id,
                                                    LaboratoriumId = lab.Id,
                                                    LaboratoriumName = lab.LaboratoriumName,
                                                    QueueNo = exa.QueueNo,
                                                    Ktpno = pat.Ktpno,
                                                    FamilyCardNo = pat.FamilyCardNo,
                                                    Name = pat.Name,
                                                    PaymentType = pat.PaymentType,
                                                    Bpjsno = pat.Bpjsno,
                                                    InsuranceName = pat.InsuranceName,
                                                    InsuranceNo = pat.InsuranceNo,
                                                    CreateBy = exa.CreateBy,
                                                    CreateDate = exa.CreateDate,
                                                    UpdateBy = exa.UpdateBy,
                                                    UpdateDate = exa.UpdateDate
                                                }).Take(0).AsNoTracking();


                    return transactionHeaderLabList;
                }


                transactionHeaderLabList = (from branch in _context.Branch
                                            join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                            join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                            join pat in _context.PatientRegistrationLab on exa.QueueNo equals pat.QueueNo
                                            join lab in _context.Laboratorium on pat.LaboratoriumId equals lab.Id
                                            join tr in _context.TransactionLab on exa.Id equals tr.ExaminationLabId
                                            where tr.Id == param.Id
                                            select new TransactionHeaderPatientLabResponse
                                            {
                                                Id = exa.Id,
                                                TransactionNo = tr.TransactionNo,
                                                TransactionDate = tr.TransactionDate,
                                                ClinicId = clinic.Id,
                                                ClinicName = clinic.Name,
                                                BranchId = branch.Id,
                                                BranchName = branch.Name,
                                                ExaminationLabId = exa.Id,
                                                LaboratoriumId = lab.Id,
                                                LaboratoriumName = lab.LaboratoriumName,
                                                QueueNo = exa.QueueNo,
                                                Ktpno = pat.Ktpno,
                                                FamilyCardNo = pat.FamilyCardNo,
                                                Name = pat.Name,
                                                PaymentType = pat.PaymentType,
                                                Bpjsno = pat.Bpjsno,
                                                InsuranceName = pat.InsuranceName,
                                                InsuranceNo = pat.InsuranceNo,
                                                CreateBy = exa.CreateBy,
                                                CreateDate = exa.CreateDate,
                                                UpdateBy = exa.UpdateBy,
                                                UpdateDate = exa.UpdateDate
                                            }).Take(1).AsNoTracking();


                return transactionHeaderLabList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateTrHeaderPatientLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return transactionHeaderLabList;
            }

        }
    }
}
