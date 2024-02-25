using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ScheduleRepository : ISchedule
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public ScheduleRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ScheduleDoctorResponse>> GetScheduleDoctor(ScheduleDoctorSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ScheduleDoctorResponse>? scheduleDoctorList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Day == null || param.Day == "")
                {
                    scheduleDoctorList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join spe in _context.ScheduleDoctor on branch.Id equals spe.BranchId
                                          join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                          join spec in _context.SpecialistDoctor on spe.SpecialistDoctorId equals spec.Id
                                          join specialist in _context.Specialist on spec.SpecialistId equals specialist.Id
                                          where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId
                                          select new ScheduleDoctorResponse
                                          {
                                              Id = spe.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              SpecialistDoctorId = spec.Id,
                                              DoctorId = spe.DoctorId,
                                              DoctorName = doc.DoctorName,
                                              SpecialistId = specialist.Id,
                                              SpecialistName = specialist.Name,
                                              Active = spe.Active,
                                              CreateBy = spe.CreateBy,
                                              CreateDate = spe.CreateDate,
                                              UpdateBy = spe.UpdateBy,
                                              UpdateDate = spe.UpdateDate
                                          }).OrderBy(x => x.DoctorName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.Day != null || param.Day != "")
                {
                    scheduleDoctorList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join spe in _context.ScheduleDoctor on branch.Id equals spe.BranchId
                                          join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                          join spec in _context.SpecialistDoctor on spe.SpecialistDoctorId equals spec.Id
                                          join specialist in _context.Specialist on spec.SpecialistId equals specialist.Id
                                          where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId && spe.Day == param.Day
                                          select new ScheduleDoctorResponse
                                          {
                                              Id = spe.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              SpecialistDoctorId = spec.Id,
                                              DoctorId = spe.DoctorId,
                                              DoctorName = doc.DoctorName,
                                              SpecialistId = specialist.Id,
                                              SpecialistName = specialist.Name,
                                              Active = spe.Active,
                                              CreateBy = spe.CreateBy,
                                              CreateDate = spe.CreateDate,
                                              UpdateBy = spe.UpdateBy,
                                              UpdateDate = spe.UpdateDate
                                          }).OrderBy(x => x.DoctorName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return scheduleDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetScheduleDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return scheduleDoctorList;
            }
        }

        public async Task<IEnumerable<ScheduleDoctorResponse>> CreateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ScheduleDoctorResponse>? scheduleDoctorList = null;
            ScheduleDoctor scheduleAdd = new ScheduleDoctor();
            try
            {
                scheduleAdd.ClinicId = param.ClinicId;
                scheduleAdd.BranchId = param.BranchId;
                scheduleAdd.DoctorId = param.DoctorId;
                scheduleAdd.SpecialistDoctorId = param.SpecialistDoctorId;
                scheduleAdd.Day = param.Day;
                scheduleAdd.StartTime = param.StartTime;
                scheduleAdd.EndTime = param.EndTime;
                scheduleAdd.Active = param.Active;
                scheduleAdd.CreateBy = param.CreateBy;
                scheduleAdd.CreateDate = DateTime.Now;
                _context.ScheduleDoctor.Add(scheduleAdd);
                await _context.SaveChangesAsync();

                scheduleDoctorList = (from branch in _context.Branch
                                      join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                      join spe in _context.ScheduleDoctor on branch.Id equals spe.BranchId
                                      join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                      join spec in _context.SpecialistDoctor on spe.SpecialistDoctorId equals spec.Id
                                      join specialist in _context.Specialist on spec.SpecialistId equals specialist.Id
                                      where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId && spe.DoctorId == param.DoctorId
                                            && spe.SpecialistDoctorId == param.SpecialistDoctorId
                                      select new ScheduleDoctorResponse
                                      {
                                          Id = spe.Id,
                                          ClinicId = clinic.Id,
                                          ClinicName = clinic.Name,
                                          BranchId = branch.Id,
                                          BranchName = branch.Name,
                                          SpecialistDoctorId = spec.Id,
                                          DoctorId = spe.DoctorId,
                                          DoctorName = doc.DoctorName,
                                          SpecialistId = specialist.Id,
                                          SpecialistName = specialist.Name,
                                          Active = spe.Active,
                                          CreateBy = spe.CreateBy,
                                          CreateDate = spe.CreateDate,
                                          UpdateBy = spe.UpdateBy,
                                          UpdateDate = spe.UpdateDate
                                      }).Take(1).AsNoTracking();


                return scheduleDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateScheduleDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return scheduleDoctorList;
            }

        }

        public async Task<IEnumerable<ScheduleDoctorResponse>> UpdateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ScheduleDoctorResponse>? scheduleDoctorList = null;
            try
            {
                var scheduleUpdate = await _context.ScheduleDoctor.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (scheduleUpdate != null)
                {
                    scheduleUpdate.DoctorId = param.DoctorId;
                    scheduleUpdate.SpecialistDoctorId = param.SpecialistDoctorId;
                    scheduleUpdate.Day = param.Day;
                    scheduleUpdate.StartTime = param.StartTime;
                    scheduleUpdate.EndTime = param.EndTime;
                    scheduleUpdate.Active = param.Active;
                    scheduleUpdate.UpdateBy = param.CreateBy;
                    scheduleUpdate.UpdateDate = DateTime.Now;
                    _context.ScheduleDoctor.Update(scheduleUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    scheduleDoctorList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join spe in _context.ScheduleDoctor on branch.Id equals spe.BranchId
                                          join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                          join spec in _context.SpecialistDoctor on spe.SpecialistDoctorId equals spec.Id
                                          join specialist in _context.Specialist on spec.SpecialistId equals specialist.Id
                                          where spe.Id == param.Id
                                          select new ScheduleDoctorResponse
                                          {
                                              Id = spe.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              SpecialistDoctorId = spec.Id,
                                              DoctorId = spe.DoctorId,
                                              DoctorName = doc.DoctorName,
                                              SpecialistId = specialist.Id,
                                              SpecialistName = specialist.Name,
                                              Active = spe.Active,
                                              CreateBy = spe.CreateBy,
                                              CreateDate = spe.CreateDate,
                                              UpdateBy = spe.UpdateBy,
                                              UpdateDate = spe.UpdateDate
                                          }).Take(0).AsNoTracking();

                    return scheduleDoctorList;
                }


                scheduleDoctorList = (from branch in _context.Branch
                                      join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                      join spe in _context.ScheduleDoctor on branch.Id equals spe.BranchId
                                      join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                      join spec in _context.SpecialistDoctor on spe.SpecialistDoctorId equals spec.Id
                                      join specialist in _context.Specialist on spec.SpecialistId equals specialist.Id
                                      where spe.Id == param.Id
                                      select new ScheduleDoctorResponse
                                      {
                                          Id = spe.Id,
                                          ClinicId = clinic.Id,
                                          ClinicName = clinic.Name,
                                          BranchId = branch.Id,
                                          BranchName = branch.Name,
                                          SpecialistDoctorId = spec.Id,
                                          DoctorId = spe.DoctorId,
                                          DoctorName = doc.DoctorName,
                                          SpecialistId = specialist.Id,
                                          SpecialistName = specialist.Name,
                                          Active = spe.Active,
                                          CreateBy = spe.CreateBy,
                                          CreateDate = spe.CreateDate,
                                          UpdateBy = spe.UpdateBy,
                                          UpdateDate = spe.UpdateDate
                                      }).Take(1).AsNoTracking();

                return scheduleDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateScheduleDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return scheduleDoctorList;
            }

        }
    }
}
