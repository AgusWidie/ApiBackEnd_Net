using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class SupplierService : ISupplierService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly ISupplier _supplierRepo;

        public SupplierService(IConfiguration Configuration, retail_systemContext context, ILogError logError, ISupplier supplierRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _supplierRepo = supplierRepo;
        }

        public async Task<List<SupplierResponse>> GetSupplier(SupplierRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listSupplier.Count() > 0)
                {
                    if (param.SupplierName != null && param.SupplierName != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSupplier.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && param.SupplierName.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listSupplier.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _supplierRepo.GetSupplier(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listSupplier.Clear();
                throw;
            }

        }

        public async Task<List<SupplierResponse>> CreateSupplier(SupplierAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _supplierRepo.CreateSupplier(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listSupplier.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listSupplier.Clear();
                throw;
            }


        }

        public async Task<List<SupplierResponse>> UpdateSupplier(SupplierUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _supplierRepo.UpdateSupplier(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listSupplier.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
                        checkData.Address = resultData.First().Address;
                        checkData.Telp = resultData.First().Telp;
                        checkData.Fax = resultData.First().Fax;
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
                GeneralList._listSupplier.Clear();
                throw;
            }


        }
    }
}
