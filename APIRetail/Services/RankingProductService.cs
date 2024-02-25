using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class RankingProductService : IRankingProductService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IRankingProduct _rankingProductRepo;

        public RankingProductService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IRankingProduct rankingProductRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _rankingProductRepo = rankingProductRepo;
        }

        public async Task<List<RankingProductResponse>> GetRankingProductSell(MonthlyStockRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listRankingProduct.Count() > 0)
                {
                    if (param.ProductTypeId != null && param.ProductTypeId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listRankingProduct.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.ProductTypeId == param.ProductTypeId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).OrderByDescending(x => x.StockSell).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listRankingProduct.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).OrderByDescending(x => x.StockSell).ToList();
                    }
                }
                else
                {
                    var resultList = await _rankingProductRepo.GetRangkingProductSell(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<RankingProductResponse>> GetRankingProductBuy(MonthlyStockRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listRankingProduct.Count() > 0)
                {
                    if (param.ProductTypeId != null && param.ProductTypeId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listRankingProduct.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.ProductTypeId == param.ProductTypeId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).OrderByDescending(x => x.StockBuy).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listRankingProduct.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).OrderByDescending(x => x.StockBuy).ToList();
                    }
                }
                else
                {
                    var resultList = await _rankingProductRepo.GetRangkingProductSell(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
