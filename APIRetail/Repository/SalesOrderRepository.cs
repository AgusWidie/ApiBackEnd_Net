using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class SalesOrderRepository : ISalesOrder
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public SalesOrderRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<SalesOrderResponse>> GetSalesOrderHeader(SalesOrderRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SalesOrderResponse>? salesOrderList = null;
            try
            {
                long Page = param.Page - 1;
                salesOrderList = (from sal in _context.SalesOrderHeader
                                  join branch in _context.Branch on sal.BranchId equals branch.Id
                                  join company in _context.Company on sal.CompanyId equals company.Id
                                  join customer in _context.Customer on sal.CustomerId equals customer.Id
                                  where sal.CompanyId == param.CompanyId && sal.BranchId == param.BranchId
                                  && sal.SalesOrderDate >= param.SalesOrderDateFrom && sal.SalesOrderDate <= param.SalesOrderDateTo
                                  select new SalesOrderResponse
                                  {
                                      Id = sal.Id,
                                      CompanyId = sal.CompanyId,
                                      CompanyName = company.Name,
                                      BranchId = sal.BranchId,
                                      BranchName = branch.Name,
                                      InvoiceNo = sal.InvoiceNo,
                                      SalesOrderDate = sal.SalesOrderDate,
                                      SalesId = sal.SalesId,
                                      SalesName = "",
                                      CustomerId = sal.CustomerId,
                                      CustomerName = customer.Name,
                                      Description = sal.Description,
                                      Total = sal.Total,
                                      Quantity = sal.Quantity,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderByDescending(x => x.InvoiceNo).AsNoTracking();

                var TotalPageSize = Math.Ceiling((decimal)salesOrderList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = salesOrderList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetSalesOrderHeader";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return salesOrderList;
            }
        }

        public async Task<IEnumerable<SalesOrderDetailResponse>> GetSalesOrderDetail(SalesOrderRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SalesOrderDetailResponse>? salesOrderDetailList = null;
            try
            {

                salesOrderDetailList = (from salDetail in _context.SalesOrderDetail
                                        join prod in _context.Product on salDetail.ProductId equals prod.Id
                                        join prodType in _context.ProductType on salDetail.ProductTypeId equals prodType.Id
                                        where salDetail.SalesHeaderId == param.SalesHeaderId
                                        select new SalesOrderDetailResponse
                                        {
                                            SalesHeaderId = salDetail.SalesHeaderId,
                                            ProductTypeId = salDetail.ProductTypeId,
                                            ProductTypeName = prodType.ProductTypeName,
                                            ProductId = salDetail.ProductId,
                                            ProductName = prod.ProductName,
                                            Quantity = salDetail.Quantity,
                                            Price = salDetail.Price,
                                            Subtotal = salDetail.Subtotal,
                                            CreateBy = salDetail.CreateBy,
                                            CreateDate = salDetail.CreateDate,
                                            UpdateBy = salDetail.UpdateBy,
                                            UpdateDate = salDetail.UpdateDate
                                        }).OrderBy(x => x.ProductName).AsNoTracking();

                return salesOrderDetailList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetSalesOrderDetail";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return salesOrderDetailList;
            }
        }

        public async Task<IEnumerable<SalesOrderResponse>> CreateSalesOrder(SalesOrderHeaderAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SalesOrderResponse>? salesOrderList = null;
            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    param.Total = 0;
                    param.Quantity = 0;
                    SalesOrderHeader salesOrderHeader = new SalesOrderHeader();
                    salesOrderHeader.CompanyId = param.CompanyId;
                    salesOrderHeader.BranchId = param.BranchId;
                    salesOrderHeader.InvoiceNo = param.InvoiceNo;
                    salesOrderHeader.SalesOrderDate = DateTime.Now;
                    salesOrderHeader.SalesId = param.SalesId;
                    salesOrderHeader.CustomerId = param.CustomerId;
                    salesOrderHeader.Description = param.Description;
                    salesOrderHeader.Total = param.Total;
                    salesOrderHeader.CreateBy = param.CreateBy;
                    salesOrderHeader.CreateDate = DateTime.Now;
                    _context.SalesOrderHeader.Add(salesOrderHeader);
                    await _context.SaveChangesAsync();

                    foreach (var data in param.SalesOrderDetailAddRequest)
                    {
                        SalesOrderDetail salesOrderDetail = new SalesOrderDetail();
                        salesOrderDetail.SalesHeaderId = salesOrderHeader.Id;
                        salesOrderDetail.ProductTypeId = data.ProductTypeId;
                        salesOrderDetail.ProductId = data.ProductId;
                        salesOrderDetail.Quantity = data.Quantity;
                        salesOrderDetail.Price = data.Price;
                        salesOrderDetail.Subtotal = data.Quantity * data.Price;
                        param.Total = param.Total + (data.Quantity * data.Price);
                        param.Quantity = param.Quantity + data.Quantity;
                        salesOrderDetail.CreateBy = data.CreateBy;
                        salesOrderDetail.CreateDate = DateTime.Now;
                        _context.SalesOrderDetail.Add(salesOrderDetail);
                        await _context.SaveChangesAsync();

                        var dailyStockUpdate = await _context.DailyStock.Where(x => x.ProductTypeId == data.ProductTypeId && x.ProductId == data.ProductId 
                                                   && x.StockDate.Year == DateTime.Now.Year && x.StockDate.Month == DateTime.Now.Month && x.StockDate.Day == DateTime.Now.Day).AsNoTracking().FirstOrDefaultAsync();
                        if (dailyStockUpdate != null)
                        {
                            dailyStockUpdate.StockSell = dailyStockUpdate.StockSell + data.Quantity;
                            dailyStockUpdate.StockSellPrice = (dailyStockUpdate.StockSellPrice + (data.Quantity * data.Price)) / dailyStockUpdate.StockSell;
                            dailyStockUpdate.StockLast = (dailyStockUpdate.StockFirst + dailyStockUpdate.StockBuy) - dailyStockUpdate.StockSell;
                            dailyStockUpdate.UpdateBy = param.CreateBy;
                            dailyStockUpdate.UpdateDate = DateTime.Now;
                            _context.DailyStock.Update(dailyStockUpdate);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {

                            DailyStock dailyStock = new DailyStock();
                            dailyStock.CompanyId = param.CompanyId;
                            dailyStock.BranchId = param.BranchId;
                            dailyStock.ProductTypeId = data.ProductTypeId;
                            dailyStock.ProductId = data.ProductId;
                            dailyStock.StockFirst = data.Quantity;
                            dailyStock.StockBuy = 0;
                            dailyStock.StockBuyPrice = 0;
                            dailyStock.StockSell = data.Quantity;
                            dailyStock.StockSellPrice = data.Quantity * data.Price;
                            dailyStock.StockLast = data.Quantity;
                            dailyStock.CreateBy = param.CreateBy;
                            dailyStock.CreateDate = param.CreateDate;
                            _context.DailyStock.Add(dailyStock);
                            await _context.SaveChangesAsync();
                        }

                        var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId 
                                                 && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month && x.ProductId == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                        if (monthlyStockUpdate != null)
                        {
                            if (monthlyStockUpdate.StockLast < data.Quantity)
                            {
                                salesOrderList = (from sal in _context.SalesOrderHeader
                                                  join branch in _context.Branch on sal.BranchId equals branch.Id
                                                  join company in _context.Company on sal.CompanyId equals company.Id
                                                  join customer in _context.Customer on sal.CustomerId equals customer.Id
                                                  where sal.CompanyId == param.CompanyId && sal.BranchId == param.BranchId
                                                  && sal.InvoiceNo == param.InvoiceNo
                                                  orderby sal.SalesOrderDate
                                                  select new SalesOrderResponse
                                                  {
                                                      Id = sal.Id,
                                                      CompanyId = sal.CompanyId,
                                                      CompanyName = company.Name,
                                                      BranchId = sal.BranchId,
                                                      BranchName = branch.Name,
                                                      InvoiceNo = sal.InvoiceNo,
                                                      SalesOrderDate = sal.SalesOrderDate,
                                                      SalesId = sal.SalesId,
                                                      SalesName = "",
                                                      CustomerId = sal.CustomerId,
                                                      CustomerName = customer.Name,
                                                      Description = sal.Description,
                                                      Total = sal.Total,
                                                      Quantity = sal.Quantity,
                                                      CreateBy = sal.CreateBy,
                                                      CreateDate = sal.CreateDate,
                                                      UpdateBy = sal.UpdateBy,
                                                      UpdateDate = sal.UpdateDate
                                                  }).Take(0).AsNoTracking();

                                dbContextTransaction.Rollback();
                                dbContextTransaction.Dispose();
                                return salesOrderList;
                            }

                            monthlyStockUpdate.StockSell = monthlyStockUpdate.StockSell + data.Quantity;
                            monthlyStockUpdate.StockSellPrice = (monthlyStockUpdate.StockSellPrice + (data.Quantity * data.Price)) / monthlyStockUpdate.StockSell;
                            monthlyStockUpdate.StockLast = (monthlyStockUpdate.StockFirst + monthlyStockUpdate.StockBuy) - monthlyStockUpdate.StockSell;
                            monthlyStockUpdate.UpdateBy = param.CreateBy;
                            monthlyStockUpdate.UpdateDate = DateTime.Now;
                            _context.MonthlyStock.Update(monthlyStockUpdate);
                            await _context.SaveChangesAsync();

                            var updateProduct = await _context.Product.Where(x => x.ProductTypeId == data.ProductTypeId && x.Id == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (updateProduct != null)
                            {
                                updateProduct.SellPrice = monthlyStockUpdate.StockSellPrice;
                                _context.Product.Update(updateProduct);
                                await _context.SaveChangesAsync();
                            }

                        }
                        else
                        {

                            salesOrderList = (from sal in _context.SalesOrderHeader
                                              join branch in _context.Branch on sal.BranchId equals branch.Id
                                              join company in _context.Company on sal.CompanyId equals company.Id
                                              join customer in _context.Customer on sal.CustomerId equals customer.Id
                                              where sal.CompanyId == param.CompanyId && sal.BranchId == param.BranchId
                                              && sal.InvoiceNo == param.InvoiceNo
                                              orderby sal.SalesOrderDate
                                              select new SalesOrderResponse
                                              {
                                                  Id = sal.Id,
                                                  CompanyId = sal.CompanyId,
                                                  CompanyName = company.Name,
                                                  BranchId = sal.BranchId,
                                                  BranchName = branch.Name,
                                                  InvoiceNo = sal.InvoiceNo,
                                                  SalesOrderDate = sal.SalesOrderDate,
                                                  SalesId = sal.SalesId,
                                                  SalesName = "",
                                                  CustomerId = sal.CustomerId,
                                                  CustomerName = customer.Name,
                                                  Description = sal.Description,
                                                  Total = sal.Total,
                                                  Quantity = sal.Quantity,
                                                  CreateBy = sal.CreateBy,
                                                  CreateDate = sal.CreateDate,
                                                  UpdateBy = sal.UpdateBy,
                                                  UpdateDate = sal.UpdateDate
                                              }).Take(0).AsNoTracking();

                            dbContextTransaction.Rollback();
                            dbContextTransaction.Dispose();
                            return salesOrderList;
                        }

                    }

                    var salesOrder = await _context.SalesOrderHeader.Where(x => x.Id == salesOrderHeader.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (salesOrder != null)
                    {
                        salesOrder.Total = param.Total;
                        salesOrder.Quantity = param.Quantity;
                        _context.SalesOrderHeader.Update(salesOrder);
                        await _context.SaveChangesAsync();
                    }

                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();

                    salesOrderList = (from sal in _context.SalesOrderHeader
                                      join branch in _context.Branch on sal.BranchId equals branch.Id
                                      join company in _context.Company on sal.CompanyId equals company.Id
                                      join customer in _context.Customer on sal.CustomerId equals customer.Id
                                      where sal.CompanyId == param.CompanyId && sal.BranchId == param.BranchId
                                      && sal.InvoiceNo == param.InvoiceNo
                                      orderby sal.SalesOrderDate
                                      select new SalesOrderResponse
                                      {
                                          Id = sal.Id,
                                          CompanyId = sal.CompanyId,
                                          CompanyName = company.Name,
                                          BranchId = sal.BranchId,
                                          BranchName = branch.Name,
                                          InvoiceNo = sal.InvoiceNo,
                                          SalesOrderDate = sal.SalesOrderDate,
                                          SalesId = sal.SalesId,
                                          SalesName = "",
                                          CustomerId = sal.CustomerId,
                                          CustomerName = customer.Name,
                                          Description = sal.Description,
                                          Total = sal.Total,
                                          Quantity = sal.Quantity,
                                          CreateBy = sal.CreateBy,
                                          CreateDate = sal.CreateDate,
                                          UpdateBy = sal.UpdateBy,
                                          UpdateDate = sal.UpdateDate
                                      }).Take(1).AsNoTracking();

                    return salesOrderList;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();

                    LogErrorAddRequest logDataError = new LogErrorAddRequest();
                    logDataError.ServiceName = "CreateSalesOrder";
                    if (ex.InnerException != null) {
                        logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                    } else {
                        logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                    }
                    logDataError.ErrorDate = DateTime.Now;
                    logDataError.CreateDate = DateTime.Now;
                    logDataError.CreateBy = "System";

                    var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                    return salesOrderList;
                }
            }
        }

        public async Task<IEnumerable<SalesOrderResponse>> UpdateSalesOrder(SalesOrderHeaderUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SalesOrderResponse>? salesOrderList = null;

            using (var dbContextTransaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted))
            {
                try
                {
                    param.Total = 0;
                    param.Quantity = 0;

                    var salesOrderHeader = await _context.SalesOrderHeader.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (salesOrderHeader != null)
                    {
                        salesOrderHeader.CompanyId = param.CompanyId;
                        salesOrderHeader.BranchId = param.BranchId;
                        salesOrderHeader.InvoiceNo = param.InvoiceNo;
                        salesOrderHeader.SalesOrderDate = param.SalesOrderDate;
                        salesOrderHeader.SalesId = param.SalesId;
                        salesOrderHeader.CustomerId = param.CustomerId;
                        salesOrderHeader.Description = param.Description;
                        salesOrderHeader.Total = param.Total;
                        salesOrderHeader.CreateBy = param.UpdateBy;
                        salesOrderHeader.CreateDate = DateTime.Now;
                        _context.SalesOrderHeader.Update(salesOrderHeader);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        salesOrderList = (from sal in _context.SalesOrderHeader
                                          join branch in _context.Branch on sal.BranchId equals branch.Id
                                          join company in _context.Company on sal.CompanyId equals company.Id
                                          join customer in _context.Customer on sal.CustomerId equals customer.Id
                                          where sal.Id == param.Id
                                          orderby sal.SalesOrderDate
                                          select new SalesOrderResponse
                                          {
                                              Id = sal.Id,
                                              CompanyId = sal.CompanyId,
                                              CompanyName = company.Name,
                                              BranchId = sal.BranchId,
                                              BranchName = branch.Name,
                                              InvoiceNo = sal.InvoiceNo,
                                              SalesOrderDate = sal.SalesOrderDate,
                                              SalesId = sal.SalesId,
                                              SalesName = "",
                                              CustomerId = sal.CustomerId,
                                              CustomerName = customer.Name,
                                              Description = sal.Description,
                                              Total = sal.Total,
                                              Quantity = sal.Quantity,
                                              CreateBy = branch.CreateBy,
                                              CreateDate = branch.CreateDate,
                                              UpdateBy = branch.UpdateBy,
                                              UpdateDate = branch.UpdateDate
                                          }).Take(0).AsNoTracking();

                        return salesOrderList;
                    }

                    foreach (var data in param.SalesOrderDetailUpdateRequest)
                    {
                        var salesOrderDetailRemove = await _context.SalesOrderDetail.Where(x => x.ProductId == data.ProductId && x.SalesHeaderId == param.Id).FirstOrDefaultAsync();
                        if (salesOrderDetailRemove != null)
                        {
                            var monthlyStockUpdate = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId 
                                                     && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month && x.ProductId == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                            if (monthlyStockUpdate != null)
                            {
                                monthlyStockUpdate.StockSell = monthlyStockUpdate.StockSell - salesOrderDetailRemove.Quantity;
                                monthlyStockUpdate.StockSellPrice = (monthlyStockUpdate.StockSellPrice - (salesOrderDetailRemove.Quantity * salesOrderDetailRemove.Price)) / monthlyStockUpdate.StockSell;
                                monthlyStockUpdate.StockLast = monthlyStockUpdate.StockFirst + monthlyStockUpdate.StockBuy - monthlyStockUpdate.StockSell;
                                monthlyStockUpdate.UpdateBy = param.UpdateBy;
                                monthlyStockUpdate.UpdateDate = DateTime.Now;
                                _context.MonthlyStock.Update(monthlyStockUpdate);
                                await _context.SaveChangesAsync();
                            }
                            _context.SalesOrderDetail.Remove(salesOrderDetailRemove);
                            await _context.SaveChangesAsync();
                        }

                        SalesOrderDetail salesOrderDetail = new SalesOrderDetail();
                        salesOrderDetail.SalesHeaderId = salesOrderHeader.Id;
                        salesOrderDetail.ProductTypeId = data.ProductTypeId;
                        salesOrderDetail.ProductId = data.ProductId;
                        salesOrderDetail.Quantity = data.Quantity;
                        salesOrderDetail.Price = data.Price;
                        salesOrderDetail.Subtotal = data.Quantity * data.Price;
                        param.Total = param.Total + (data.Quantity * data.Price);
                        param.Quantity = param.Quantity + data.Quantity;
                        salesOrderDetail.CreateBy = data.CreateBy;
                        salesOrderDetail.CreateDate = DateTime.Now;
                        _context.SalesOrderDetail.Add(salesOrderDetail);
                        await _context.SaveChangesAsync();

                        var dailyStockUpdate = await _context.DailyStock.Where(x => x.ProductTypeId == data.ProductTypeId && x.ProductId == data.ProductId 
                                               && x.StockDate.Year == DateTime.Now.Year && x.StockDate.Month == DateTime.Now.Month && x.StockDate.Day == DateTime.Now.Day).AsNoTracking().FirstOrDefaultAsync();
                        if (dailyStockUpdate != null)
                        {
                            dailyStockUpdate.StockSell = dailyStockUpdate.StockSell + data.Quantity;
                            dailyStockUpdate.StockSellPrice = (dailyStockUpdate.StockSellPrice + (data.Quantity * data.Price)) / dailyStockUpdate.StockSell;
                            dailyStockUpdate.StockLast = dailyStockUpdate.StockFirst + dailyStockUpdate.StockBuy - (dailyStockUpdate.StockSell + data.Quantity);
                            dailyStockUpdate.UpdateBy = param.UpdateBy;
                            dailyStockUpdate.UpdateDate = DateTime.Now;
                            _context.DailyStock.Update(dailyStockUpdate);
                            await _context.SaveChangesAsync();

                        }
                        else
                        {

                            DailyStock dailyStock = new DailyStock();
                            dailyStock.CompanyId = param.CompanyId;
                            dailyStock.BranchId = param.BranchId;
                            dailyStock.ProductTypeId = data.ProductTypeId;
                            dailyStock.ProductId = data.ProductId;
                            dailyStock.StockFirst = data.Quantity;
                            dailyStock.StockBuy = 0;
                            dailyStock.StockBuyPrice = 0;
                            dailyStock.StockSell = data.Quantity;
                            dailyStock.StockSellPrice = data.Quantity * data.Price;
                            dailyStock.StockLast = data.Quantity;
                            dailyStock.CreateBy = data.CreateBy;
                            dailyStock.CreateDate = data.CreateDate;
                            _context.DailyStock.Add(dailyStock);
                            await _context.SaveChangesAsync();
                        }

                        var monthlyStockUpdate1 = await _context.MonthlyStock.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId 
                                                  && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month && x.ProductId == data.ProductId).AsNoTracking().FirstOrDefaultAsync();
                        if (monthlyStockUpdate1 != null)
                        {

                            if (monthlyStockUpdate1.StockLast < data.Quantity)
                            {
                                salesOrderList = (from sal in _context.SalesOrderHeader
                                                  join branch in _context.Branch on sal.BranchId equals branch.Id
                                                  join company in _context.Company on sal.CompanyId equals company.Id
                                                  join customer in _context.Customer on sal.CustomerId equals customer.Id
                                                  where sal.CompanyId == param.CompanyId && sal.BranchId == param.BranchId
                                                  && sal.Id == salesOrderHeader.Id
                                                  orderby sal.SalesOrderDate
                                                  select new SalesOrderResponse
                                                  {
                                                      Id = sal.Id,
                                                      CompanyId = sal.CompanyId,
                                                      CompanyName = company.Name,
                                                      BranchId = sal.BranchId,
                                                      BranchName = branch.Name,
                                                      InvoiceNo = sal.InvoiceNo,
                                                      SalesOrderDate = sal.SalesOrderDate,
                                                      SalesId = sal.SalesId,
                                                      SalesName = "",
                                                      CustomerId = sal.CustomerId,
                                                      CustomerName = customer.Name,
                                                      Description = sal.Description,
                                                      Total = sal.Total,
                                                      Quantity = sal.Quantity,
                                                      CreateBy = sal.CreateBy,
                                                      CreateDate = sal.CreateDate,
                                                      UpdateBy = sal.UpdateBy,
                                                      UpdateDate = sal.UpdateDate
                                                  }).Take(0).AsNoTracking();

                                dbContextTransaction.Rollback();
                                dbContextTransaction.Dispose();
                                return salesOrderList;
                            }

                            monthlyStockUpdate1.StockSell = monthlyStockUpdate1.StockSell + data.Quantity;
                            monthlyStockUpdate1.StockSellPrice = (monthlyStockUpdate1.StockSellPrice + (data.Quantity * data.Price)) / monthlyStockUpdate1.StockSell;
                            monthlyStockUpdate1.StockLast = monthlyStockUpdate1.StockFirst + monthlyStockUpdate1.StockBuy - monthlyStockUpdate1.StockSell;
                            monthlyStockUpdate1.UpdateBy = param.UpdateBy;
                            monthlyStockUpdate1.UpdateDate = DateTime.Now;
                            _context.MonthlyStock.Update(monthlyStockUpdate1);
                            await _context.SaveChangesAsync();

                            var updateProduct = await _context.Product.Where(x => x.ProductTypeId == data.ProductTypeId && x.Id == data.ProductId).FirstOrDefaultAsync();
                            if (updateProduct != null)
                            {
                                updateProduct.SellPrice = monthlyStockUpdate1.StockSellPrice;
                                _context.Product.Update(updateProduct);
                                await _context.SaveChangesAsync();
                            }

                        }
                        else
                        {

                            salesOrderList = (from sal in _context.SalesOrderHeader
                                              join branch in _context.Branch on sal.BranchId equals branch.Id
                                              join company in _context.Company on sal.CompanyId equals company.Id
                                              join customer in _context.Customer on sal.CustomerId equals customer.Id
                                              where sal.CompanyId == param.CompanyId && sal.BranchId == param.BranchId
                                              && sal.Id == salesOrderHeader.Id
                                              orderby sal.SalesOrderDate
                                              select new SalesOrderResponse
                                              {
                                                  Id = sal.Id,
                                                  CompanyId = sal.CompanyId,
                                                  CompanyName = company.Name,
                                                  BranchId = sal.BranchId,
                                                  BranchName = branch.Name,
                                                  InvoiceNo = sal.InvoiceNo,
                                                  SalesOrderDate = sal.SalesOrderDate,
                                                  SalesId = sal.SalesId,
                                                  SalesName = "",
                                                  CustomerId = sal.CustomerId,
                                                  CustomerName = customer.Name,
                                                  Description = sal.Description,
                                                  Total = sal.Total,
                                                  Quantity = sal.Quantity,
                                                  CreateBy = sal.CreateBy,
                                                  CreateDate = sal.CreateDate,
                                                  UpdateBy = sal.UpdateBy,
                                                  UpdateDate = sal.UpdateDate
                                              }).Take(0).AsNoTracking();

                            dbContextTransaction.Rollback();
                            dbContextTransaction.Dispose();
                            return salesOrderList;

                        }

                        var salesOrder = await _context.SalesOrderHeader.Where(x => x.Id == salesOrderHeader.Id).FirstOrDefaultAsync();
                        if (salesOrder != null)
                        {
                            salesOrder.Total = param.Total;
                            salesOrder.Quantity = param.Quantity;
                            _context.SalesOrderHeader.Update(salesOrder);
                            await _context.SaveChangesAsync();
                        }
                    }

                    dbContextTransaction.Commit();
                    dbContextTransaction.Dispose();
                    salesOrderList = (from sal in _context.SalesOrderHeader
                                      join branch in _context.Branch on sal.BranchId equals branch.Id
                                      join company in _context.Company on sal.CompanyId equals company.Id
                                      join customer in _context.Customer on sal.CustomerId equals customer.Id
                                      where sal.Id == param.Id
                                      orderby sal.SalesOrderDate
                                      select new SalesOrderResponse
                                      {
                                          Id = sal.Id,
                                          CompanyId = sal.CompanyId,
                                          CompanyName = company.Name,
                                          BranchId = sal.BranchId,
                                          BranchName = branch.Name,
                                          InvoiceNo = sal.InvoiceNo,
                                          SalesOrderDate = sal.SalesOrderDate,
                                          SalesId = sal.SalesId,
                                          SalesName = "",
                                          CustomerId = sal.CustomerId,
                                          CustomerName = customer.Name,
                                          Description = sal.Description,
                                          Total = sal.Total,
                                          Quantity = sal.Quantity,
                                          CreateBy = sal.CreateBy,
                                          CreateDate = sal.CreateDate,
                                          UpdateBy = sal.UpdateBy,
                                          UpdateDate = sal.UpdateDate
                                      }).Take(1).AsNoTracking();

                    return salesOrderList;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    dbContextTransaction.Dispose();

                    LogErrorAddRequest logDataError = new LogErrorAddRequest();
                    logDataError.ServiceName = "UpdateSalesOrder";
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
                    return salesOrderList;
                }
            }
        }
    }
}
