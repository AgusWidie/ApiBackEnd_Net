using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class MonthlyStockRepository : IMonthlyStock
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public MonthlyStockRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<MonthlyStockResponse>> GetMonthlyStock(MonthlyStockRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MonthlyStockResponse>? monthlyStockList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProductTypeId != null || param.ProductTypeId != 0)
                {
                    monthlyStockList = (from ds in _context.MonthlyStock
                                        join company in _context.Company on ds.CompanyId equals company.Id
                                        join branch in _context.Branch on ds.BranchId equals branch.Id
                                        join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                        join prod in _context.Product on ds.ProductId equals prod.Id
                                        where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                              && prodType.Id == param.ProductTypeId
                                        orderby prod.ProductName
                                        select new MonthlyStockResponse
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
                                            StockFirst = ds.StockFirst,
                                            StockBuy = ds.StockBuy,
                                            StockBuyPrice = ds.StockBuyPrice,
                                            StockSell = ds.StockSell,
                                            StockSellPrice = ds.StockSellPrice,
                                            StockLast = ds.StockLast,
                                            Month = ds.Month,
                                            Year = ds.Year,
                                            CreateBy = branch.CreateBy,
                                            CreateDate = branch.CreateDate,
                                            UpdateBy = branch.UpdateBy,
                                            UpdateDate = branch.UpdateDate
                                        }).OrderBy(x => x.ProductName).AsNoTracking();
                }
                else
                {
                    monthlyStockList = (from ds in _context.MonthlyStock
                                        join company in _context.Company on ds.CompanyId equals company.Id
                                        join branch in _context.Branch on ds.BranchId equals branch.Id
                                        join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                        join prod in _context.Product on ds.ProductId equals prod.Id
                                        where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                        orderby prod.ProductName
                                        select new MonthlyStockResponse
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
                                            StockFirst = ds.StockFirst,
                                            StockBuy = ds.StockBuy,
                                            StockBuyPrice = ds.StockBuyPrice,
                                            StockSell = ds.StockSell,
                                            StockSellPrice = ds.StockSellPrice,
                                            StockLast = ds.StockLast,
                                            Month = ds.Month,
                                            Year = ds.Year,
                                            CreateBy = branch.CreateBy,
                                            CreateDate = branch.CreateDate,
                                            UpdateBy = branch.UpdateBy,
                                            UpdateDate = branch.UpdateDate
                                        }).OrderBy(x => x.ProductName).AsNoTracking();
                }


                var TotalPageSize = Math.Ceiling((decimal)monthlyStockList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = monthlyStockList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetMonthlyStock";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return monthlyStockList;
            }
        }

        public async Task<IEnumerable<MonthlyStockResponse>> GetMonthlyStockList(MonthlyStockListRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MonthlyStockResponse>? monthlyStockList = null;
            try
            {
                monthlyStockList = (from ds in _context.MonthlyStock
                                    join company in _context.Company on ds.CompanyId equals company.Id
                                    join branch in _context.Branch on ds.BranchId equals branch.Id
                                    join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                    join prod in _context.Product on ds.ProductId equals prod.Id
                                    where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                          && ds.ProductTypeId == param.ProductTypeId && ds.ProductId == param.ProductId
                                    orderby prod.ProductName
                                    select new MonthlyStockResponse
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
                                        StockFirst = ds.StockFirst,
                                        StockBuy = ds.StockBuy,
                                        StockBuyPrice = ds.StockBuyPrice,
                                        StockSell = ds.StockSell,
                                        StockSellPrice = ds.StockSellPrice,
                                        StockLast = ds.StockLast,
                                        Month = ds.Month,
                                        Year = ds.Year,
                                        CreateBy = branch.CreateBy,
                                        CreateDate = branch.CreateDate,
                                        UpdateBy = branch.UpdateBy,
                                        UpdateDate = branch.UpdateDate
                                    }).OrderBy(x => x.ProductName).AsNoTracking();

                return monthlyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetMonthlyStockList";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return monthlyStockList;
            }
        }

        public async Task<IEnumerable<MonthlyStockResponse>> CreateMonthlyStock(MonthlyStockAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MonthlyStockResponse>? monthlyStockList = null;
            MonthlyStock monthlyStockAdd = new MonthlyStock();
            try
            {
                monthlyStockAdd.CompanyId = param.CompanyId;
                monthlyStockAdd.BranchId = param.BranchId;
                monthlyStockAdd.ProductTypeId = param.ProductTypeId;
                monthlyStockAdd.ProductId = param.ProductId;
                monthlyStockAdd.StockFirst = param.StockFirst;
                monthlyStockAdd.StockBuy = param.StockBuy;
                monthlyStockAdd.StockBuyPrice = param.StockBuyPrice;
                monthlyStockAdd.StockSell = param.StockSell;
                monthlyStockAdd.StockSellPrice = param.StockSellPrice;
                monthlyStockAdd.StockLast = param.StockFirst + param.StockBuy - param.StockSell;
                monthlyStockAdd.Month = param.Month;
                monthlyStockAdd.Year = param.Year;
                monthlyStockAdd.CreateBy = param.CreateBy;
                monthlyStockAdd.CreateDate = DateTime.Now;
                _context.MonthlyStock.Add(monthlyStockAdd);
                await _context.SaveChangesAsync();

                monthlyStockList = (from ds in _context.MonthlyStock
                                    join company in _context.Company on ds.CompanyId equals company.Id
                                    join branch in _context.Branch on ds.BranchId equals branch.Id
                                    join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                    join prod in _context.Product on ds.ProductId equals prod.Id
                                    where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.Month == param.Month && ds.Year == param.Year
                                          && prodType.Id == param.ProductTypeId
                                    orderby prod.ProductName
                                    select new MonthlyStockResponse
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
                                        StockFirst = ds.StockFirst,
                                        StockBuy = ds.StockBuy,
                                        StockBuyPrice = ds.StockBuyPrice,
                                        StockSell = ds.StockSell,
                                        StockSellPrice = ds.StockSellPrice,
                                        StockLast = ds.StockLast,
                                        Month = ds.Month,
                                        Year = ds.Year,
                                        CreateBy = branch.CreateBy,
                                        CreateDate = branch.CreateDate,
                                        UpdateBy = branch.UpdateBy,
                                        UpdateDate = branch.UpdateDate
                                    }).Take(1).AsNoTracking(); ;


                return monthlyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateMonthlyStock";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }  else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return monthlyStockList;
            }

        }
        public async Task<IEnumerable<MonthlyStockResponse>> UpdateMontlyStockBuy(MonthlyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MonthlyStockResponse>? monthlyStockList = null;
            try
            {
                var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (monthlyStockUpdate != null)
                {
                    monthlyStockUpdate.StockBuy = monthlyStockUpdate.StockBuy + param.StockBuy;
                    monthlyStockUpdate.StockBuyPrice = (monthlyStockUpdate.StockBuyPrice + param.StockBuyPrice) / monthlyStockUpdate.StockBuy;
                    monthlyStockUpdate.StockLast = monthlyStockUpdate.StockFirst + monthlyStockUpdate.StockBuy - param.StockSell;
                    monthlyStockUpdate.UpdateBy = param.UpdateBy;
                    monthlyStockUpdate.UpdateDate = DateTime.Now;
                    _context.MonthlyStock.Update(monthlyStockUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    monthlyStockList = (from ds in _context.MonthlyStock
                                        join company in _context.Company on ds.CompanyId equals company.Id
                                        join branch in _context.Branch on ds.BranchId equals branch.Id
                                        join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                        join prod in _context.Product on ds.ProductId equals prod.Id
                                        where ds.Id == param.Id && ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.ProductId == param.ProductId
                                        orderby prod.ProductName
                                        select new MonthlyStockResponse
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
                                            StockFirst = ds.StockFirst,
                                            StockBuy = ds.StockBuy,
                                            StockBuyPrice = ds.StockBuyPrice,
                                            StockSell = ds.StockSell,
                                            StockSellPrice = ds.StockSellPrice,
                                            StockLast = ds.StockLast,
                                            Month = ds.Month,
                                            Year = ds.Year,
                                            CreateBy = branch.CreateBy,
                                            CreateDate = branch.CreateDate,
                                            UpdateBy = branch.UpdateBy,
                                            UpdateDate = branch.UpdateDate
                                        }).Take(0).AsNoTracking(); ;

                    return monthlyStockList;
                }


                monthlyStockList = (from ds in _context.MonthlyStock
                                    join company in _context.Company on ds.CompanyId equals company.Id
                                    join branch in _context.Branch on ds.BranchId equals branch.Id
                                    join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                    join prod in _context.Product on ds.ProductId equals prod.Id
                                    where ds.Id == param.Id && ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.ProductId == param.ProductId
                                    orderby prod.ProductName
                                    select new MonthlyStockResponse
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
                                        StockFirst = ds.StockFirst,
                                        StockBuy = ds.StockBuy,
                                        StockBuyPrice = ds.StockBuyPrice,
                                        StockSell = ds.StockSell,
                                        StockSellPrice = ds.StockSellPrice,
                                        StockLast = ds.StockLast,
                                        Month = ds.Month,
                                        Year = ds.Year,
                                        CreateBy = branch.CreateBy,
                                        CreateDate = branch.CreateDate,
                                        UpdateBy = branch.UpdateBy,
                                        UpdateDate = branch.UpdateDate
                                    }).Take(1).AsNoTracking(); ;


                return monthlyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateMontlyStockBuy";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return monthlyStockList;
            }

        }

        public async Task<IEnumerable<MonthlyStockResponse>> UpdateMontlyStockSell(MonthlyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MonthlyStockResponse>? monthlyStockList = null;
            try
            {
                var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (monthlyStockUpdate != null)
                {
                    monthlyStockUpdate.StockSell = monthlyStockUpdate.StockSell + param.StockSell;
                    monthlyStockUpdate.StockSellPrice = (monthlyStockUpdate.StockSellPrice + param.StockSellPrice) / monthlyStockUpdate.StockSell;
                    monthlyStockUpdate.StockLast = monthlyStockUpdate.StockFirst + monthlyStockUpdate.StockBuy - monthlyStockUpdate.StockSell;
                    monthlyStockUpdate.UpdateBy = param.UpdateBy;
                    monthlyStockUpdate.UpdateDate = DateTime.Now;
                    _context.MonthlyStock.Update(monthlyStockUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    monthlyStockList = (from ds in _context.MonthlyStock
                                        join company in _context.Company on ds.CompanyId equals company.Id
                                        join branch in _context.Branch on ds.BranchId equals branch.Id
                                        join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                        join prod in _context.Product on ds.ProductId equals prod.Id
                                        where ds.Id == param.Id && ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.ProductId == param.ProductId
                                        orderby prod.ProductName
                                        select new MonthlyStockResponse
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
                                            StockFirst = ds.StockFirst,
                                            StockBuy = ds.StockBuy,
                                            StockBuyPrice = ds.StockBuyPrice,
                                            StockSell = ds.StockSell,
                                            StockSellPrice = ds.StockSellPrice,
                                            StockLast = ds.StockLast,
                                            Month = ds.Month,
                                            Year = ds.Year,
                                            CreateBy = branch.CreateBy,
                                            CreateDate = branch.CreateDate,
                                            UpdateBy = branch.UpdateBy,
                                            UpdateDate = branch.UpdateDate
                                        }).Take(0).AsNoTracking(); ;

                    return monthlyStockList;
                }


                monthlyStockList = (from ds in _context.MonthlyStock
                                    join company in _context.Company on ds.CompanyId equals company.Id
                                    join branch in _context.Branch on ds.BranchId equals branch.Id
                                    join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                    join prod in _context.Product on ds.ProductId equals prod.Id
                                    where ds.Id == param.Id && ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.ProductId == param.ProductId
                                    orderby prod.ProductName
                                    select new MonthlyStockResponse
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
                                        StockFirst = ds.StockFirst,
                                        StockBuy = ds.StockBuy,
                                        StockBuyPrice = ds.StockBuyPrice,
                                        StockSell = ds.StockSell,
                                        StockSellPrice = ds.StockSellPrice,
                                        StockLast = ds.StockLast,
                                        Month = ds.Month,
                                        Year = ds.Year,
                                        CreateBy = branch.CreateBy,
                                        CreateDate = branch.CreateDate,
                                        UpdateBy = branch.UpdateBy,
                                        UpdateDate = branch.UpdateDate
                                    }).Take(1).AsNoTracking();


                return monthlyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateMontlyStockSell";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return monthlyStockList;
            }

        }
    }
}
