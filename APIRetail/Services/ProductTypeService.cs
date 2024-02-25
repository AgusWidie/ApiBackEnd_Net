using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class ProductTypeService : IProductTypeService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IProductType _productTypeRepo;

        public ProductTypeService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IProductType productTypeRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _productTypeRepo = productTypeRepo;
        }

        public async Task<List<ProductTypeResponse>> GetProductType(ProductTypeRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listProductType.Count() > 0)
                {
                    long Page = param.Page - 1;
                    var resultList = GeneralList._listProductType.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                    var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                    param.TotalPageSize = (long)TotalPageSize;
                    return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                }
                else
                {
                    var resultList = await _productTypeRepo.GetProductType(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listProductType.Clear();
                throw;
            }

        }

        public async Task<List<ProductTypeResponse>> CreateProductType(ProductTypeAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _productTypeRepo.CreateProductType(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listProductType.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listProductType.Clear();
                throw;
            }


        }

        public async Task<List<ProductTypeResponse>> UpdateProductType(ProductTypeUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _productTypeRepo.UpdateProductType(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listProductType.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.ProductTypeName = resultData.First().ProductTypeName;
                        checkData.Active = resultData.First().Active;
                        checkData.Description = resultData.First().Description;
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
                GeneralList._listProductType.Clear();
                throw;
            }


        }
    }
}
