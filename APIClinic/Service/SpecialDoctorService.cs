using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class SpecialDoctorService : ISpecialDoctorService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ISpecialDoctor _specialDoctorRepo;

        public SpecialDoctorService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ISpecialDoctor specialDoctorRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _specialDoctorRepo = specialDoctorRepo;
        }

        public async Task<List<SpecialistDoctorResponse>> GetSpecialistDoctor(SpecialistDoctorSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listSpecialDoctor.Count() > 0)
                {
                    if (param.Name == null || param.Name == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSpecialDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSpecialDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && param.Name.Contains(x.SpecialistName));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _specialDoctorRepo.GetSpecialistDoctor(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<SpecialistDoctorResponse>> CreateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _specialDoctorRepo.CreateSpecialistDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listSpecialDoctor.Add(resultData.First());
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

        public async Task<List<SpecialistDoctorResponse>> UpdateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _specialDoctorRepo.UpdateSpecialistDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listSpecialDoctor.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.ClinicId = resultData.First().ClinicId;
                        checkData.BranchId = resultData.First().BranchId;
                        checkData.DoctorId = resultData.First().DoctorId;
                        checkData.SpecialistId = resultData.First().SpecialistId;
                        checkData.Description = resultData.First().Description;
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
                return null;
            }


        }
    }
}
