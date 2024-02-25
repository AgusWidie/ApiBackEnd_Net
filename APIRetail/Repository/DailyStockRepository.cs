using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace APIRetail.Repository
{

    public class DailyStockRepository : IDailyStock
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public DailyStockRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<DailyStockResponse>> GetDailyStock(DailyStockRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DailyStockResponse>? dailyStockList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProductTypeId != null)
                {
                    dailyStockList = (from ds in _context.DailyStock
                                      join company in _context.Company on ds.CompanyId equals company.Id
                                      join branch in _context.Branch on ds.BranchId equals branch.Id
                                      join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                      join prod in _context.Product on ds.ProductId equals prod.Id
                                      where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId &&
                                            Convert.ToDateTime(ds.StockDate).Year == DateTime.Now.Year &&
                                            Convert.ToDateTime(ds.StockDate).Month == DateTime.Now.Month 
                                      orderby prod.ProductName
                                      select new DailyStockResponse
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
                                          StockDate = ds.StockDate,
                                          CreateBy = branch.CreateBy,
                                          CreateDate = branch.CreateDate,
                                          UpdateBy = branch.UpdateBy,
                                          UpdateDate = branch.UpdateDate
                                      }).OrderBy(x => x.ProductName).AsNoTracking();
                }
                else
                {
                    dailyStockList = (from ds in _context.DailyStock
                                      join company in _context.Company on ds.CompanyId equals company.Id
                                      join branch in _context.Branch on ds.BranchId equals branch.Id
                                      join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                      join prod in _context.Product on ds.ProductId equals prod.Id
                                      where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId &&
                                            Convert.ToDateTime(ds.StockDate).Year == DateTime.Now.Year &&
                                            Convert.ToDateTime(ds.StockDate).Month == DateTime.Now.Month 
                                      orderby prod.ProductName
                                      select new DailyStockResponse
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
                                          StockDate = ds.StockDate,
                                          CreateBy = branch.CreateBy,
                                          CreateDate = branch.CreateDate,
                                          UpdateBy = branch.UpdateBy,
                                          UpdateDate = branch.UpdateDate
                                      }).OrderBy(x => x.ProductName).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)dailyStockList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = dailyStockList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetDailyStock";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return dailyStockList;
            }
        }

        public async Task<IEnumerable<DailyStockResponse>> GetDailyStockList(DailyStockListRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DailyStockResponse>? dailyStockList = null;
            try
            {
                dailyStockList = (from ds in _context.DailyStock
                                  join company in _context.Company on ds.CompanyId equals company.Id
                                  join branch in _context.Branch on ds.BranchId equals branch.Id
                                  join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                  join prod in _context.Product on ds.ProductId equals prod.Id
                                  select new DailyStockResponse
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
                                      StockDate = ds.StockDate,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId).OrderBy(x => x.ProductName).AsNoTracking();


                return dailyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetDailyStock";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return dailyStockList;
            }
        }

        public async Task<IEnumerable<DailyStockResponse>> CreateDailyStock(DailyStockAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DailyStockResponse>? dailyStockList = null;
            DailyStock dailyStockAdd = new DailyStock();
            try
            {
                dailyStockAdd.CompanyId = param.CompanyId;
                dailyStockAdd.BranchId = param.BranchId;
                dailyStockAdd.ProductTypeId = param.ProductTypeId;
                dailyStockAdd.ProductId = param.ProductId;
                dailyStockAdd.StockFirst = param.StockFirst;
                dailyStockAdd.StockBuy = param.StockBuy;
                dailyStockAdd.StockBuyPrice = param.StockBuyPrice;
                dailyStockAdd.StockSell = param.StockSell;
                dailyStockAdd.StockSellPrice = param.StockSellPrice;
                dailyStockAdd.StockLast = (param.StockFirst + param.StockBuy) - param.StockSell;
                dailyStockAdd.CreateBy = param.CreateBy;
                dailyStockAdd.CreateDate = DateTime.Now;
                _context.DailyStock.Add(dailyStockAdd);
                await _context.SaveChangesAsync();

                dailyStockList = (from ds in _context.DailyStock
                                  join company in _context.Company on ds.CompanyId equals company.Id
                                  join branch in _context.Branch on ds.BranchId equals branch.Id
                                  join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                  join prod in _context.Product on ds.ProductId equals prod.Id
                                  where ds.CompanyId == param.CompanyId && ds.BranchId == param.BranchId && ds.StockDate.Year == DateTime.Now.Year
                                        && ds.StockDate.Month == DateTime.Now.Month && ds.StockDate.Day == DateTime.Now.Day
                                  select new DailyStockResponse
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
                                      StockDate = ds.StockDate,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).Take(1).OrderBy(x => x.ProductName).AsNoTracking();


                return dailyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateDailyStock";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return dailyStockList;
            }

        }

        public async Task<IEnumerable<DailyStockResponse>> UpdateDailyStockBuy(DailyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DailyStockResponse>? dailyStockList = null;
            try
            {
                var dailyStockUpdate = await _context.DailyStock.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (dailyStockUpdate != null)
                {
                    dailyStockUpdate.StockBuy = dailyStockUpdate.StockBuy + param.StockBuy;
                    dailyStockUpdate.StockBuyPrice = (dailyStockUpdate.StockBuyPrice + param.StockBuyPrice) / dailyStockUpdate.StockBuy;
                    dailyStockUpdate.StockLast = (dailyStockUpdate.StockFirst + dailyStockUpdate.StockBuy) - param.StockSell;
                    dailyStockUpdate.UpdateBy = param.UpdateBy;
                    dailyStockUpdate.UpdateDate = DateTime.Now;
                    _context.DailyStock.Update(dailyStockUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    dailyStockList = (from ds in _context.DailyStock
                                      join company in _context.Company on ds.CompanyId equals company.Id
                                      join branch in _context.Branch on ds.BranchId equals branch.Id
                                      join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                      join prod in _context.Product on ds.ProductId equals prod.Id
                                      where ds.Id == param.Id && ds.StockDate.Year == DateTime.Now.Year 
                                            && ds.StockDate.Month == DateTime.Now.Month && ds.StockDate.Day == DateTime.Now.Day
                                      select new DailyStockResponse
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
                                          StockDate = ds.StockDate,
                                          CreateBy = branch.CreateBy,
                                          CreateDate = branch.CreateDate,
                                          UpdateBy = branch.UpdateBy,
                                          UpdateDate = branch.UpdateDate
                                      }).Take(0).OrderBy(x => x.ProductName).AsNoTracking();
                }


                dailyStockList = (from ds in _context.DailyStock
                                  join company in _context.Company on ds.CompanyId equals company.Id
                                  join branch in _context.Branch on ds.BranchId equals branch.Id
                                  join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                  join prod in _context.Product on ds.ProductId equals prod.Id
                                  where ds.Id == param.Id && ds.StockDate.Year == DateTime.Now.Year
                                           && ds.StockDate.Month == DateTime.Now.Month && ds.StockDate.Day == DateTime.Now.Day
                                  select new DailyStockResponse
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
                                      StockDate = ds.StockDate,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).Take(1).OrderBy(x => x.ProductName).AsNoTracking();


                return dailyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateDailyStockBuy";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return dailyStockList;
            }

        }

        public async Task<IEnumerable<DailyStockResponse>> UpdateDailyStockSell(DailyStockUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DailyStockResponse>? dailyStockList = null;
            try
            {
                var dailyStockUpdate = await _context.DailyStock.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (dailyStockUpdate != null) {
                    dailyStockUpdate.StockSell = dailyStockUpdate.StockSell + param.StockSell;
                    dailyStockUpdate.StockSellPrice = (dailyStockUpdate.StockSellPrice + param.StockSellPrice) / dailyStockUpdate.StockSell;
                    dailyStockUpdate.StockLast = (dailyStockUpdate.StockFirst + dailyStockUpdate.StockBuy) - dailyStockUpdate.StockSell;
                    dailyStockUpdate.UpdateBy = param.UpdateBy;
                    dailyStockUpdate.UpdateDate = DateTime.Now;
                    _context.DailyStock.Update(dailyStockUpdate);
                    await _context.SaveChangesAsync();

                } else {
                    
                    dailyStockList = (from ds in _context.DailyStock
                                      join company in _context.Company on ds.CompanyId equals company.Id
                                      join branch in _context.Branch on ds.BranchId equals branch.Id
                                      join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                      join prod in _context.Product on ds.ProductId equals prod.Id
                                      where ds.Id == param.Id && ds.StockDate.Year == DateTime.Now.Year
                                            && ds.StockDate.Month == DateTime.Now.Month && ds.StockDate.Day == DateTime.Now.Day
                                      select new DailyStockResponse
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
                                          StockDate = ds.StockDate,
                                          CreateBy = branch.CreateBy,
                                          CreateDate = branch.CreateDate,
                                          UpdateBy = branch.UpdateBy,
                                          UpdateDate = branch.UpdateDate
                                      }).Take(0).OrderBy(x => x.ProductName).AsNoTracking();

                    return dailyStockList;
                }


                dailyStockList = (from ds in _context.DailyStock
                                  join company in _context.Company on ds.CompanyId equals company.Id
                                  join branch in _context.Branch on ds.BranchId equals branch.Id
                                  join prodType in _context.ProductType on ds.ProductTypeId equals prodType.Id
                                  join prod in _context.Product on ds.ProductId equals prod.Id
                                  where ds.Id == param.Id && ds.StockDate.Year == DateTime.Now.Year
                                        && ds.StockDate.Month == DateTime.Now.Month && ds.StockDate.Day == DateTime.Now.Day
                                  select new DailyStockResponse
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
                                      StockDate = ds.StockDate,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).Take(1).OrderBy(x => x.ProductName).AsNoTracking();


                return dailyStockList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateDailyStockSell";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return dailyStockList;
            }

        }
    }
}
