using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class DoctorService : IDoctorService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IDoctor _doctorRepo;

        public DoctorService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IDoctor doctorRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _doctorRepo = doctorRepo;
        }

        public async Task<List<DoctorResponse>> GetDoctor(DoctorSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listDoctor.Count() > 0)
                {
                    if (param.ClinicId != null && (param.DoctorName == null || param.DoctorName == ""))
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && param.DoctorName.Contains(x.DoctorName));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && param.DoctorName.Contains(x.DoctorName));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _doctorRepo.GetDoctor(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listDoctor.Clear();
                return null;
            }

        }

        public async Task<List<DoctorResponse>> CreateDoctor(DoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _doctorRepo.CreateDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listDoctor.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listDoctor.Clear();
                return null;
            }


        }

        public async Task<List<DoctorResponse>> UpdateDoctor(DoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _doctorRepo.UpdateDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listDoctor.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.ClinicId = resultData.First().ClinicId;
                        checkData.ClinicName = resultData.First().ClinicName;
                        checkData.BranchId = resultData.First().BranchId;
                        checkData.BranchName = resultData.First().BranchName;
                        checkData.DoctorName = resultData.First().DoctorName;
                        checkData.DateOfBirth = resultData.First().DateOfBirth;
                        checkData.NoTelephone = resultData.First().NoTelephone;
                        checkData.MobilePhone = resultData.First().MobilePhone;
                        checkData.Gender = resultData.First().Gender;
                        checkData.Education = resultData.First().Education;
                        checkData.StatusEmployee = resultData.First().StatusEmployee;
                        checkData.StatusDoctor = resultData.First().StatusDoctor;
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
                GeneralList._listDoctor.Clear();
                return null;
            }


        }
    }
}
