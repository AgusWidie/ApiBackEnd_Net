using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class ProductService : IProductService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IProduct _productRepo;

        public ProductService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IProduct productRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _productRepo = productRepo;
        }

        public async Task<List<ProductResponse>> GetProduct(ProductRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listProduct.Count() > 0)
                {
                    if (param.ProductTypeId != null && param.ProductTypeId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProduct.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.ProductTypeId == param.ProductTypeId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProduct.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _productRepo.GetProduct(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listProduct.Clear();
                throw;
            }

        }

        public async Task<List<ProductResponse>> CreateProduct(ProductAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _productRepo.CreateProduct(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listProduct.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listProduct.Clear();
                throw;
            }


        }

        public async Task<List<ProductResponse>> UpdateProduct(ProductUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _productRepo.UpdateProduct(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listProduct.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.ProductTypeId = resultData.First().ProductTypeId;
                        checkData.ProductTypeName = resultData.First().ProductTypeName;
                        checkData.ProductNo = resultData.First().ProductNo;
                        checkData.ProductName = resultData.First().ProductName;
                        checkData.BuyPrice = resultData.First().BuyPrice;
                        checkData.SellPrice = resultData.First().SellPrice;
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
                GeneralList._listProduct.Clear();
                throw;
            }


        }
    }
}
