using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class AbsenDoctorRepository : IAbsenDoctor
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public AbsenDoctorRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<AbsenDoctorResponse>> GetAbsenDoctor(AbsenDoctorSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<AbsenDoctorResponse>? absenDoctorList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Day == null || param.Day == "")
                {
                    absenDoctorList = (from branch in _context.Branch
                                       join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                       join doc in _context.Doctor on branch.Id equals doc.BranchId
                                       join abs in _context.AbsentDoctor on doc.Id equals abs.DoctorId
                                       where abs.ClinicId == param.ClinicId && abs.BranchId == param.BranchId &&
                                             Convert.ToDateTime(abs.StartTime) >= Convert.ToDateTime(param.StartTime) &&
                                             Convert.ToDateTime(abs.EndTime) <= Convert.ToDateTime(param.EndTime)
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
                                       }).OrderBy(x => x.DoctorName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.Day != null || param.Day != "")
                {
                    absenDoctorList = (from branch in _context.Branch
                                       join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                       join doc in _context.Doctor on branch.Id equals doc.BranchId
                                       join abs in _context.AbsentDoctor on doc.Id equals abs.DoctorId
                                       where abs.ClinicId == param.ClinicId && abs.BranchId == param.BranchId &&
                                             Convert.ToDateTime(abs.StartTime) >= Convert.ToDateTime(param.StartTime) &&
                                             Convert.ToDateTime(abs.EndTime) <= Convert.ToDateTime(param.EndTime) &&
                                             abs.Day == param.Day
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
                                       }).OrderBy(x => x.DoctorName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return absenDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetAbsenDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return absenDoctorList;
            }
        }

        public async Task<IEnumerable<AbsenDoctorResponse>> CreateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<AbsenDoctorResponse>? absenDoctorList = null;
            AbsentDoctor absenAdd = new AbsentDoctor();
            try
            {
                absenAdd.ClinicId = param.ClinicId;
                absenAdd.BranchId = param.BranchId;
                absenAdd.DoctorId = param.DoctorId;
                absenAdd.AbsenType = param.AbsenType;
                absenAdd.Day = param.Day;
                absenAdd.StartTime = param.StartTime;
                absenAdd.EndTime = param.EndTime;
                absenAdd.CreateBy = param.CreateBy;
                absenAdd.CreateDate = DateTime.Now;
                _context.AbsentDoctor.Add(absenAdd);
                await _context.SaveChangesAsync();

                absenDoctorList = (from branch in _context.Branch
                                   join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                   join doc in _context.Doctor on branch.Id equals doc.BranchId
                                   join abs in _context.AbsentDoctor on doc.Id equals abs.DoctorId
                                   where abs.ClinicId == param.ClinicId && abs.BranchId == param.BranchId && abs.DoctorId == param.DoctorId && abs.Day == param.Day &&
                                         Convert.ToDateTime(abs.StartTime) >= Convert.ToDateTime(param.StartTime) &&
                                         Convert.ToDateTime(abs.EndTime) <= Convert.ToDateTime(param.EndTime) &&
                                         abs.Day == param.Day
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
                                   }).Take(1).AsNoTracking();


                return absenDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateAbsenDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return absenDoctorList;
            }

        }

        public async Task<IEnumerable<AbsenDoctorResponse>> UpdateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<AbsenDoctorResponse>? absenDoctorList = null;
            try
            {
                var absenUpdate = await _context.AbsentDoctor.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (absenUpdate != null)
                {
                    absenUpdate.DoctorId = param.DoctorId;
                    absenUpdate.AbsenType = param.AbsenType;
                    absenUpdate.Day = param.Day;
                    absenUpdate.StartTime = param.StartTime;
                    absenUpdate.EndTime = param.EndTime;
                    absenUpdate.UpdateBy = param.CreateBy;
                    absenUpdate.UpdateDate = DateTime.Now;
                    _context.AbsentDoctor.Update(absenUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    absenDoctorList = (from branch in _context.Branch
                                       join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                       join doc in _context.Doctor on branch.Id equals doc.BranchId
                                       join abs in _context.AbsentDoctor on doc.Id equals abs.DoctorId
                                       where abs.Id == param.Id
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
                                       }).Take(0).AsNoTracking();

                    return absenDoctorList;
                }


                absenDoctorList = (from branch in _context.Branch
                                   join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                   join doc in _context.Doctor on branch.Id equals doc.BranchId
                                   join abs in _context.AbsentDoctor on doc.Id equals abs.DoctorId
                                   where abs.Id == param.Id
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
                                   }).Take(1).AsNoTracking();

                return absenDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateAbsenDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return absenDoctorList;
            }

        }
    }
}
