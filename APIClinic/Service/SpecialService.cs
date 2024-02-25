using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class SpecialService : ISpecialService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ISpecialist _specialRepo;

        public SpecialService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ISpecialist specialRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _specialRepo = specialRepo;
        }

        public async Task<List<SpecialistResponse>> GetSpecialist(SpecialistSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listAbsenDoctor.Count() > 0)
                {
                    if (param.Name == null || param.Name == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSpeciallist.Where(x => param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSpeciallist;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _specialRepo.GetSpecialist(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<SpecialistResponse>> CreateSpecialist(SpecialistRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _specialRepo.CreateSpecialist(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listSpeciallist.Add(resultData.First());
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

        public async Task<List<SpecialistResponse>> UpdateSpecialist(SpecialistRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _specialRepo.UpdateSpecialist(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listSpeciallist.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
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
