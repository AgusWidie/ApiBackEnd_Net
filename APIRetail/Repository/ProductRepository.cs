using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class ProductRepository : IProduct
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public ProductRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<ProductResponse>> GetProduct(ProductRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProductResponse>? productList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProductTypeId != null || param.ProductTypeId != 0)
                {
                    productList = (from prod in _context.Product
                                   join branch in _context.Branch on prod.BranchId equals branch.Id
                                   join company in _context.Company on prod.CompanyId equals company.Id
                                   join prodType in _context.ProductType on prod.ProductTypeId equals prodType.Id
                                   where prod.CompanyId == param.CompanyId && prod.BranchId == param.BranchId && prod.ProductTypeId == param.ProductTypeId
                                   select new ProductResponse
                                   {
                                       Id = prod.Id,
                                       CompanyId = prod.CompanyId,
                                       CompanyName = company.Name,
                                       BranchId = prod.BranchId,
                                       BranchName = branch.Name,
                                       ProductTypeId = prod.ProductTypeId,
                                       ProductTypeName = prodType.ProductTypeName,
                                       ProductNo = prod.ProductNo,
                                       ProductName = prod.ProductName,
                                       BuyPrice = prod.BuyPrice,
                                       SellPrice = prod.SellPrice,
                                       CreateBy = branch.CreateBy,
                                       CreateDate = branch.CreateDate,
                                       UpdateBy = branch.UpdateBy,
                                       UpdateDate = branch.UpdateDate
                                   }).OrderBy(x => x.ProductName).AsNoTracking();



                }

                if (param.ProductTypeId == null || param.ProductTypeId == 0)
                {
                    productList = (from prod in _context.Product
                                   join branch in _context.Branch on prod.BranchId equals branch.Id
                                   join company in _context.Company on prod.CompanyId equals company.Id
                                   join prodType in _context.ProductType on prod.ProductTypeId equals prodType.Id
                                   where prod.CompanyId == param.CompanyId && prod.BranchId == param.BranchId
                                   select new ProductResponse
                                   {
                                       Id = prod.Id,
                                       CompanyId = prod.CompanyId,
                                       CompanyName = company.Name,
                                       BranchId = prod.BranchId,
                                       BranchName = branch.Name,
                                       ProductTypeId = prod.ProductTypeId,
                                       ProductTypeName = prodType.ProductTypeName,
                                       ProductNo = prod.ProductNo,
                                       ProductName = prod.ProductName,
                                       BuyPrice = prod.BuyPrice,
                                       SellPrice = prod.SellPrice,
                                       CreateBy = branch.CreateBy,
                                       CreateDate = branch.CreateDate,
                                       UpdateBy = branch.UpdateBy,
                                       UpdateDate = branch.UpdateDate
                                   }).OrderBy(x => x.ProductName).AsNoTracking();


                }

                var TotalPageSize = Math.Ceiling((decimal)productList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = productList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetProduct";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return productList;
            }
        }

        public async Task<IEnumerable<ProductResponse>> CreateProduct(ProductAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProductResponse>? productList = null;
            Product productAdd = new Product();
            try
            {
                productAdd.CompanyId = param.CompanyId;
                productAdd.BranchId = param.BranchId;
                productAdd.ProductTypeId = param.ProductTypeId;
                productAdd.ProductNo = param.ProductNo;
                productAdd.ProductName = param.ProductName;
                productAdd.BuyPrice = param.BuyPrice;
                productAdd.SellPrice = param.SellPrice;
                productAdd.CreateBy = param.CreateBy;
                productAdd.CreateDate = DateTime.Now;
                _context.Product.Add(productAdd);
                await _context.SaveChangesAsync();

                productList = (from prod in _context.Product
                               join branch in _context.Branch on prod.BranchId equals branch.Id
                               join company in _context.Company on prod.CompanyId equals company.Id
                               join prodType in _context.ProductType on prod.ProductTypeId equals prodType.Id
                               where prod.CompanyId == param.CompanyId && prod.BranchId == param.BranchId
                                     && prod.ProductTypeId == param.ProductTypeId && prod.ProductName == param.ProductName
                               select new ProductResponse
                               {
                                   Id = prod.Id,
                                   CompanyId = prod.CompanyId,
                                   CompanyName = company.Name,
                                   BranchId = prod.BranchId,
                                   BranchName = branch.Name,
                                   ProductTypeId = prod.ProductTypeId,
                                   ProductTypeName = prodType.ProductTypeName,
                                   ProductNo = prod.ProductNo,
                                   ProductName = prod.ProductName,
                                   BuyPrice = prod.BuyPrice,
                                   SellPrice = prod.SellPrice,
                                   CreateBy = branch.CreateBy,
                                   CreateDate = branch.CreateDate,
                                   UpdateBy = branch.UpdateBy,
                                   UpdateDate = branch.UpdateDate
                               }).Take(1).AsNoTracking();


                return productList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateProduct";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return productList;
            }

        }

        public async Task<IEnumerable<ProductResponse>> UpdateProduct(ProductUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProductResponse>? productList = null;
            Product productAdd = new Product();
            try
            {
                var productUpdate = await _context.Product.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (productUpdate != null)
                {
                    productUpdate.CompanyId = param.CompanyId;
                    productUpdate.BranchId = param.BranchId;
                    productUpdate.ProductTypeId = param.ProductTypeId;
                    productUpdate.ProductNo = param.ProductNo;
                    productUpdate.ProductName = param.ProductName;
                    productUpdate.BuyPrice = param.BuyPrice;
                    productUpdate.SellPrice = param.SellPrice;
                    productUpdate.UpdateBy = param.UpdateBy;
                    productUpdate.UpdateDate = DateTime.Now;
                    _context.Product.Update(productAdd);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    productList = (from prod in _context.Product
                                   join branch in _context.Branch on prod.BranchId equals branch.Id
                                   join company in _context.Company on prod.CompanyId equals company.Id
                                   join prodType in _context.ProductType on prod.ProductTypeId equals prodType.Id
                                   where prod.CompanyId == param.CompanyId && prod.BranchId == param.BranchId
                                         && prod.ProductTypeId == param.ProductTypeId && prod.Id == param.Id
                                   select new ProductResponse
                                   {
                                       Id = prod.Id,
                                       CompanyId = prod.CompanyId,
                                       CompanyName = company.Name,
                                       BranchId = prod.BranchId,
                                       BranchName = branch.Name,
                                       ProductTypeId = prod.ProductTypeId,
                                       ProductTypeName = prodType.ProductTypeName,
                                       ProductNo = prod.ProductNo,
                                       ProductName = prod.ProductName,
                                       BuyPrice = prod.BuyPrice,
                                       SellPrice = prod.SellPrice,
                                       CreateBy = branch.CreateBy,
                                       CreateDate = branch.CreateDate,
                                       UpdateBy = branch.UpdateBy,
                                       UpdateDate = branch.UpdateDate
                                   }).Take(0).AsNoTracking();

                    return productList;
                }


                productList = (from prod in _context.Product
                               join branch in _context.Branch on prod.BranchId equals branch.Id
                               join company in _context.Company on prod.CompanyId equals company.Id
                               join prodType in _context.ProductType on prod.ProductTypeId equals prodType.Id
                               where prod.CompanyId == param.CompanyId && prod.BranchId == param.BranchId
                                     && prod.ProductTypeId == param.ProductTypeId && prod.Id == param.Id
                               select new ProductResponse
                               {
                                   Id = prod.Id,
                                   CompanyId = prod.CompanyId,
                                   CompanyName = company.Name,
                                   BranchId = prod.BranchId,
                                   BranchName = branch.Name,
                                   ProductTypeId = prod.ProductTypeId,
                                   ProductTypeName = prodType.ProductTypeName,
                                   ProductNo = prod.ProductNo,
                                   ProductName = prod.ProductName,
                                   BuyPrice = prod.BuyPrice,
                                   SellPrice = prod.SellPrice,
                                   CreateBy = branch.CreateBy,
                                   CreateDate = branch.CreateDate,
                                   UpdateBy = branch.UpdateBy,
                                   UpdateDate = branch.UpdateDate
                               }).Take(1).AsNoTracking();


                return productList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateProduct";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return productList;
            }

        }
    }
}
