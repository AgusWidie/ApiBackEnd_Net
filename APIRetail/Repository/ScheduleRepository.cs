using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class ScheduleRepository : ISchedule
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public ScheduleRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<ScheduleResponse>> GetSchedule(ScheduleRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ScheduleResponse>? scheduleList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.CompanyId != null)
                {
                    scheduleList = (from sch in _context.Schedule
                                    join company in _context.Company on sch.CompanyId equals company.Id
                                    where sch.CompanyId == param.CompanyId
                                    select new ScheduleResponse
                                    {
                                        Id = sch.Id,
                                        ScheduleDate = sch.ScheduleDate,
                                        CompanyId = company.Id,
                                        CompanyName = company.Name,
                                        Active = sch.Active,
                                        CreateBy = sch.CreateBy,
                                        CreateDate = sch.CreateDate,
                                        UpdateBy = sch.UpdateBy,
                                        UpdateDate = sch.UpdateDate
                                    }).OrderByDescending(x => x.Id).AsNoTracking();
                }

                if (param.CompanyId == null)
                {
                    scheduleList = (from sch in _context.Schedule
                                    join company in _context.Company on sch.CompanyId equals company.Id
                                    select new ScheduleResponse
                                    {
                                        Id = sch.Id,
                                        ScheduleDate = sch.ScheduleDate,
                                        CompanyId = company.Id,
                                        CompanyName = company.Name,
                                        Active = sch.Active,
                                        CreateBy = sch.CreateBy,
                                        CreateDate = sch.CreateDate,
                                        UpdateBy = sch.UpdateBy,
                                        UpdateDate = sch.UpdateDate
                                    }).OrderByDescending(x => x.Id).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)scheduleList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = scheduleList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetSchedule";
                if (ex.InnerException != null)
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }
                else
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return scheduleList;
            }

        }

        public async Task<IEnumerable<ScheduleResponse>> CreateSchedule(ScheduleAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ScheduleResponse>? scheduleList = null;
            Schedule scheduleAdd = new Schedule();
            try
            {
                var checkSchedule = await _context.Schedule.Where(x => x.ScheduleDate == param.ScheduleDate).AsNoTracking().FirstOrDefaultAsync();
                if (checkSchedule != null)
                {
                    scheduleList = (from sch in _context.Schedule
                                    join company in _context.Company on sch.CompanyId equals company.Id
                                    where sch.CompanyId == param.CompanyId && sch.ScheduleDate == param.ScheduleDate
                                    orderby sch.Id
                                    select new ScheduleResponse
                                    {
                                        Id = sch.Id,
                                        ScheduleDate = sch.ScheduleDate,
                                        CompanyId = company.Id,
                                        CompanyName = company.Name,
                                        Active = sch.Active,
                                        CreateBy = sch.CreateBy,
                                        CreateDate = sch.CreateDate,
                                        UpdateBy = sch.UpdateBy,
                                        UpdateDate = sch.UpdateDate
                                    }).Take(0).AsNoTracking();

                    return scheduleList;

                }
                else
                {

                    scheduleAdd.CompanyId = param.CompanyId;
                    scheduleAdd.ScheduleDate = param.ScheduleDate;
                    scheduleAdd.Active = param.Active;
                    scheduleAdd.CreateBy = param.CreateBy;
                    scheduleAdd.CreateDate = DateTime.Now;
                    _context.Schedule.Add(scheduleAdd);
                    await _context.SaveChangesAsync();
                }


                scheduleList = (from sch in _context.Schedule
                                join company in _context.Company on sch.CompanyId equals company.Id
                                where sch.CompanyId == param.CompanyId && sch.ScheduleDate == param.ScheduleDate
                                orderby sch.Id
                                select new ScheduleResponse
                                {
                                    Id = sch.Id,
                                    ScheduleDate = sch.ScheduleDate,
                                    CompanyId = company.Id,
                                    CompanyName = company.Name,
                                    Active = sch.Active,
                                    CreateBy = sch.CreateBy,
                                    CreateDate = sch.CreateDate,
                                    UpdateBy = sch.UpdateBy,
                                    UpdateDate = sch.UpdateDate
                                }).Take(1).AsNoTracking();


                return scheduleList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateSchedule";
                if (ex.InnerException != null)
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }
                else
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return scheduleList;
            }

        }

        public async Task<IEnumerable<ScheduleResponse>> UpdateSchedule(ScheduleUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ScheduleResponse>? scheduleList = null;
            try
            {
                var checkSchedule = await _context.Schedule.Where(x => x.ScheduleDate == param.ScheduleDate).AsNoTracking().FirstOrDefaultAsync();
                if (checkSchedule != null)
                {
                    scheduleList = (from sch in _context.Schedule
                                    join company in _context.Company on sch.CompanyId equals company.Id
                                    where sch.CompanyId == param.CompanyId && sch.ScheduleDate == param.ScheduleDate
                                    orderby sch.Id
                                    select new ScheduleResponse
                                    {
                                        Id = sch.Id,
                                        ScheduleDate = sch.ScheduleDate,
                                        CompanyId = company.Id,
                                        CompanyName = company.Name,
                                        Active = sch.Active,
                                        CreateBy = sch.CreateBy,
                                        CreateDate = sch.CreateDate,
                                        UpdateBy = sch.UpdateBy,
                                        UpdateDate = sch.UpdateDate
                                    }).Take(0).AsNoTracking();

                    return scheduleList;

                }
                else
                {
                    var scheduleUpdate = await _context.Schedule.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (scheduleUpdate != null)
                    {
                        scheduleUpdate.CompanyId = param.CompanyId;
                        scheduleUpdate.ScheduleDate = param.ScheduleDate;
                        scheduleUpdate.Active = param.Active;
                        scheduleUpdate.UpdateBy = param.UpdateBy;
                        scheduleUpdate.UpdateDate = DateTime.Now;
                        _context.Schedule.Update(scheduleUpdate);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {

                        scheduleList = (from sch in _context.Schedule
                                        join company in _context.Company on sch.CompanyId equals company.Id
                                        where sch.CompanyId == param.CompanyId && sch.ScheduleDate == param.ScheduleDate
                                        orderby sch.Id
                                        select new ScheduleResponse
                                        {
                                            Id = sch.Id,
                                            ScheduleDate = sch.ScheduleDate,
                                            CompanyId = company.Id,
                                            CompanyName = company.Name,
                                            Active = sch.Active,
                                            CreateBy = sch.CreateBy,
                                            CreateDate = sch.CreateDate,
                                            UpdateBy = sch.UpdateBy,
                                            UpdateDate = sch.UpdateDate
                                        }).Take(0).AsNoTracking();

                        return scheduleList;

                    }

                }

                scheduleList = (from sch in _context.Schedule
                                join company in _context.Company on sch.CompanyId equals company.Id
                                where sch.CompanyId == param.CompanyId && sch.ScheduleDate == param.ScheduleDate
                                orderby sch.Id
                                select new ScheduleResponse
                                {
                                    Id = sch.Id,
                                    ScheduleDate = sch.ScheduleDate,
                                    CompanyId = company.Id,
                                    CompanyName = company.Name,
                                    Active = sch.Active,
                                    CreateBy = sch.CreateBy,
                                    CreateDate = sch.CreateDate,
                                    UpdateBy = sch.UpdateBy,
                                    UpdateDate = sch.UpdateDate
                                }).Take(1).AsNoTracking();


                return scheduleList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateSchedule";
                if (ex.InnerException != null)
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }
                else
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return scheduleList;
            }

        }
    }
}
