using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APIClinic.Service
{
    public class ScheduleDoctorService : IScheduleDoctorService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ISchedule _scheduleRepo;

        public ScheduleDoctorService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ISchedule scheduleRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _scheduleRepo = scheduleRepo;
        }

        public async Task<List<ScheduleDoctorResponse>> GetScheduleDoctor(ScheduleDoctorSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listScheduleDoctor.Count() > 0)
                {
                    if (param.Day != null && param.Day != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listScheduleDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.Day == param.Day);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listScheduleDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _scheduleRepo.GetScheduleDoctor(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ScheduleDoctorResponse>> CreateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _scheduleRepo.CreateScheduleDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listScheduleDoctor.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ScheduleDoctorResponse>> UpdateScheduleDoctor(ScheduleDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _scheduleRepo.UpdateScheduleDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listScheduleDoctor.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.DoctorId = resultData.First().DoctorId;
                        checkData.SpecialistDoctorId = resultData.First().SpecialistDoctorId;
                        checkData.Day = resultData.First().Day;
                        checkData.StartTime = resultData.First().StartTime;
                        checkData.EndTime = resultData.First().EndTime;
                        checkData.Active = resultData.First().Active;
                        checkData.UpdateBy = resultData.First().CreateBy;
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
                return null;
            }


        }
    }
}
