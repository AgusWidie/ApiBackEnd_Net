using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class TransactionHeaderPatientRegistrationRepository : ITransactionHeaderPatient
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public TransactionHeaderPatientRegistrationRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<TransactionHeaderPatientResponse>> GetTrPatientRegistration(TransactionHeaderPatientSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHeaderPatientResponse>? transactionHeaderList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.PaymentType == "" || param.PaymentType == null)
                {
                    transactionHeaderList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                             where tr.ClinicId == param.ClinicId && tr.BranchId == param.BranchId
                                                   && tr.TransactionDate >= param.TransactionDateFrom && tr.TransactionDate <= param.TransactionDateTo
                                             select new TransactionHeaderPatientResponse
                                             {
                                                 Id = exa.Id,
                                                 TransactionNo = tr.TransactionNo,
                                                 TransactionDate = tr.TransactionDate,
                                                 ClinicId = clinic.Id,
                                                 ClinicName = clinic.Name,
                                                 BranchId = branch.Id,
                                                 BranchName = branch.Name,
                                                 DoctorId = doc.Id,
                                                 DoctorName = doc.DoctorName,
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
                    transactionHeaderList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                             where tr.ClinicId == param.ClinicId && tr.BranchId == param.BranchId
                                                   && tr.TransactionDate >= param.TransactionDateFrom && tr.TransactionDate <= param.TransactionDateTo
                                                   && tr.PaymentType == param.PaymentType
                                             select new TransactionHeaderPatientResponse
                                             {
                                                 Id = exa.Id,
                                                 TransactionNo = tr.TransactionNo,
                                                 TransactionDate = tr.TransactionDate,
                                                 ClinicId = clinic.Id,
                                                 ClinicName = clinic.Name,
                                                 BranchId = branch.Id,
                                                 BranchName = branch.Name,
                                                 DoctorId = doc.Id,
                                                 DoctorName = doc.DoctorName,
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

                var TotalPageSize = Math.Ceiling((decimal)transactionHeaderList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = transactionHeaderList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetTrPatientRegistration";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return transactionHeaderList;
            }
        }

        public async Task<IEnumerable<TransactionHeaderPatientResponse>> CreateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHeaderPatientResponse>? transactionHeaderList = null;
            TransactionHeaderPatient transactionHeaderAdd = new TransactionHeaderPatient();

            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    transactionHeaderAdd.ClinicId = param.ClinicId;
                    transactionHeaderAdd.BranchId = param.BranchId;
                    transactionHeaderAdd.TransactionDate = param.TransactionDate;
                    transactionHeaderAdd.TransactionNo = param.TransactionNo;
                    transactionHeaderAdd.ExaminationDoctorId = param.ExaminationDoctorId;
                    transactionHeaderAdd.PaymentType = param.PaymentType;
                    transactionHeaderAdd.Bpjsno = param.Bpjsno;
                    transactionHeaderAdd.InsuranceName = param.InsuranceName;
                    transactionHeaderAdd.InsuranceNo = param.InsuranceNo;
                    transactionHeaderAdd.Total = 0;
                    transactionHeaderAdd.CreateBy = param.CreateBy;
                    transactionHeaderAdd.CreateDate = DateTime.Now;
                    _context.TransactionHeaderPatient.Add(transactionHeaderAdd);
                    await _context.SaveChangesAsync();

                    long? Total = 0;
                    foreach (var dataDetail in param.TransactionDetailPatientRequest)
                    {
                        var checkDrug = await _context.Drug.Where(x => x.Id == dataDetail.DrugId).AsNoTracking().FirstOrDefaultAsync();
                        if (checkDrug != null)
                        {
                            if (checkDrug.Stock < dataDetail.Qty)
                            {
                                transactionHeaderList = (from branch in _context.Branch
                                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                         join doc in _context.Doctor on branch.Id equals doc.BranchId
                                                         join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                                         join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                                         join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                                         where tr.Id == transactionHeaderAdd.Id
                                                         select new TransactionHeaderPatientResponse
                                                         {
                                                             Id = exa.Id,
                                                             TransactionNo = tr.TransactionNo,
                                                             TransactionDate = tr.TransactionDate,
                                                             ClinicId = clinic.Id,
                                                             ClinicName = clinic.Name,
                                                             BranchId = branch.Id,
                                                             BranchName = branch.Name,
                                                             DoctorId = doc.Id,
                                                             DoctorName = doc.DoctorName,
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

                                dbContextTransaction.Rollback();
                                dbContextTransaction.Dispose();
                                return transactionHeaderList;
                            }

                        }
                        else
                        {
                            transactionHeaderList = (from branch in _context.Branch
                                                     join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                     join doc in _context.Doctor on branch.Id equals doc.BranchId
                                                     join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                                     join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                                     join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                                     where tr.Id == transactionHeaderAdd.Id
                                                     select new TransactionHeaderPatientResponse
                                                     {
                                                         Id = exa.Id,
                                                         TransactionNo = tr.TransactionNo,
                                                         TransactionDate = tr.TransactionDate,
                                                         ClinicId = clinic.Id,
                                                         ClinicName = clinic.Name,
                                                         BranchId = branch.Id,
                                                         BranchName = branch.Name,
                                                         DoctorId = doc.Id,
                                                         DoctorName = doc.DoctorName,
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

                            dbContextTransaction.Rollback();
                            dbContextTransaction.Dispose();
                            return transactionHeaderList;
                        }

                        if (checkDrug != null)
                        {
                            checkDrug.Stock = checkDrug.Stock - dataDetail.Qty;
                            _context.Drug.Update(checkDrug);
                            await _context.SaveChangesAsync();
                        }

                        TransactionDetailPatient transactionDetailAdd = new TransactionDetailPatient();
                        transactionDetailAdd.TransactionNo = param.TransactionNo;
                        transactionDetailAdd.DrugId = dataDetail.DrugId;
                        transactionDetailAdd.UnitType = dataDetail.UnitType;
                        transactionDetailAdd.Price = dataDetail.Price;
                        transactionDetailAdd.Qty = dataDetail.Qty;
                        transactionDetailAdd.Subtotal = dataDetail.Price * dataDetail.Qty;
                        transactionDetailAdd.CreateBy = param.CreateBy;
                        transactionDetailAdd.CreateDate = DateTime.Now;
                        _context.TransactionDetailPatient.Add(transactionDetailAdd);
                        await _context.SaveChangesAsync();

                        Total = Total + transactionDetailAdd.Subtotal;

                    }

                    var transactionHeaderUpdate = await _context.TransactionHeaderPatient.Where(x => x.Id == transactionHeaderAdd.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (transactionHeaderUpdate != null)
                    {
                        transactionHeaderAdd.Total = Total;
                        _context.TransactionHeaderPatient.Update(transactionHeaderUpdate);
                        await _context.SaveChangesAsync();
                    }

                    transactionHeaderList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                             where tr.Id == transactionHeaderAdd.Id
                                             select new TransactionHeaderPatientResponse
                                             {
                                                 Id = exa.Id,
                                                 TransactionNo = tr.TransactionNo,
                                                 TransactionDate = tr.TransactionDate,
                                                 ClinicId = clinic.Id,
                                                 ClinicName = clinic.Name,
                                                 BranchId = branch.Id,
                                                 BranchName = branch.Name,
                                                 DoctorId = doc.Id,
                                                 DoctorName = doc.DoctorName,
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

                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();
                    return transactionHeaderList;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();

                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "CreateTrHeaderPatient";
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                    return transactionHeaderList;
                }
            }

        }

        public async Task<IEnumerable<TransactionHeaderPatientResponse>> UpdateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<TransactionHeaderPatientResponse>? transactionHeaderList = null;

            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    var transactionHeaderUpdate = await _context.TransactionHeaderPatient.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (transactionHeaderUpdate != null)
                    {
                        transactionHeaderUpdate.TransactionDate = param.TransactionDate;
                        transactionHeaderUpdate.TransactionNo = param.TransactionNo;
                        transactionHeaderUpdate.ExaminationDoctorId = param.ExaminationDoctorId;
                        transactionHeaderUpdate.PaymentType = param.PaymentType;
                        transactionHeaderUpdate.Bpjsno = param.Bpjsno;
                        transactionHeaderUpdate.InsuranceName = param.InsuranceName;
                        transactionHeaderUpdate.InsuranceNo = param.InsuranceNo;
                        transactionHeaderUpdate.Total = 0;
                        transactionHeaderUpdate.UpdateBy = param.UpdateBy;
                        transactionHeaderUpdate.UpdateDate = DateTime.Now;
                        _context.TransactionHeaderPatient.Update(transactionHeaderUpdate);
                        await _context.SaveChangesAsync();
                    }

                    long? Total = 0;
                    foreach (var dataDetail in param.TransactionDetailPatientRequest)
                    {

                        var checkDrug = await _context.Drug.Where(x => x.Id == dataDetail.DrugId).AsNoTracking().FirstOrDefaultAsync();
                        if (checkDrug != null)
                        {
                            if (checkDrug.Stock < dataDetail.Qty)
                            {
                                transactionHeaderList = (from branch in _context.Branch
                                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                         join doc in _context.Doctor on branch.Id equals doc.BranchId
                                                         join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                                         join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                                         join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                                         where tr.Id == param.Id
                                                         select new TransactionHeaderPatientResponse
                                                         {
                                                             Id = exa.Id,
                                                             TransactionNo = tr.TransactionNo,
                                                             TransactionDate = tr.TransactionDate,
                                                             ClinicId = clinic.Id,
                                                             ClinicName = clinic.Name,
                                                             BranchId = branch.Id,
                                                             BranchName = branch.Name,
                                                             DoctorId = doc.Id,
                                                             DoctorName = doc.DoctorName,
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

                                dbContextTransaction.Rollback();
                                dbContextTransaction.Dispose();
                                return transactionHeaderList;
                            }

                        }
                        else
                        {
                            transactionHeaderList = (from branch in _context.Branch
                                                     join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                     join doc in _context.Doctor on branch.Id equals doc.BranchId
                                                     join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                                     join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                                     join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                                     where tr.Id == param.Id
                                                     select new TransactionHeaderPatientResponse
                                                     {
                                                         Id = exa.Id,
                                                         TransactionNo = tr.TransactionNo,
                                                         TransactionDate = tr.TransactionDate,
                                                         ClinicId = clinic.Id,
                                                         ClinicName = clinic.Name,
                                                         BranchId = branch.Id,
                                                         BranchName = branch.Name,
                                                         DoctorId = doc.Id,
                                                         DoctorName = doc.DoctorName,
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

                            dbContextTransaction.Rollback();
                            dbContextTransaction.Dispose();
                            return transactionHeaderList;
                        }

                        if (checkDrug != null)
                        {
                            checkDrug.Stock = checkDrug.Stock - dataDetail.Qty;
                            _context.Drug.Update(checkDrug);
                            await _context.SaveChangesAsync();
                        }

                        var transactionDetailUpdate = await _context.TransactionDetailPatient.Where(x => x.Id == dataDetail.Id).AsNoTracking().FirstOrDefaultAsync();
                        if (transactionDetailUpdate != null)
                        {

                            transactionDetailUpdate.TransactionNo = param.TransactionNo;
                            transactionDetailUpdate.DrugId = dataDetail.DrugId;
                            transactionDetailUpdate.UnitType = dataDetail.UnitType;
                            transactionDetailUpdate.Price = dataDetail.Price;
                            transactionDetailUpdate.Qty = dataDetail.Qty;
                            transactionDetailUpdate.Subtotal = dataDetail.Price * dataDetail.Qty;
                            transactionDetailUpdate.UpdateBy = param.CreateBy;
                            transactionDetailUpdate.UpdateDate = DateTime.Now;
                            _context.TransactionDetailPatient.Update(transactionDetailUpdate);
                            await _context.SaveChangesAsync();

                            Total = Total + transactionDetailUpdate.Subtotal;
                        }

                    }

                    var transactionHeaderUpdateTotal = await _context.TransactionHeaderPatient.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (transactionHeaderUpdateTotal != null)
                    {
                        transactionHeaderUpdateTotal.Total = Total;
                        _context.TransactionHeaderPatient.Update(transactionHeaderUpdateTotal);
                        await _context.SaveChangesAsync();
                    }

                    transactionHeaderList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                             where tr.Id == param.Id
                                             select new TransactionHeaderPatientResponse
                                             {
                                                 Id = exa.Id,
                                                 TransactionNo = tr.TransactionNo,
                                                 TransactionDate = tr.TransactionDate,
                                                 ClinicId = clinic.Id,
                                                 ClinicName = clinic.Name,
                                                 BranchId = branch.Id,
                                                 BranchName = branch.Name,
                                                 DoctorId = doc.Id,
                                                 DoctorName = doc.DoctorName,
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

                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    return transactionHeaderList;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();

                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "UpdateTrHeaderPatient";
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                    return transactionHeaderList;
                }
            }


        }
    }
}
