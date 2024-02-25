using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace APIClinic.Service
{
    public class AbsenDoctorService : IAbsenDoctorService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IAbsenDoctor _absenDoctorRepo;

        public AbsenDoctorService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IAbsenDoctor absenDoctorRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _absenDoctorRepo = absenDoctorRepo;
        }

        public async Task<List<AbsenDoctorResponse>> GetAbsenDoctor(AbsenDoctorSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listAbsenDoctor.Count() > 0)
                {
                    if (param.Day == null || param.Day == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listAbsenDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && Convert.ToDateTime(x.StartTime) >= Convert.ToDateTime(param.StartTime) && Convert.ToDateTime(x.EndTime) <= Convert.ToDateTime(param.EndTime));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listAbsenDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && Convert.ToDateTime(x.StartTime) >= Convert.ToDateTime(param.StartTime) && Convert.ToDateTime(x.EndTime) <= Convert.ToDateTime(param.EndTime));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _absenDoctorRepo.GetAbsenDoctor(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listAbsenDoctor.Clear();
                return null;
            }

        }

        public async Task<List<AbsenDoctorResponse>> CreateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _absenDoctorRepo.CreateAbsenDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listAbsenDoctor.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listAbsenDoctor.Clear();
                return null;
            }


        }

        public async Task<List<AbsenDoctorResponse>> UpdateAbsenDoctor(AbsenDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _absenDoctorRepo.UpdateAbsenDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listAbsenDoctor.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.AbsenType = resultData.First().AbsenType;
                        checkData.StartTime = resultData.First().StartTime;
                        checkData.EndTime = resultData.First().EndTime;
                        checkData.Day = resultData.First().Day;
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
                GeneralList._listAbsenDoctor.Clear();
                return null;
            }


        }
    }
}
