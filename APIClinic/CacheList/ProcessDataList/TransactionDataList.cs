using APIClinic.CacheList.ProcessDataList.Interface;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.CacheList.ProcessDataList
{
    public class TransactionDataList : ITransactionDataList
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public TransactionDataList(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public void GetAbsenDoctor()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<AbsenDoctorResponse>? absenDoctorList = null;
                    absenDoctorList = (from branch in _context.Branch
                                       join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                       join doc in _context.Doctor on branch.Id equals doc.BranchId
                                       join abs in _context.AbsentDoctor on doc.Id equals abs.DoctorId
                                       where Convert.ToDateTime(abs.StartTime).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:AbsenDoctorYear"))
                                       select new AbsenDoctorResponse
                                       {
                                           Id = abs.Id,
                                           ClinicId = clinic.Id,
                                           ClinicName = clinic.Name,
                                           BranchId = branch.Id,
                                           BranchName = branch.Name,
                                           DoctorId = abs.DoctorId,
                                           DoctorName = doc.DoctorName,
                                           AbsenType = abs.AbsenType,
                                           Day = abs.Day,
                                           StartTime = abs.StartTime,
                                           EndTime = abs.EndTime,
                                           CreateBy = abs.CreateBy,
                                           CreateDate = abs.CreateDate,
                                           UpdateBy = abs.UpdateBy,
                                           UpdateDate = abs.UpdateDate
                                       }).OrderBy(x => x.DoctorName).AsNoTracking();

                    if (absenDoctorList.Count() > 0)
                    {
                        if (GeneralList._listAbsenDoctor.Count() <= 0 || GeneralList._listAbsenDoctor.Count() < absenDoctorList.Count())
                        {
                            GeneralList._listAbsenDoctor.Clear();
                            GeneralList._listAbsenDoctor.AddRange(absenDoctorList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetAbsenDoctor";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetExaminationDoctor()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ExaminationDoctorResponse>? examinationDoctorList = null;
                    examinationDoctorList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             where Convert.ToDateTime(exa.ExaminationDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:ExaminationDoctorYear"))
                                             select new ExaminationDoctorResponse
                                             {
                                                 Id = exa.Id,
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
                                                 ExaminationDate = exa.ExaminationDate,
                                                 Inspection = exa.Inspection,
                                                 Recipe = exa.Recipe,
                                                 CreateBy = exa.CreateBy,
                                                 CreateDate = exa.CreateDate,
                                                 UpdateBy = exa.UpdateBy,
                                                 UpdateDate = exa.UpdateDate
                                             }).OrderBy(x => x.QueueNo).AsNoTracking();

                    if (examinationDoctorList.Count() > 0)
                    {
                        if (GeneralList._listExaminationDoctor.Count() <= 0 || GeneralList._listAbsenDoctor.Count() < examinationDoctorList.Count())
                        {
                            GeneralList._listExaminationDoctor.Clear();
                            GeneralList._listExaminationDoctor.AddRange(examinationDoctorList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {

                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetExaminationDoctor";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }

        public void GetExaminationLab()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<ExaminationLabResponse>? examinationLabList = null;
                    examinationLabList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                          join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                          where Convert.ToDateTime(exa.ExaminationDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:ExaminationLabYear"))
                                          select new ExaminationLabResponse
                                          {
                                              Id = exa.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              QueueNo = exa.QueueNo,
                                              Ktpno = pat.Ktpno,
                                              FamilyCardNo = pat.FamilyCardNo,
                                              Name = pat.Name,
                                              Description = exa.Description,
                                              CreateBy = exa.CreateBy,
                                              CreateDate = exa.CreateDate,
                                              UpdateBy = exa.UpdateBy,
                                              UpdateDate = exa.UpdateDate
                                          }).OrderBy(x => x.QueueNo).AsNoTracking();

                    if (examinationLabList.Count() > 0)
                    {
                        if (GeneralList._listExaminationLab.Count() <= 0 || GeneralList._listExaminationLab.Count() < examinationLabList.Count())
                        {
                            GeneralList._listExaminationLab.Clear();
                            GeneralList._listExaminationLab.AddRange(examinationLabList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetExaminationLab";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }
        }

        public void GetPatientRegistrationLab()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<PatientRegistrationLabResponse>? patientRegistrationList = null;
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                               join pat in _context.PatientRegistrationLab on lab.Id equals pat.LaboratoriumId
                                               where Convert.ToDateTime(pat.RegistrationDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:PatientRegistrationLabYear"))
                                               select new PatientRegistrationLabResponse
                                               {
                                                   Id = pat.Id,
                                                   ClinicId = clinic.Id,
                                                   ClinicName = clinic.Name,
                                                   BranchId = branch.Id,
                                                   BranchName = branch.Name,
                                                   QueueNo = pat.QueueNo,
                                                   RegistrationDate = pat.RegistrationDate,
                                                   Ktpno = pat.Ktpno,
                                                   FamilyCardNo = pat.FamilyCardNo,
                                                   Name = pat.Name,
                                                   DateOfBirth = pat.DateOfBirth,
                                                   Gender = pat.Gender,
                                                   Address = pat.Address,
                                                   Religion = pat.Religion,
                                                   Education = pat.Education,
                                                   Work = pat.Work,
                                                   PaymentType = pat.PaymentType,
                                                   Bpjsno = pat.Bpjsno,
                                                   InsuranceName = pat.InsuranceName,
                                                   InsuranceNo = pat.InsuranceNo,
                                                   CreateBy = pat.CreateBy,
                                                   CreateDate = pat.CreateDate,
                                                   UpdateBy = pat.UpdateBy,
                                                   UpdateDate = pat.UpdateDate
                                               }).OrderByDescending(x => x.QueueNo).AsNoTracking();

                    if (patientRegistrationList.Count() > 0)
                    {
                        if (GeneralList._listPatientRegistrationLab.Count() <= 0 || GeneralList._listPatientRegistrationLab.Count() < patientRegistrationList.Count())
                        {
                            GeneralList._listPatientRegistrationLab.Clear();
                            GeneralList._listPatientRegistrationLab.AddRange(patientRegistrationList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetPatientRegistrationLab";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }
        }

        public void GetPatientRegistration()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<PatientRegistrationResponse>? patientRegistrationList = null;
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join doc in _context.Doctor on branch.Id equals doc.BranchId
                                               join spe in _context.SpecialistDoctor on doc.Id equals spe.DoctorId
                                               join pat in _context.PatientRegistration on spe.Id equals pat.SpecialistDoctorId
                                               join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                               where Convert.ToDateTime(pat.RegistrationDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:PatientRegistrationYear"))
                                               select new PatientRegistrationResponse
                                               {
                                                   Id = pat.Id,
                                                   ClinicId = clinic.Id,
                                                   ClinicName = clinic.Name,
                                                   BranchId = branch.Id,
                                                   BranchName = branch.Name,
                                                   SpecialistDoctorId = spe.Id,
                                                   DoctorId = doc.Id,
                                                   DoctorName = doc.DoctorName,
                                                   SpecialistId = specialist.Id,
                                                   SpecialistName = specialist.Name,
                                                   QueueNo = pat.QueueNo,
                                                   RegistrationDate = pat.RegistrationDate,
                                                   Ktpno = pat.Ktpno,
                                                   FamilyCardNo = pat.FamilyCardNo,
                                                   Name = pat.Name,
                                                   DateOfBirth = pat.DateOfBirth,
                                                   Gender = pat.Gender,
                                                   Address = pat.Address,
                                                   Religion = pat.Religion,
                                                   Education = pat.Education,
                                                   Work = pat.Work,
                                                   PaymentType = pat.PaymentType,
                                                   Bpjsno = pat.Bpjsno,
                                                   InsuranceName = pat.InsuranceName,
                                                   InsuranceNo = pat.InsuranceNo,
                                                   Complaint = pat.Complaint,
                                                   CreateBy = pat.CreateBy,
                                                   CreateDate = pat.CreateDate,
                                                   UpdateBy = pat.UpdateBy,
                                                   UpdateDate = pat.UpdateDate
                                               }).OrderByDescending(x => x.QueueNo).AsNoTracking();

                    if (patientRegistrationList.Count() > 0)
                    {
                        if (GeneralList._listPatientRegistration.Count() <= 0 || GeneralList._listPatientRegistration.Count() < patientRegistrationList.Count())
                        {
                            GeneralList._listPatientRegistration.Clear();
                            GeneralList._listPatientRegistration.AddRange(patientRegistrationList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetPatientRegistration";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }


        }

        public void GetTransactionHeaderPatientRegistration()
        {

            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<TransactionHeaderPatientResponse>? transactionHeaderList = null;

                    transactionHeaderList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             join tr in _context.TransactionHeaderPatient on exa.Id equals tr.ExaminationDoctorId
                                             where Convert.ToDateTime(tr.TransactionDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:TrPatientRegistrationYear"))
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
                                             }).OrderBy(x => x.TransactionDate).AsNoTracking();

                    if (transactionHeaderList.Count() > 0)
                    {
                        if (GeneralList._listTransactionHeaderPatient.Count() <= 0 || GeneralList._listTransactionHeaderPatient.Count() < transactionHeaderList.Count())
                        {
                            GeneralList._listTransactionHeaderPatient.Clear();
                            GeneralList._listTransactionHeaderPatient.AddRange(transactionHeaderList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetTransactionHeaderPatientRegistration";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }
        }

        public void GetTransactionDetailPatientRegistration()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<TransactionDetailPatientResponse>? transactionDetailList = null;
                    transactionDetailList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join tr in _context.TransactionHeaderPatient on branch.Id equals tr.BranchId
                                             join td in _context.TransactionDetailPatient on tr.TransactionNo equals td.TransactionNo
                                             join drug in _context.Drug on td.DrugId equals drug.Id
                                             where Convert.ToDateTime(tr.TransactionDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:TrPatientRegistrationYear"))
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

                    if (transactionDetailList.Count() > 0)
                    {
                        if (GeneralList._listTransactionDetailPatient.Count() <= 0 || GeneralList._listTransactionDetailPatient.Count() < transactionDetailList.Count())
                        {
                            GeneralList._listTransactionDetailPatient.Clear();
                            GeneralList._listTransactionDetailPatient.AddRange(transactionDetailList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetTransactionDetailPatientRegistration";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }
        }

        public void GetTransactionHeaderPatientLab()
        {
            using (var dbTrans = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    IEnumerable<TransactionHeaderPatientLabResponse>? transactionHeaderLabList = null;

                    transactionHeaderLabList = (from branch in _context.Branch
                                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                                join pat in _context.PatientRegistrationLab on exa.QueueNo equals pat.QueueNo
                                                join lab in _context.Laboratorium on pat.LaboratoriumId equals lab.Id
                                                join tr in _context.TransactionLab on exa.Id equals tr.ExaminationLabId
                                                where Convert.ToDateTime(tr.TransactionDate).Year == Convert.ToInt32(_configuration.GetValue<string>("JobTransactionDataList:TrPatientLabYear"))
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
                                                }).OrderBy(x => x.TransactionDate).AsNoTracking();

                    if (transactionHeaderLabList.Count() > 0)
                    {
                        if (GeneralList._listTransactionHeaderPatientLab.Count() <= 0 || GeneralList._listTransactionHeaderPatientLab.Count() < transactionHeaderLabList.Count())
                        {
                            GeneralList._listTransactionHeaderPatientLab.Clear();
                            GeneralList._listTransactionHeaderPatientLab.AddRange(transactionHeaderLabList);
                            GarbageCollector.GarbageCollection();
                        }
                    }
                    dbTrans.Dispose();
                }
                catch (Exception ex)
                {
                    LogErrorRequest logDataError = new LogErrorRequest();
                    logDataError.ServiceName = "GetTransactionHeaderPatientLab";
                    logDataError.ErrorDeskripsi = "Error Job TransactionDataList : " + ex.Message + ", Source : " + ex.StackTrace;
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";
                    dbTrans.Dispose();

                    var resultError = _errorService.CreateLogError(logDataError);
                }
            }

        }
    }
}
