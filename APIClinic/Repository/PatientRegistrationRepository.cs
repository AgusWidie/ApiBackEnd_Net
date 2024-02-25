using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


namespace APIClinic.Repository
{
    public class PatientRegistrationRepository : IPatientRegistration
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly IQueueNo _queueService;
        public readonly ILogError _errorService;

        public PatientRegistrationRepository(IConfiguration Configuration, IQueueNo queueService, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _queueService = queueService;
            _errorService = errorService;
        }

        public async Task<IEnumerable<PatientRegistrationResponse>> GetPatientRegistration(PatientRegistrationSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PatientRegistrationResponse>? patientRegistrationList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.DoctorId != null || param.QueueNo == "")
                {
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join doc in _context.Doctor on branch.Id equals doc.BranchId
                                               join spe in _context.SpecialistDoctor on doc.Id equals spe.DoctorId
                                               join pat in _context.PatientRegistration on spe.Id equals pat.SpecialistDoctorId
                                               join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                               where pat.ClinicId == param.ClinicId && pat.BranchId == param.BranchId
                                                     && spe.DoctorId == param.DoctorId && pat.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd")
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
                                               }).OrderByDescending(x => x.QueueNo).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.DoctorId != null || param.QueueNo != "")
                {
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join doc in _context.Doctor on branch.Id equals doc.BranchId
                                               join spe in _context.SpecialistDoctor on doc.Id equals spe.DoctorId
                                               join pat in _context.PatientRegistration on spe.Id equals pat.SpecialistDoctorId
                                               join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                               where pat.ClinicId == param.ClinicId && pat.BranchId == param.BranchId
                                                     && spe.DoctorId == param.DoctorId && pat.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd")
                                                     && pat.QueueNo == param.QueueNo
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
                                               }).OrderByDescending(x => x.QueueNo).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return patientRegistrationList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetPatientRegistration";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return patientRegistrationList;
            }
        }

        public async Task<IEnumerable<PatientRegistrationResponse>> CreatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PatientRegistrationResponse>? patientRegistrationList = null;
            PatientRegistration patientRegistrationAdd = new PatientRegistration();
            try
            {
                patientRegistrationAdd.ClinicId = param.ClinicId;
                patientRegistrationAdd.BranchId = param.BranchId;
                patientRegistrationAdd.SpecialistDoctorId = param.SpecialistDoctorId;
                patientRegistrationAdd.QueueNo = await _queueService.CreateQueueDoctor(param.SpecialistDoctorId, cancellationToken);
                //patientRegistrationAdd.QueueNo = param.QueueNo;
                patientRegistrationAdd.RegistrationDate = DateTime.Now;
                patientRegistrationAdd.Ktpno = param.Ktpno;
                patientRegistrationAdd.FamilyCardNo = param.FamilyCardNo;
                patientRegistrationAdd.Name = param.Name;
                patientRegistrationAdd.DateOfBirth = param.DateOfBirth;
                patientRegistrationAdd.Gender = param.Gender;
                patientRegistrationAdd.Address = param.Address;
                patientRegistrationAdd.Religion = param.Religion;
                patientRegistrationAdd.Education = param.Education;
                patientRegistrationAdd.Work = param.Work;
                patientRegistrationAdd.Complaint = param.Complaint;
                patientRegistrationAdd.PaymentType = param.PaymentType;
                patientRegistrationAdd.Bpjsno = param.Bpjsno;
                patientRegistrationAdd.InsuranceName = param.InsuranceName;
                patientRegistrationAdd.InsuranceNo = param.InsuranceNo;
                patientRegistrationAdd.CreateBy = param.CreateBy;
                patientRegistrationAdd.CreateDate = DateTime.Now;
                _context.PatientRegistration.Add(patientRegistrationAdd);
                await _context.SaveChangesAsync();

                patientRegistrationList = (from branch in _context.Branch
                                           join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                           join doc in _context.Doctor on branch.Id equals doc.BranchId
                                           join spe in _context.SpecialistDoctor on doc.Id equals spe.DoctorId
                                           join pat in _context.PatientRegistration on spe.Id equals pat.SpecialistDoctorId
                                           join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                           where pat.ClinicId == param.ClinicId && pat.BranchId == param.BranchId
                                                 && spe.Id == param.SpecialistDoctorId && pat.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd")
                                                 && pat.QueueNo == param.QueueNo
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
                                           }).Take(1).AsNoTracking();


                return patientRegistrationList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreatePatientRegistration";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return patientRegistrationList;
            }

        }

        public async Task<IEnumerable<PatientRegistrationResponse>> UpdatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PatientRegistrationResponse>? patientRegistrationList = null;
            try
            {
                var patientRegistrationUpdate = await _context.PatientRegistration.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (patientRegistrationUpdate != null)
                {
                    patientRegistrationUpdate.SpecialistDoctorId = param.SpecialistDoctorId;
                    patientRegistrationUpdate.RegistrationDate = DateTime.Now;
                    patientRegistrationUpdate.Ktpno = param.Ktpno;
                    patientRegistrationUpdate.FamilyCardNo = param.FamilyCardNo;
                    patientRegistrationUpdate.Name = param.Name;
                    patientRegistrationUpdate.DateOfBirth = param.DateOfBirth;
                    patientRegistrationUpdate.Gender = param.Gender;
                    patientRegistrationUpdate.Address = param.Address;
                    patientRegistrationUpdate.Religion = param.Religion;
                    patientRegistrationUpdate.Education = param.Education;
                    patientRegistrationUpdate.Work = param.Work;
                    patientRegistrationUpdate.PaymentType = param.PaymentType;
                    patientRegistrationUpdate.Bpjsno = param.Bpjsno;
                    patientRegistrationUpdate.InsuranceName = param.InsuranceName;
                    patientRegistrationUpdate.InsuranceNo = param.InsuranceNo;
                    patientRegistrationUpdate.Complaint = param.Complaint;
                    patientRegistrationUpdate.UpdateBy = param.UpdateBy;
                    patientRegistrationUpdate.UpdateDate = DateTime.Now;
                    _context.PatientRegistration.Update(patientRegistrationUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    patientRegistrationList = (from branch in _context.Branch
                                               join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                               join doc in _context.Doctor on branch.Id equals doc.BranchId
                                               join spe in _context.SpecialistDoctor on doc.Id equals spe.DoctorId
                                               join pat in _context.PatientRegistration on spe.Id equals pat.SpecialistDoctorId
                                               join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                               where pat.Id == param.Id
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
                                               }).Take(0).AsNoTracking();

                    return patientRegistrationList;
                }


                patientRegistrationList = (from branch in _context.Branch
                                           join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                           join doc in _context.Doctor on branch.Id equals doc.BranchId
                                           join spe in _context.SpecialistDoctor on doc.Id equals spe.DoctorId
                                           join pat in _context.PatientRegistration on spe.Id equals pat.SpecialistDoctorId
                                           join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                           where pat.Id == param.Id
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
                                           }).Take(1).AsNoTracking();

                return patientRegistrationList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdatePatientRegistration";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return patientRegistrationList;
            }

        }
    }
}
