using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class ScheduleService : IScheduleService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly ISchedule _scheduleRepo;

        public ScheduleService(IConfiguration Configuration, retail_systemContext context, ILogError logError, ISchedule scheduleRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _scheduleRepo = scheduleRepo;
        }

        public async Task<List<ScheduleResponse>> GetSchedule(ScheduleRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listSchedule.Count() > 0)
                {
                    if (param.CompanyId != null && param.CompanyId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSchedule.Where(x => x.CompanyId == param.CompanyId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSchedule;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _scheduleRepo.GetSchedule(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listSchedule.Clear();
                throw;
            }

        }

        public async Task<List<ScheduleResponse>> CreateSchedule(ScheduleAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _scheduleRepo.CreateSchedule(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listSchedule.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listSchedule.Clear();
                throw;
            }


        }

        public async Task<List<ScheduleResponse>> UpdateSchedule(ScheduleUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _scheduleRepo.UpdateSchedule(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listSchedule.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.ScheduleDate = resultData.First().ScheduleDate;
                        checkData.Active = resultData.First().Active;
                        checkData.UpdateBy = resultData.First().UpdateBy;
                        checkData.UpdateDate = DateTime.Now;

                    }
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listSchedule.Clear();
                throw;
            }


        }
    }
}
