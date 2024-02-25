using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class MonthlyStockService : IMonthlyStockService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IMonthlyStock _monthlyStockRepo;

        public MonthlyStockService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IMonthlyStock monthlyStockRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _monthlyStockRepo = monthlyStockRepo;
        }

        public async Task<List<MonthlyStockResponse>> GetMonthlyStock(MonthlyStockRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listMonthlyStock.Count() > 0)
                {
                    if (param.ProductTypeId != null && param.ProductTypeId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listMonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.ProductTypeId == param.ProductTypeId && x.Month == param.Month && x.Year == param.Year);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listMonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.Month == param.Month && x.Year == param.Year);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _monthlyStockRepo.GetMonthlyStock(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listMonthlyStock.Clear();
                throw;
            }

        }

        public async Task<List<MonthlyStockResponse>> CreateMonthlyStock(MonthlyStockAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _monthlyStockRepo.CreateMonthlyStock(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listMonthlyStock.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listMonthlyStock.Clear();
                throw;
            }


        }

        public async Task<List<MonthlyStockResponse>> UpdateMonthlyStockBuy(MonthlyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _monthlyStockRepo.UpdateMontlyStockBuy(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listMonthlyStock.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.StockBuy = resultData.First().StockBuy;
                        checkData.StockBuyPrice = resultData.First().StockBuyPrice;
                        checkData.StockLast = resultData.First().StockLast;
                        checkData.UpdateBy = param.UpdateBy;
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
                GeneralList._listMonthlyStock.Clear();
                throw;
            }


        }

        public async Task<List<MonthlyStockResponse>> UpdateMonthlyStockSell(MonthlyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _monthlyStockRepo.UpdateMontlyStockSell(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listMonthlyStock.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.StockSell = resultData.First().StockBuy;
                        checkData.StockSellPrice = resultData.First().StockBuyPrice;
                        checkData.StockLast = resultData.First().StockLast;
                        checkData.UpdateBy = param.UpdateBy;
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
                GeneralList._listMonthlyStock.Clear();
                throw;
            }


        }
    }
}
