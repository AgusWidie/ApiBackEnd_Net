using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class DrugService : IDrugService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IDrug _drugRepo;

        public DrugService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IDrug drugRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _drugRepo = drugRepo;
        }

        public async Task<List<DrugResponse>> GetDrug(DrugSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listDrug.Count() > 0)
                {
                    if (param.DrugName == null || param.DrugName == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listDrug.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listDrug.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && param.DrugName.Contains(x.DrugName));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _drugRepo.GetDrug(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listDrug.Clear();
                return null;
            }

        }

        public async Task<List<DrugResponse>> CreateDrug(DrugRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _drugRepo.CreateDrug(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listDrug.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listDrug.Clear();
                return null;
            }


        }

        public async Task<List<DrugResponse>> UpdateDrug(DrugRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _drugRepo.UpdateDrug(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listDrug.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.ClinicId = resultData.First().ClinicId;
                        checkData.BranchId = resultData.First().BranchId;
                        checkData.DrugName = resultData.First().DrugName;
                        checkData.UnitType = resultData.First().UnitType;
                        checkData.Price = resultData.First().Price;
                        checkData.Stock = resultData.First().Stock;
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
                GeneralList._listDrug.Clear();
                return null;
            }


        }
    }
}
