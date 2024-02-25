using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class DailyStockService : IDailyStockService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IDailyStock _dailyStockRepo;

        public DailyStockService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IDailyStock dailyStockRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _dailyStockRepo = dailyStockRepo;
        }

        public async Task<List<DailyStockResponse>> GetDailyStock(DailyStockRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listDailyStock.Count() > 0)
                {
                    if (param.ProductTypeId != null)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listDailyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.ProductTypeId == param.ProductTypeId && x.StockDate.ToString("yyyyMMdd") == param.StockDate.ToString("yyyyMMdd"));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listDailyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.StockDate.ToString("yyyyMMdd") == param.StockDate.ToString("yyyyMMdd"));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _dailyStockRepo.GetDailyStock(param, cancellationToken);
                    return resultList.ToList();
                }

            }
            catch (Exception ex)
            {
                GeneralList._listDailyStock.Clear();
                throw;
            }
        }

        public async Task<List<DailyStockResponse>> CreateDailyStock(DailyStockAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _dailyStockRepo.CreateDailyStock(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listDailyStock.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listDailyStock.Clear();
                throw;
            }


        }

        public async Task<List<DailyStockResponse>> UpdateDailyStockBuy(DailyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _dailyStockRepo.UpdateDailyStockBuy(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listDailyStock.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.ProductTypeId = resultData.First().ProductTypeId;
                        checkData.ProductTypeName = resultData.First().ProductTypeName;
                        checkData.ProductId = resultData.First().ProductId;
                        checkData.ProductName = resultData.First().ProductName;
                        checkData.StockBuy = resultData.First().StockBuy;
                        checkData.StockBuyPrice = resultData.First().StockBuyPrice;
                        checkData.StockSell = resultData.First().StockSell;
                        checkData.StockSellPrice = resultData.First().StockSellPrice;
                        checkData.StockLast = resultData.First().StockLast;
                        checkData.StockDate = resultData.First().StockDate;
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
                GeneralList._listDailyStock.Clear();
                throw;
            }


        }
    }
}
