using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class PatientRegistrationLabRepository : IPatientRegistrationLab
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly IQueueNo _queueLabService;
        public readonly ILogError _errorService;

        public PatientRegistrationLabRepository(IConfiguration Configuration, IQueueNo queueLabService, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _queueLabService = queueLabService;
            _errorService = errorService;
        }

        public async Task<IEnumerable<PatientRegistrationLabResponse>> GetPatientRegistrationLab(PatientRegistrationLabSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PatientRegistrationLabResponse>? patientRegistrationList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.QueueNo == null || param.QueueNo == "")
                {
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                               join pat in _context.PatientRegistrationLab on lab.Id equals pat.LaboratoriumId
                                               where pat.ClinicId == param.ClinicId && pat.BranchId == param.BranchId
                                                     && pat.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd")
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
                                               }).OrderByDescending(x => x.QueueNo).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.QueueNo != null && param.QueueNo != "")
                {
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                               join pat in _context.PatientRegistrationLab on lab.Id equals pat.LaboratoriumId
                                               where pat.ClinicId == param.ClinicId && pat.BranchId == param.BranchId
                                                     && pat.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd")
                                                     && param.QueueNo.Contains(param.QueueNo)
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
                                               }).OrderByDescending(x => x.QueueNo).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return patientRegistrationList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetPatientRegistrationLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return patientRegistrationList;
            }
        }

        public async Task<IEnumerable<PatientRegistrationLabResponse>> CreatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PatientRegistrationLabResponse>? patientRegistrationLabList = null;
            PatientRegistrationLab patientRegistrationLabAdd = new PatientRegistrationLab();
            try
            {
                patientRegistrationLabAdd.ClinicId = param.ClinicId;
                patientRegistrationLabAdd.BranchId = param.BranchId;
                patientRegistrationLabAdd.LaboratoriumId = param.LaboratoriumId;
                //patientRegistrationLabAdd.QueueNo = await _queueLabService.CreateQueueLab(param.LaboratoriumId, cancellationToken);
                patientRegistrationLabAdd.QueueNo = param.QueueNo;
                patientRegistrationLabAdd.RegistrationDate = DateTime.Now;
                patientRegistrationLabAdd.Ktpno = param.Ktpno;
                patientRegistrationLabAdd.FamilyCardNo = param.FamilyCardNo;
                patientRegistrationLabAdd.Name = param.Name;
                patientRegistrationLabAdd.DateOfBirth = param.DateOfBirth;
                patientRegistrationLabAdd.Gender = param.Gender;
                patientRegistrationLabAdd.Address = param.Address;
                patientRegistrationLabAdd.Religion = param.Religion;
                patientRegistrationLabAdd.Education = param.Education;
                patientRegistrationLabAdd.Religion = param.Religion;
                patientRegistrationLabAdd.Work = param.Work;
                patientRegistrationLabAdd.PaymentType = param.PaymentType;
                patientRegistrationLabAdd.Bpjsno = param.Bpjsno;
                patientRegistrationLabAdd.InsuranceName = param.InsuranceName;
                patientRegistrationLabAdd.InsuranceNo = param.InsuranceNo;
                patientRegistrationLabAdd.CreateBy = param.CreateBy;
                patientRegistrationLabAdd.CreateDate = DateTime.Now;
                _context.PatientRegistrationLab.Add(patientRegistrationLabAdd);
                await _context.SaveChangesAsync();

                patientRegistrationLabList = (from branch in _context.Branch
                                              join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                              join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                              join pat in _context.PatientRegistrationLab on lab.Id equals pat.LaboratoriumId
                                              where pat.ClinicId == param.ClinicId && pat.BranchId == param.BranchId
                                                    && pat.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd")
                                                    && param.QueueNo.Contains(param.QueueNo)
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
                                              }).Take(1).AsNoTracking();


                return patientRegistrationLabList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreatePatientRegistrationLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return patientRegistrationLabList;
            }

        }

        public async Task<IEnumerable<PatientRegistrationLabResponse>> UpdatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PatientRegistrationLabResponse>? patientRegistrationLabList = null;
            try
            {
                var patientRegistrationLabUpdate = await _context.PatientRegistrationLab.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (patientRegistrationLabUpdate != null)
                {
                    patientRegistrationLabUpdate.LaboratoriumId = param.LaboratoriumId;
                    patientRegistrationLabUpdate.RegistrationDate = DateTime.Now;
                    patientRegistrationLabUpdate.Ktpno = param.Ktpno;
                    patientRegistrationLabUpdate.FamilyCardNo = param.FamilyCardNo;
                    patientRegistrationLabUpdate.Name = param.Name;
                    patientRegistrationLabUpdate.DateOfBirth = param.DateOfBirth;
                    patientRegistrationLabUpdate.Gender = param.Gender;
                    patientRegistrationLabUpdate.Address = param.Address;
                    patientRegistrationLabUpdate.Religion = param.Religion;
                    patientRegistrationLabUpdate.Education = param.Education;
                    patientRegistrationLabUpdate.Religion = param.Religion;
                    patientRegistrationLabUpdate.Work = param.Work;
                    patientRegistrationLabUpdate.PaymentType = param.PaymentType;
                    patientRegistrationLabUpdate.Bpjsno = param.Bpjsno;
                    patientRegistrationLabUpdate.InsuranceName = param.InsuranceName;
                    patientRegistrationLabUpdate.InsuranceNo = param.InsuranceNo;
                    patientRegistrationLabUpdate.UpdateBy = param.UpdateBy;
                    patientRegistrationLabUpdate.UpdateDate = DateTime.Now;
                    _context.PatientRegistrationLab.Update(patientRegistrationLabUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    patientRegistrationLabList = (from branch in _context.Branch
                                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                                  join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                                  join pat in _context.PatientRegistrationLab on lab.Id equals pat.LaboratoriumId
                                                  where pat.Id == param.Id
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
                                                  }).Take(0).AsNoTracking();

                    return patientRegistrationLabList;
                }


                patientRegistrationLabList = (from branch in _context.Branch
                                              join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                              join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                              join pat in _context.PatientRegistrationLab on lab.Id equals pat.LaboratoriumId
                                              where pat.Id == param.Id
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
                                              }).Take(1).AsNoTracking();

                return patientRegistrationLabList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdatePatientRegistrationLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return patientRegistrationLabList;
            }

        }
    }
}
