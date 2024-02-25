using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class LaboratoriumService : ILaboratoriumService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ILaboratorium _labRepo;

        public LaboratoriumService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ILaboratorium labRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _labRepo = labRepo;
        }

        public async Task<List<LaboratoriumResponse>> GetLaboratorium(LaboratoriumSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listLaboratorium.Count() > 0)
                {
                    if (param.LaboratoriumName != null && param.LaboratoriumName != "")
                    {
                        param.Page = param.Page - 1;
                        var resultList = GeneralList._listLaboratorium.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId);
                        var resultLaboratorium = resultList.Where(x => param.LaboratoriumName.Contains(x.LaboratoriumName));
                        var TotalPageSize = Math.Ceiling((decimal)resultLaboratorium.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultLaboratorium.Skip((int)param.Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        param.Page = param.Page - 1;
                        var resultList = GeneralList._listLaboratorium.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId);
                        var resultLaboratorium = resultList.Where(x => param.LaboratoriumName.Contains(x.LaboratoriumName));
                        var TotalPageSize = Math.Ceiling((decimal)resultLaboratorium.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultLaboratorium.Skip((int)param.Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _labRepo.GetLaboratorium(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listLaboratorium.Clear();
                return null;
            }

        }

        public async Task<List<LaboratoriumResponse>> CreateLaboratium(LaboratoriumRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _labRepo.CreateLaboratium(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listLaboratorium.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listLaboratorium.Clear();
                return null;
            }
        }

        public async Task<List<LaboratoriumResponse>> UpdateLaboratium(LaboratoriumRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _labRepo.UpdateLaboratorium(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listLaboratorium.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.LaboratoriumName = resultData.First().LaboratoriumName;
                        checkData.Price = resultData.First().Price;
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
                GeneralList._listLaboratorium.Clear();
                return null;
            }


        }
    }
}
