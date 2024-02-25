using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class StockOpnameRepository : IStockOpname
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public StockOpnameRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<StockOpnameResponse>> GetStockOpname(StockOpnameRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<StockOpnameResponse>? stockOpnameList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProductTypeId != null || param.ProductTypeId != 0)
                {
                    stockOpnameList = (from stockOp in _context.StockOpname
                                       join branch in _context.Branch on stockOp.BranchId equals branch.Id
                                       join company in _context.Company on stockOp.CompanyId equals company.Id
                                       join prodType in _context.ProductType on stockOp.ProductTypeId equals prodType.Id
                                       join prod in _context.Product on stockOp.ProductId equals prod.Id
                                       where stockOp.CompanyId == param.CompanyId && stockOp.BranchId == param.BranchId && stockOp.ProductTypeId == param.ProductTypeId
                                       orderby prod.ProductName
                                       select new StockOpnameResponse
                                       {
                                           Id = stockOp.Id,
                                           CompanyId = stockOp.CompanyId,
                                           CompanyName = company.Name,
                                           BranchId = stockOp.BranchId,
                                           BranchName = branch.Name,
                                           ProductTypeId = stockOp.ProductTypeId,
                                           ProductTypeName = prodType.ProductTypeName,
                                           ProductId = stockOp.ProductId,
                                           ProductName = prod.ProductName,
                                           Description = stockOp.Description,
                                           StockFirst = stockOp.StockFirst,
                                           StockOpnameDefault = stockOp.StockOpnameDefault,
                                           StockOpnameDate = stockOp.StockOpnameDate,
                                           CreateBy = branch.CreateBy,
                                           CreateDate = branch.CreateDate,
                                           UpdateBy = branch.UpdateBy,
                                           UpdateDate = branch.UpdateDate
                                       }).OrderBy(x => x.ProductName).AsNoTracking();
                }

                if (param.ProductTypeId == null || param.ProductTypeId == 0)
                {
                    stockOpnameList = (from stockOp in _context.StockOpname
                                       join branch in _context.Branch on stockOp.BranchId equals branch.Id
                                       join company in _context.Company on stockOp.CompanyId equals company.Id
                                       join prodType in _context.ProductType on stockOp.ProductTypeId equals prodType.Id
                                       join prod in _context.Product on stockOp.ProductId equals prod.Id
                                       where stockOp.CompanyId == param.CompanyId && stockOp.BranchId == param.BranchId
                                       orderby prod.ProductName
                                       select new StockOpnameResponse
                                       {
                                           Id = stockOp.Id,
                                           CompanyId = stockOp.CompanyId,
                                           CompanyName = company.Name,
                                           BranchId = stockOp.BranchId,
                                           BranchName = branch.Name,
                                           ProductTypeId = stockOp.ProductTypeId,
                                           ProductTypeName = prodType.ProductTypeName,
                                           ProductId = stockOp.ProductId,
                                           ProductName = prod.ProductName,
                                           Description = stockOp.Description,
                                           StockFirst = stockOp.StockFirst,
                                           StockOpnameDefault = stockOp.StockOpnameDefault,
                                           StockOpnameDate = stockOp.StockOpnameDate,
                                           CreateBy = branch.CreateBy,
                                           CreateDate = branch.CreateDate,
                                           UpdateBy = branch.UpdateBy,
                                           UpdateDate = branch.UpdateDate
                                       }).OrderBy(x => x.ProductName).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)stockOpnameList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = stockOpnameList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetStockOpname";
                if (ex.InnerException != null)
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }
                else
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return stockOpnameList;
            }
        }

        public async Task<IEnumerable<StockOpnameResponse>> CreateStockOpname(StockOpnameAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<StockOpnameResponse>? stockOpnameList = null;
            StockOpname stockOpnameAdd = new StockOpname();
            try
            {

                stockOpnameAdd.CompanyId = param.CompanyId;
                stockOpnameAdd.BranchId = param.BranchId;
                stockOpnameAdd.ProductTypeId = param.ProductTypeId;
                stockOpnameAdd.ProductId = param.ProductId;
                stockOpnameAdd.StockFirst = param.StockFirst;
                stockOpnameAdd.StockOpnameDefault = param.StockOpnameDefault;
                stockOpnameAdd.Month = param.Month;
                stockOpnameAdd.Year = param.Year;
                stockOpnameAdd.CreateBy = param.CreateBy;
                stockOpnameAdd.CreateDate = DateTime.Now;
                _context.StockOpname.Add(stockOpnameAdd);
                await _context.SaveChangesAsync();

                var thisYear = DateTime.Now.Year;
                var thisMonth = DateTime.Now.Month;
                var i = 1;
                while (thisYear >= param.Year)
                {
                    while (param.Month <= thisMonth)
                    {
                        var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId
                                        && x.ProductId == param.ProductId && x.Month == param.Month && x.Year == param.Year).AsNoTracking().FirstOrDefaultAsync();
                        if (monthlyStockUpdate != null)
                        {
                            if (i == 1)
                            {
                                monthlyStockUpdate.StockFirst = param.StockFirst;
                                monthlyStockUpdate.StockLast = (param.StockFirst + monthlyStockUpdate.StockBuy) - monthlyStockUpdate.StockSell;
                                monthlyStockUpdate.UpdateBy = param.CreateBy;
                                monthlyStockUpdate.UpdateDate = DateTime.Now;
                                _context.MonthlyStock.Update(monthlyStockUpdate);
                                await _context.SaveChangesAsync();

                                param.StockFirst = monthlyStockUpdate.StockLast;

                            }
                            else
                            {
                                monthlyStockUpdate.StockFirst = param.StockFirst;
                                monthlyStockUpdate.StockLast = (param.StockFirst + monthlyStockUpdate.StockBuy) - monthlyStockUpdate.StockSell;
                                monthlyStockUpdate.UpdateBy = param.CreateBy;
                                monthlyStockUpdate.UpdateDate = DateTime.Now;
                                _context.MonthlyStock.Update(monthlyStockUpdate);
                                await _context.SaveChangesAsync();
                            }

                        }
                        else
                        {
                            continue;
                        }

                        i++;
                        thisMonth = thisMonth - 1;
                    }
                    thisYear = thisYear - 1;
                }

                stockOpnameList = (from stockOp in _context.StockOpname
                                   join branch in _context.Branch on stockOp.BranchId equals branch.Id
                                   join company in _context.Company on stockOp.CompanyId equals company.Id
                                   join prodType in _context.ProductType on stockOp.ProductTypeId equals prodType.Id
                                   join prod in _context.Product on stockOp.ProductId equals prod.Id
                                   where stockOp.CompanyId == param.CompanyId && stockOp.BranchId == param.BranchId && stockOp.ProductTypeId == param.ProductTypeId && stockOp.ProductId == param.ProductId
                                   orderby prod.ProductName
                                   select new StockOpnameResponse
                                   {
                                       Id = stockOp.Id,
                                       CompanyId = stockOp.CompanyId,
                                       CompanyName = company.Name,
                                       BranchId = stockOp.BranchId,
                                       BranchName = branch.Name,
                                       ProductTypeId = stockOp.ProductTypeId,
                                       ProductTypeName = prodType.ProductTypeName,
                                       ProductId = stockOp.ProductId,
                                       ProductName = prod.ProductName,
                                       Description = stockOp.Description,
                                       StockFirst = stockOp.StockFirst,
                                       StockOpnameDefault = stockOp.StockOpnameDefault,
                                       StockOpnameDate = stockOp.StockOpnameDate,
                                       CreateBy = branch.CreateBy,
                                       CreateDate = branch.CreateDate,
                                       UpdateBy = branch.UpdateBy,
                                       UpdateDate = branch.UpdateDate
                                   }).Take(1).AsNoTracking();


                return stockOpnameList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateStockOpname";
                if (ex.InnerException != null)
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }
                else
                {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return stockOpnameList;
            }

        }
    }
}
