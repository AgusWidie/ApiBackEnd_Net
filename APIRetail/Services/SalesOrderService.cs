using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;

namespace APIRetail.Services
{
    public class SalesOrderService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly ISalesOrder _salesOrderRepo;
        public readonly IDailyStock _dailyStockRepo;
        public readonly IMonthlyStock _monthlyStockRepo;
        public readonly IGenerateNumber _generateNumber;

        public SalesOrderService(IConfiguration Configuration, retail_systemContext context, ILogError logError, 
                                 ISalesOrder salesOrderRepo, IDailyStock dailyStockRepo, IMonthlyStock monthlyStockRepo, IGenerateNumber generateNumber)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _salesOrderRepo = salesOrderRepo;
            _dailyStockRepo = dailyStockRepo;
            _monthlyStockRepo = monthlyStockRepo;
            _generateNumber = generateNumber;
        }
        public async Task<List<SalesOrderResponse>> GetSalesOrder(SalesOrderRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listPurchaseOrder.Count() > 0)
                {
                    long Page = param.Page - 1;
                    var resultList = GeneralList._listSalesOrder.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.SalesOrderDate >= param.SalesOrderDateFrom && x.SalesOrderDate <= param.SalesOrderDateTo);
                    var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                    param.TotalPageSize = (long)TotalPageSize;
                    return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                }
                else
                {
                    var resultList = await _salesOrderRepo.GetSalesOrderHeader(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listSalesOrder.Clear();
                GeneralList._listSalesOrderDetail.Clear();
                throw;
            }

        }

        public async Task<List<SalesOrderResponse>> CreateSalesOrder(SalesOrderHeaderAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                param.InvoiceNo = await _generateNumber.GenerateInvoiceNo(cancellationToken); ;
                var resultData = await _salesOrderRepo.CreateSalesOrder(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listSalesOrder.Add(resultData.First());
                    SalesOrderRequest paramSalesDetail = new();
                    paramSalesDetail.SalesHeaderId = resultData.First().Id;

                    var resultDataSalesDetail = await _salesOrderRepo.GetSalesOrderDetail(paramSalesDetail, cancellationToken);
                    GeneralList._listSalesOrderDetail.AddRange(resultDataSalesDetail);

                    DailyStockListRequest paramDailyStock = new();
                    paramDailyStock.CompanyId = resultData.First().CompanyId;
                    paramDailyStock.BranchId = resultData.First().BranchId;
                    paramDailyStock.ProductTypeId = resultDataSalesDetail.First().ProductTypeId;
                    paramDailyStock.ProductId = resultDataSalesDetail.First().ProductId;

                    var resultDataDailyStock = await _dailyStockRepo.GetDailyStockList(paramDailyStock, cancellationToken);
                    var listDataDailyStock = GeneralList._listDailyStock.Where(x => x.CompanyId == paramDailyStock.CompanyId && x.BranchId == paramDailyStock.BranchId && x.ProductTypeId == paramDailyStock.ProductTypeId && x.ProductId == paramDailyStock.ProductId).FirstOrDefault();
                    if (listDataDailyStock == null)
                    {
                        GeneralList._listDailyStock.Add(resultDataDailyStock.First());
                    }
                    else
                    {
                        listDataDailyStock.StockSell = resultDataDailyStock.First().StockSell;
                        listDataDailyStock.StockSellPrice = resultDataDailyStock.First().StockSellPrice;
                        listDataDailyStock.StockBuy = resultDataDailyStock.First().StockBuy;
                        listDataDailyStock.StockBuyPrice = resultDataDailyStock.First().StockBuyPrice;
                        listDataDailyStock.StockLast = resultDataDailyStock.First().StockLast;
                        listDataDailyStock.UpdateBy = resultDataDailyStock.First().UpdateBy;
                        listDataDailyStock.UpdateDate = DateTime.Now;
                    }

                    MonthlyStockListRequest paramMonthlyStockList = new();
                    paramMonthlyStockList.CompanyId = paramDailyStock.CompanyId;
                    paramMonthlyStockList.BranchId = paramDailyStock.BranchId;
                    paramMonthlyStockList.ProductTypeId = paramDailyStock.ProductTypeId;
                    paramMonthlyStockList.ProductId = paramDailyStock.ProductId;
                    paramMonthlyStockList.Month = DateTime.Now.Month;
                    paramMonthlyStockList.Year = DateTime.Now.Year;

                    var resultDataMonthlyStock = await _monthlyStockRepo.GetMonthlyStockList(paramMonthlyStockList, cancellationToken);
                    var listDataMonthlyStock = GeneralList._listMonthlyStock.Where(x => x.CompanyId == paramMonthlyStockList.CompanyId && x.BranchId == paramMonthlyStockList.BranchId && x.ProductTypeId == paramMonthlyStockList.ProductTypeId && x.ProductId == paramMonthlyStockList.ProductId && x.Month == paramMonthlyStockList.Month && x.Year == paramMonthlyStockList.Year).FirstOrDefault();
                    if (listDataMonthlyStock == null)
                    {
                        GeneralList._listMonthlyStock.Add(resultDataMonthlyStock.First());
                    }
                    else
                    {
                        listDataMonthlyStock.StockSell = resultDataMonthlyStock.First().StockSell;
                        listDataMonthlyStock.StockSellPrice = resultDataMonthlyStock.First().StockSellPrice;
                        listDataMonthlyStock.StockBuy = resultDataMonthlyStock.First().StockBuy;
                        listDataMonthlyStock.StockBuyPrice = resultDataMonthlyStock.First().StockBuyPrice;
                        listDataMonthlyStock.StockLast = resultDataMonthlyStock.First().StockLast;
                        listDataMonthlyStock.UpdateBy = resultDataMonthlyStock.First().UpdateBy;
                        listDataMonthlyStock.UpdateDate = DateTime.Now;
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
                GeneralList._listSalesOrder.Clear();
                GeneralList._listSalesOrderDetail.Clear();
                throw;
            }


        }
    }
}
