using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class RankingProductRepository : IRankingProduct
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public RankingProductRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<RankingProductResponse>> GetRangkingProductSell(MonthlyStockRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<RankingProductResponse>? rangkingProductList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProductTypeId != null || param.ProductTypeId != 0)
                {
                    rangkingProductList = (from ds in _context.MonthlyStock
                                           join company in _context.Company on ds.CompanyId equals company.Id
                                           join branch in _context.Branch on ds.BranchId equals branch.Id
                                           join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                           join prod in _context.Product on ds.ProductId equals prod.Id
                                           where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                                 && prodType.Id == param.ProductTypeId
                                           select new RankingProductResponse
                                           {
                                               Id = ds.Id,
                                               CompanyId = company.Id,
                                               CompanyName = company.Name,
                                               BranchId = branch.Id,
                                               BranchName = branch.Name,
                                               ProductTypeId = ds.ProductTypeId,
                                               ProductTypeName = prodType.ProductTypeName,
                                               ProductId = ds.ProductId,
                                               ProductName = prod.ProductName,
                                               StockBuy = ds.StockBuy,
                                               StockSell = ds.StockSell,
                                               Month = ds.Month,
                                               Year = ds.Year
                                           }).OrderByDescending(x => x.StockSell).AsNoTracking();
                }
                else
                {
                    rangkingProductList = (from ds in _context.MonthlyStock
                                           join company in _context.Company on ds.CompanyId equals company.Id
                                           join branch in _context.Branch on ds.BranchId equals branch.Id
                                           join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                           join prod in _context.Product on ds.ProductId equals prod.Id
                                           where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                           select new RankingProductResponse
                                           {
                                               Id = ds.Id,
                                               CompanyId = company.Id,
                                               CompanyName = company.Name,
                                               BranchId = branch.Id,
                                               BranchName = branch.Name,
                                               ProductTypeId = ds.ProductTypeId,
                                               ProductTypeName = prodType.ProductTypeName,
                                               ProductId = ds.ProductId,
                                               ProductName = prod.ProductName,
                                               StockBuy = ds.StockBuy,
                                               StockSell = ds.StockSell,
                                               Month = ds.Month,
                                               Year = ds.Year
                                           }).OrderByDescending(x => x.StockSell).AsNoTracking();
                }



                var TotalPageSize = Math.Ceiling((decimal)rangkingProductList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = rangkingProductList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetRangkingProductSell";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return rangkingProductList;
            }
        }

        public async Task<IEnumerable<RankingProductResponse>> GetRangkingProductBuy(MonthlyStockRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<RankingProductResponse>? rangkingProductList = null;
            try
            {
                if (param.ProductTypeId != null || param.ProductTypeId != 0)
                {
                    rangkingProductList = (from ds in _context.MonthlyStock
                                           join company in _context.Company on ds.CompanyId equals company.Id
                                           join branch in _context.Branch on ds.BranchId equals branch.Id
                                           join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                           join prod in _context.Product on ds.ProductId equals prod.Id
                                           where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                                 && prodType.Id == param.ProductTypeId
                                           select new RankingProductResponse
                                           {
                                               Id = ds.Id,
                                               CompanyId = company.Id,
                                               CompanyName = company.Name,
                                               BranchId = branch.Id,
                                               BranchName = branch.Name,
                                               ProductTypeId = ds.ProductTypeId,
                                               ProductTypeName = prodType.ProductTypeName,
                                               ProductId = ds.ProductId,
                                               ProductName = prod.ProductName,
                                               StockBuy = ds.StockBuy,
                                               StockSell = ds.StockSell,
                                               Month = ds.Month,
                                               Year = ds.Year
                                           }).OrderByDescending(x => x.StockBuy).AsNoTracking();
                }
                else
                {
                    rangkingProductList = (from ds in _context.MonthlyStock
                                           join company in _context.Company on ds.CompanyId equals company.Id
                                           join branch in _context.Branch on ds.BranchId equals branch.Id
                                           join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                           join prod in _context.Product on ds.ProductId equals prod.Id
                                           where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                           select new RankingProductResponse
                                           {
                                               Id = ds.Id,
                                               CompanyId = company.Id,
                                               CompanyName = company.Name,
                                               BranchId = branch.Id,
                                               BranchName = branch.Name,
                                               ProductTypeId = ds.ProductTypeId,
                                               ProductTypeName = prodType.ProductTypeName,
                                               ProductId = ds.ProductId,
                                               ProductName = prod.ProductName,
                                               StockBuy = ds.StockBuy,
                                               StockSell = ds.StockSell,
                                               Month = ds.Month,
                                               Year = ds.Year
                                           }).OrderByDescending(x => x.StockBuy).AsNoTracking();
                }



                return rangkingProductList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetRangkingProductBuy";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return rangkingProductList;
            }
        }
    }
}
