using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ExaminationDoctorRepository : IExaminationDoctor
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public ExaminationDoctorRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ExaminationDoctorResponse>> GetExamination(ExaminationDoctorSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ExaminationDoctorResponse>? examinationDoctorList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.DoctorId != null && param.QueueNo == "")
                {
                    examinationDoctorList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             where exa.ClinicId == param.ClinicId && exa.BranchId == param.BranchId && exa.DoctorId == param.DoctorId &&
                                                   exa.ExaminationDate >= param.ExaminationDateFrom && exa.ExaminationDate <= param.ExaminationDateTo
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
                                             }).OrderBy(x => x.QueueNo).AsNoTracking().ToList();
                }

                if (param.DoctorId != null && param.QueueNo != "")
                {
                    examinationDoctorList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             where exa.ClinicId == param.ClinicId && exa.BranchId == param.BranchId && exa.DoctorId == param.DoctorId && exa.QueueNo == param.QueueNo
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
                                             }).OrderBy(x => x.QueueNo).AsNoTracking().ToList();
                }

                var TotalPageSize = Math.Ceiling((decimal)examinationDoctorList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = examinationDoctorList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetExamination";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return examinationDoctorList;
            }
        }

        public async Task<IEnumerable<ExaminationDoctorResponse>> CreateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ExaminationDoctorResponse>? examinationDoctorList = null;
            ExaminationDoctor examinationDoctorAdd = new ExaminationDoctor();
            try
            {
                examinationDoctorAdd.ClinicId = param.ClinicId;
                examinationDoctorAdd.BranchId = param.BranchId;
                examinationDoctorAdd.DoctorId = param.DoctorId;
                examinationDoctorAdd.QueueNo = param.QueueNo;
                examinationDoctorAdd.Inspection = param.Inspection;
                examinationDoctorAdd.Recipe = param.Recipe;
                examinationDoctorAdd.CreateBy = param.CreateBy;
                examinationDoctorAdd.CreateDate = DateTime.Now;
                _context.ExaminationDoctor.Add(examinationDoctorAdd);
                await _context.SaveChangesAsync();

                examinationDoctorList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join doc in _context.Doctor on branch.Id equals doc.BranchId
                                         join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                         join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                         where exa.ClinicId == param.ClinicId && exa.BranchId == param.BranchId
                                               && exa.DoctorId == param.DoctorId && exa.QueueNo == param.QueueNo
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
                                         }).Take(1).AsNoTracking();


                return examinationDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateExaminationDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return examinationDoctorList;
            }

        }

        public async Task<IEnumerable<ExaminationDoctorResponse>> UpdateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ExaminationDoctorResponse>? examinationDoctorList = null;
            try
            {
                var examinationDoctorUpdate = await _context.ExaminationDoctor.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (examinationDoctorUpdate != null)
                {
                    examinationDoctorUpdate.DoctorId = param.DoctorId;
                    examinationDoctorUpdate.QueueNo = param.QueueNo;
                    examinationDoctorUpdate.Inspection = param.Inspection;
                    examinationDoctorUpdate.Recipe = param.Recipe;
                    examinationDoctorUpdate.UpdateBy = param.UpdateBy;
                    examinationDoctorUpdate.UpdateDate = DateTime.Now;
                    _context.ExaminationDoctor.Update(examinationDoctorUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    examinationDoctorList = (from branch in _context.Branch
                                             join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                             join doc in _context.Doctor on branch.Id equals doc.BranchId
                                             join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                             join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                             where exa.Id == param.Id
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
                                             }).Take(0).AsNoTracking();

                    return examinationDoctorList;
                }


                examinationDoctorList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join doc in _context.Doctor on branch.Id equals doc.BranchId
                                         join exa in _context.ExaminationDoctor on doc.Id equals exa.DoctorId
                                         join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                         where exa.Id == param.Id
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
                                         }).Take(1).AsNoTracking();

                return examinationDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateExaminationDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return examinationDoctorList;
            }

        }
    }
}
