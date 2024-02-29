using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class PurchaseOrderRepository : IPurchaseOrder
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IGenerateNumber _generateNumber;

        public PurchaseOrderRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError, IGenerateNumber generateNumber)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _generateNumber = generateNumber;
        }

        public async Task<IEnumerable<PurchaseOrderResponse>> GetPurchaseOrderHeader(PurchaseOrderRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PurchaseOrderResponse>? purchaseOrderList = null;
            try
            {
                long Page = param.Page - 1;
                purchaseOrderList = (from pur in _context.PurchaseOrderHeader
                                     join branch in _context.Branch on pur.BranchId equals branch.Id
                                     join company in _context.Company on pur.CompanyId equals company.Id
                                     join sup in _context.Supplier on pur.SupplierId equals sup.Id
                                     where pur.CompanyId == param.CompanyId && pur.BranchId == param.BranchId
                                     && pur.PurchaseDate >= param.PurchaseDateFrom && pur.PurchaseDate <= param.PurchaseDateTo
                                     select new PurchaseOrderResponse
                                     {
                                         Id = pur.Id,
                                         CompanyId = pur.CompanyId,
                                         CompanyName = company.Name,
                                         BranchId = pur.BranchId,
                                         BranchName = branch.Name,
                                         PurchaseNo = pur.PurchaseNo,
                                         PurchaseDate = pur.PurchaseDate,
                                         SupplierId = pur.SupplierId,
                                         SupplierName = sup.Name,
                                         Quantity = pur.Quantity,
                                         Total = pur.Total,
                                         CreateBy = pur.CreateBy,
                                         CreateDate = pur.CreateDate,
                                         UpdateBy = pur.UpdateBy,
                                         UpdateDate = pur.UpdateDate
                                     }).OrderByDescending(x => x.PurchaseNo).AsNoTracking();

                var TotalPageSize = Math.Ceiling((decimal)purchaseOrderList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = purchaseOrderList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetPurchaseOrderHeader";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return purchaseOrderList;
            }
        }

        public async Task<IEnumerable<PurchaseOrderDetailResponse>> GetPurchaseOrderDetail(PurchaseOrderRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PurchaseOrderDetailResponse>? purchaseOrderDetailList = null;
            try
            {

                purchaseOrderDetailList = (from pur in _context.PurchaseOrderDetail
                                           join prod in _context.Product on pur.ProductId equals prod.Id
                                           join prodType in _context.ProductType on pur.ProductTypeId equals prodType.Id
                                           where pur.PurchaseHeaderId == param.PurchaseHeaderId
                                           select new PurchaseOrderDetailResponse
                                           {
                                               PurchaseHeaderId = pur.PurchaseHeaderId,
                                               ProductTypeId = pur.ProductTypeId,
                                               ProductTypeName = prodType.ProductTypeName,
                                               ProductId = pur.ProductId,
                                               ProductName = prod.ProductName,
                                               Quantity = pur.Quantity,
                                               Price = pur.Price,
                                               Subtotal = pur.Subtotal,
                                               CreateBy = pur.CreateBy,
                                               CreateDate = pur.CreateDate
                                           }).OrderBy(x => x.ProductName).AsNoTracking();

                return purchaseOrderDetailList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetPurchaseOrderDetail";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return purchaseOrderDetailList;
            }
        }

        public async Task<IEnumerable<PurchaseOrderResponse>> CreatePurchaseOrder(PurchaseOrderHeaderAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PurchaseOrderResponse>? purchaseOrderList = null;
            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    param.Total = 0;
                    param.Quantity = 0;
                    PurchaseOrderHeader purchaseOrderHeader = new PurchaseOrderHeader();
                    purchaseOrderHeader.CompanyId = param.CompanyId;
                    purchaseOrderHeader.BranchId = param.BranchId;
                    purchaseOrderHeader.PurchaseNo = param.PurchaseNo;
                    purchaseOrderHeader.PurchaseDate = DateTime.Now;
                    purchaseOrderHeader.SupplierId = param.SupplierId;
                    purchaseOrderHeader.PurchaseDate = param.PurchaseDate;
                    purchaseOrderHeader.Total = param.Total;
                    purchaseOrderHeader.CreateBy = param.CreateBy;
                    purchaseOrderHeader.CreateDate = DateTime.Now;
                    _context.PurchaseOrderHeader.Add(purchaseOrderHeader);
                    await _context.SaveChangesAsync();

                    foreach (var data in param.PurchaseOrderDetailAddRequest) {
                        PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
                        purchaseOrderDetail.PurchaseHeaderId = purchaseOrderHeader.Id;
                        purchaseOrderDetail.ProductTypeId = data.ProductTypeId;
                        purchaseOrderDetail.ProductId = data.ProductId;
                        purchaseOrderDetail.Quantity = data.Quantity;
                        purchaseOrderDetail.Price = data.Price;
                        purchaseOrderDetail.Subtotal = data.Quantity * data.Price;
                        param.Total = param.Total + (data.Quantity * data.Price);
                        param.Quantity = param.Quantity + data.Quantity;
                        purchaseOrderDetail.CreateBy = data.CreateBy;
                        purchaseOrderDetail.CreateDate = DateTime.Now;
                        _context.PurchaseOrderDetail.Add(purchaseOrderDetail);
                        await _context.SaveChangesAsync();

                     
                        var dailyStockUpdate = await _context.DailyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.ProductTypeId == data.ProductTypeId && x.ProductId == data.ProductId 
                                                     && x.StockDate.Year == DateTime.Now.Year && x.StockDate.Month == DateTime.Now.Month && x.StockDate.Day == DateTime.Now.Day).AsNoTracking().FirstOrDefaultAsync();
                        
                        if (dailyStockUpdate != null) {
                            dailyStockUpdate.StockBuy = dailyStockUpdate.StockBuy + data.Quantity;
                            dailyStockUpdate.StockBuyPrice = (dailyStockUpdate.StockBuyPrice + (data.Quantity * data.Price)) / dailyStockUpdate.StockBuy;
                            dailyStockUpdate.StockLast = (dailyStockUpdate.StockFirst + dailyStockUpdate.StockBuy) - dailyStockUpdate.StockSell;
                            dailyStockUpdate.UpdateBy = param.CreateBy;
                            dailyStockUpdate.UpdateDate = DateTime.Now;
                            _context.DailyStock.Update(dailyStockUpdate);
                            await _context.SaveChangesAsync();

                        } else {

                            DailyStock dailyStock = new DailyStock();
                            dailyStock.CompanyId = param.CompanyId;
                            dailyStock.BranchId = param.BranchId;
                            dailyStock.ProductTypeId = data.ProductTypeId;
                            dailyStock.ProductId = data.ProductId;
                            dailyStock.StockFirst = data.Quantity;
                            dailyStock.StockBuy = data.Quantity;
                            dailyStock.StockBuyPrice = data.Quantity * data.Price;
                            dailyStock.StockSell = 0;
                            dailyStock.StockSellPrice = 0;
                            dailyStock.StockLast = data.Quantity;
                            dailyStock.CreateBy = param.CreateBy;
                            dailyStock.CreateDate = param.CreateDate;
                            _context.DailyStock.Add(dailyStock);
                            await _context.SaveChangesAsync();
                        }

                        var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month && x.ProductId == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                        if (monthlyStockUpdate != null) {
                            monthlyStockUpdate.StockBuy = monthlyStockUpdate.StockBuy + data.Quantity;
                            monthlyStockUpdate.StockBuyPrice = (monthlyStockUpdate.StockBuyPrice + (data.Quantity * data.Price)) / monthlyStockUpdate.StockBuy;
                            monthlyStockUpdate.StockLast = (monthlyStockUpdate.StockFirst + monthlyStockUpdate.StockBuy) - monthlyStockUpdate.StockSell;
                            monthlyStockUpdate.UpdateBy = param.CreateBy;
                            monthlyStockUpdate.UpdateDate = DateTime.Now;
                            _context.MonthlyStock.Update(monthlyStockUpdate);
                            await _context.SaveChangesAsync();

                            var updateProduct = await _context.Product.Where(x => x.ProductTypeId == data.ProductTypeId && x.Id == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (updateProduct != null) {
                                updateProduct.BuyPrice = monthlyStockUpdate.StockBuyPrice;
                                _context.Product.Update(updateProduct);
                                await _context.SaveChangesAsync();
                            }

                        } else {
                            MonthlyStock monthlyStock = new MonthlyStock();
                            monthlyStock.CompanyId = param.CompanyId;
                            monthlyStock.BranchId = param.BranchId;
                            monthlyStock.ProductTypeId = data.ProductTypeId;
                            monthlyStock.ProductId = data.ProductId;
                            monthlyStock.StockFirst = data.Quantity;
                            monthlyStock.StockBuy = data.Quantity;
                            monthlyStock.StockBuyPrice = data.Quantity * data.Price;
                            monthlyStock.StockSell = 0;
                            monthlyStock.StockSellPrice = 0;
                            monthlyStock.StockLast = data.Quantity;
                            monthlyStock.Month = DateTime.Now.Month;
                            monthlyStock.Year = DateTime.Now.Year;
                            monthlyStock.CreateBy = param.CreateBy;
                            monthlyStock.CreateDate = param.CreateDate;
                            _context.MonthlyStock.Add(monthlyStock);
                            await _context.SaveChangesAsync();

                            var updateProduct = await _context.Product.Where(x => x.ProductTypeId == data.ProductTypeId && x.Id == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (updateProduct != null)
                            {
                                updateProduct.BuyPrice = monthlyStock.StockBuyPrice;
                                _context.Product.Update(updateProduct);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }

                    var purchaseOrder = await _context.PurchaseOrderHeader.Where(x => x.Id == purchaseOrderHeader.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (purchaseOrder != null)
                    {
                        purchaseOrder.Total = param.Total;
                        purchaseOrder.Quantity = param.Quantity;
                        _context.PurchaseOrderHeader.Update(purchaseOrder);
                        await _context.SaveChangesAsync();
                    }

                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();
                    purchaseOrderList = (from pur in _context.PurchaseOrderHeader
                                         join branch in _context.Branch on pur.BranchId equals branch.Id
                                         join company in _context.Company on pur.CompanyId equals company.Id
                                         join sup in _context.Supplier on pur.SupplierId equals sup.Id
                                         where pur.CompanyId == param.CompanyId && pur.BranchId == param.BranchId
                                         && pur.PurchaseNo == param.PurchaseNo
                                         orderby pur.PurchaseDate
                                         select new PurchaseOrderResponse
                                         {
                                             Id = pur.Id,
                                             CompanyId = pur.CompanyId,
                                             CompanyName = company.Name,
                                             BranchId = pur.BranchId,
                                             BranchName = branch.Name,
                                             PurchaseNo = pur.PurchaseNo,
                                             PurchaseDate = pur.PurchaseDate,
                                             SupplierId = pur.SupplierId,
                                             SupplierName = sup.Name,
                                             Total = pur.Total,
                                             Quantity = pur.Quantity,
                                             CreateBy = branch.CreateBy,
                                             CreateDate = branch.CreateDate,
                                             UpdateBy = branch.UpdateBy,
                                             UpdateDate = branch.UpdateDate                                          
                                         }).Take(1).AsNoTracking();

                    return purchaseOrderList;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();

                    LogErrorAddRequest logDataError = new LogErrorAddRequest();
                    logDataError.ServiceName = "CreatePurchaseOrder";
                    if (ex.InnerException != null) {
                        logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                    } else {
                        logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                    }
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                    return purchaseOrderList;
                }
            }

        }

        public async Task<IEnumerable<PurchaseOrderResponse>> UpdatePuchaseOrder(PurchaseOrderHeaderUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<PurchaseOrderResponse>? purchaseOrderList = null;
            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    param.Total = 0;
                    param.Quantity = 0;

                    var puchaseOrderHeader = await _context.PurchaseOrderHeader.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (puchaseOrderHeader != null)
                    {
                        puchaseOrderHeader.CompanyId = param.CompanyId;
                        puchaseOrderHeader.BranchId = param.BranchId;
                        puchaseOrderHeader.PurchaseNo = param.PurchaseNo;
                        puchaseOrderHeader.PurchaseDate = param.PurchaseDate;
                        puchaseOrderHeader.SupplierId = param.SupplierId;
                        puchaseOrderHeader.Total = param.Total;
                        puchaseOrderHeader.UpdateBy = param.UpdateBy;
                        puchaseOrderHeader.UpdateDate = DateTime.Now;
                        _context.PurchaseOrderHeader.Update(puchaseOrderHeader);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        purchaseOrderList = (from pur in _context.PurchaseOrderHeader
                                             join branch in _context.Branch on pur.BranchId equals branch.Id
                                             join company in _context.Company on pur.CompanyId equals company.Id
                                             join sup in _context.Supplier on pur.SupplierId equals sup.Id
                                             where pur.Id == param.Id
                                             orderby pur.PurchaseDate
                                             select new PurchaseOrderResponse
                                             {
                                                 Id = pur.Id,
                                                 CompanyId = pur.CompanyId,
                                                 CompanyName = company.Name,
                                                 BranchId = pur.BranchId,
                                                 BranchName = branch.Name,
                                                 PurchaseNo = pur.PurchaseNo,
                                                 PurchaseDate = pur.PurchaseDate,
                                                 SupplierId = pur.SupplierId,
                                                 SupplierName = sup.Name,
                                                 Total = pur.Total,
                                                 Quantity = pur.Quantity,
                                                 CreateBy = pur.CreateBy,
                                                 CreateDate = pur.CreateDate,
                                                 UpdateBy = pur.UpdateBy,
                                                 UpdateDate = pur.UpdateDate
                                             }).Take(0).AsNoTracking();

                        return purchaseOrderList;
                    }


                    foreach (var data in param.PurchaseOrderDetailUpdateRequest)
                    {
                        var purchaseOrderDetailRemove = await _context.PurchaseOrderDetail.Where(x => x.ProductId == data.ProductId && x.PurchaseHeaderId == param.Id).AsNoTracking().FirstOrDefaultAsync();
                        if (purchaseOrderDetailRemove != null)
                        {
                            var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month && x.ProductId == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (monthlyStockUpdate != null)
                            {
                                monthlyStockUpdate.StockBuy = monthlyStockUpdate.StockBuy - purchaseOrderDetailRemove.Quantity;
                                monthlyStockUpdate.StockBuyPrice = (monthlyStockUpdate.StockBuyPrice - (purchaseOrderDetailRemove.Quantity * purchaseOrderDetailRemove.Price)) / monthlyStockUpdate.StockBuy;
                                monthlyStockUpdate.StockLast = monthlyStockUpdate.StockFirst + monthlyStockUpdate.StockBuy - monthlyStockUpdate.StockSell;
                                monthlyStockUpdate.UpdateBy = param.UpdateBy;
                                monthlyStockUpdate.UpdateDate = DateTime.Now;
                                _context.MonthlyStock.Update(monthlyStockUpdate);
                                await _context.SaveChangesAsync();
                            }
                         
                            _context.PurchaseOrderDetail.Remove(purchaseOrderDetailRemove);
                            await _context.SaveChangesAsync();
                        }

                        PurchaseOrderDetail purchaseOrderDetail = new PurchaseOrderDetail();
                        purchaseOrderDetail.PurchaseHeaderId = purchaseOrderDetail.Id;
                        purchaseOrderDetail.ProductTypeId = data.ProductTypeId;
                        purchaseOrderDetail.ProductId = data.ProductId;
                        purchaseOrderDetail.Quantity = data.Quantity;
                        purchaseOrderDetail.Price = data.Price;
                        purchaseOrderDetail.Subtotal = data.Quantity * data.Price;
                        param.Total = param.Total + (data.Quantity * data.Price);
                        param.Quantity = param.Quantity + data.Quantity;
                        purchaseOrderDetail.CreateBy = data.CreateBy;
                        purchaseOrderDetail.CreateDate = DateTime.Now;
                        _context.PurchaseOrderDetail.Add(purchaseOrderDetail);
                        await _context.SaveChangesAsync();

                        var dailyStockUpdate = await _context.DailyStock.Where(x => x.ProductTypeId == data.ProductTypeId && x.ProductId == data.ProductId 
                                               && x.StockDate.Year == DateTime.Now.Year && x.StockDate.Month == DateTime.Now.Month && x.StockDate.Day == DateTime.Now.Day).AsNoTracking().FirstOrDefaultAsync();
                        if (dailyStockUpdate != null) {
                            dailyStockUpdate.StockBuy = dailyStockUpdate.StockBuy + data.Quantity;
                            dailyStockUpdate.StockBuyPrice = (dailyStockUpdate.StockBuyPrice + (data.Quantity * data.Price)) / dailyStockUpdate.StockBuy;
                            dailyStockUpdate.StockLast = (dailyStockUpdate.StockFirst + dailyStockUpdate.StockBuy) - dailyStockUpdate.StockSell;
                            dailyStockUpdate.UpdateBy = param.UpdateBy;
                            dailyStockUpdate.UpdateDate = DateTime.Now;
                            _context.DailyStock.Update(dailyStockUpdate);
                            await _context.SaveChangesAsync();

                        } else {

                            DailyStock dailyStock = new DailyStock();
                            dailyStock.CompanyId = param.CompanyId;
                            dailyStock.BranchId = param.BranchId;
                            dailyStock.ProductTypeId = data.ProductTypeId;
                            dailyStock.ProductId = data.ProductId;
                            dailyStock.StockFirst = data.Quantity;
                            dailyStock.StockBuy = data.Quantity;
                            dailyStock.StockBuyPrice = data.Quantity * data.Price;
                            dailyStock.StockSell = 0;
                            dailyStock.StockSellPrice = 0;
                            dailyStock.StockLast = data.Quantity;
                            dailyStock.CreateBy = data.CreateBy;
                            dailyStock.CreateDate = data.CreateDate;
                            _context.DailyStock.Add(dailyStock);
                            await _context.SaveChangesAsync();
                        }

                        var monthlyStockUpdate1 = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month && x.ProductId == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                        if (monthlyStockUpdate1 != null) {
                            monthlyStockUpdate1.StockBuy = monthlyStockUpdate1.StockBuy + data.Quantity;
                            monthlyStockUpdate1.StockBuyPrice = (monthlyStockUpdate1.StockBuyPrice + (data.Quantity * data.Price)) / (monthlyStockUpdate1.StockBuy + data.Quantity);
                            monthlyStockUpdate1.StockLast = (monthlyStockUpdate1.StockFirst + monthlyStockUpdate1.StockBuy) - monthlyStockUpdate1.StockSell;
                            monthlyStockUpdate1.UpdateBy = param.UpdateBy;
                            monthlyStockUpdate1.UpdateDate = DateTime.Now;
                            _context.MonthlyStock.Update(monthlyStockUpdate1);
                            await _context.SaveChangesAsync();

                            var updateProduct = await _context.Product.Where(x => x.ProductTypeId == data.ProductTypeId && x.Id == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (updateProduct != null) { 
                                updateProduct.BuyPrice = monthlyStockUpdate1.StockBuyPrice;
                                _context.Product.Update(updateProduct);
                                await _context.SaveChangesAsync();
                            }

                        } else {

                            MonthlyStock monthlyStock = new MonthlyStock();
                            monthlyStock.CompanyId = param.CompanyId;
                            monthlyStock.BranchId = param.BranchId;
                            monthlyStock.ProductTypeId = data.ProductTypeId;
                            monthlyStock.ProductId = data.ProductId;
                            monthlyStock.StockFirst = data.Quantity;
                            monthlyStock.StockBuy = data.Quantity;
                            monthlyStock.StockBuyPrice = data.Quantity * data.Price;
                            monthlyStock.StockSell = 0;
                            monthlyStock.StockSellPrice = 0;
                            monthlyStock.StockLast = data.Quantity;
                            monthlyStock.Month = DateTime.Now.Month;
                            monthlyStock.Year = DateTime.Now.Year;
                            monthlyStock.CreateBy = data.CreateBy;
                            monthlyStock.CreateDate = data.CreateDate;
                            _context.MonthlyStock.Add(monthlyStock);
                            await _context.SaveChangesAsync();

                            var updateProduct = await _context.Product.Where(x => x.ProductTypeId == data.ProductTypeId && x.Id == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (updateProduct != null)
                            {
                                updateProduct.BuyPrice = monthlyStock.StockBuyPrice;
                                _context.Product.Update(updateProduct);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }

                    var purchaseOrder = await _context.PurchaseOrderHeader.Where(x => x.Id == puchaseOrderHeader.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (purchaseOrder != null) {
                        purchaseOrder.Total = param.Total;
                        purchaseOrder.Quantity = param.Quantity;
                        _context.PurchaseOrderHeader.Update(purchaseOrder);
                        await _context.SaveChangesAsync();
                    }

                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    purchaseOrderList = (from pur in _context.PurchaseOrderHeader
                                         join branch in _context.Branch on pur.BranchId equals branch.Id
                                         join company in _context.Company on pur.CompanyId equals company.Id
                                         join sup in _context.Supplier on pur.SupplierId equals sup.Id
                                         where pur.Id == param.Id
                                         orderby pur.PurchaseDate
                                         select new PurchaseOrderResponse
                                         {
                                             Id = pur.Id,
                                             CompanyId = pur.CompanyId,
                                             CompanyName = company.Name,
                                             BranchId = pur.BranchId,
                                             BranchName = branch.Name,
                                             PurchaseNo = pur.PurchaseNo,
                                             PurchaseDate = pur.PurchaseDate,
                                             SupplierId = pur.SupplierId,
                                             SupplierName = sup.Name,
                                             Total = pur.Total,
                                             Quantity = pur.Quantity,
                                             CreateBy = pur.CreateBy,
                                             CreateDate = pur.CreateDate,
                                             UpdateBy = pur.UpdateBy,
                                             UpdateDate = pur.UpdateDate
                                         }).Take(1).AsNoTracking();

                    return purchaseOrderList;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();

                    LogErrorAddRequest logDataError = new LogErrorAddRequest();
                    logDataError.ServiceName = "UpdatePuchaseOrder";
                    if (ex.InnerException != null) {
                        logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                    } else {
                        logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                    }
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                    return purchaseOrderList;
                }
            }

        }
    }
}
