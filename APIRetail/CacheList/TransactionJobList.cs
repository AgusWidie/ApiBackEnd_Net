using APIRetail.Crypto;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using MySql.Data.MySqlClient;
using System.Data;

namespace APIRetail.CacheList
{
    public interface ITransactionJobList
    {
        void ThreadJobTransactionStart();
        void ThreadJobTransactionStop();
    }

    public class TransactionJobList : ITransactionJobList
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public Thread threadTransaction;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public TransactionJobList(IConfiguration Configuration, retail_systemContext context, ILogError logError)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public void ThreadJobTransactionStart()
        {
            try
            {
                threadTransaction = new Thread(new ThreadStart(ExecuteThreadJobTransaction));
                threadTransaction.IsBackground = true;
                if (threadTransaction.IsAlive == false) {
                    threadTransaction.Start();
                }
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ThreadJobTransactionStart";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                CancellationToken cancellationToken = new CancellationToken();
                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
            }



        }

        public void ThreadJobTransactionStop()
        {
            try
            {
                if (threadTransaction.IsAlive == true) {
                    threadTransaction.Abort();
                }
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ThreadJobTransactionStop";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                CancellationToken cancellationToken = new CancellationToken();
                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
            }


        }

        public void ExecuteThreadJobTransaction()
        {
            try
            {
                while (threadTransaction.IsAlive)
                {
                    MySqlTransaction trans = null;
                    try
                    {
                        GarbageCollector.GarbageCollection();
                        TransactionList.ConnectDB();

                        trans = TransactionList.mysqlConn.BeginTransaction(IsolationLevel.ReadUncommitted);
                        TransactionList.UpdateLogStartJob();

                        if (GeneralList._listMonthlyStock.Count() <= 0) {
                            GeneralList._listMonthlyStock.Clear();
                            GeneralList._listMonthlyStock.AddRange(TransactionList.LoadDataMonthlyStock());
                        }

                        if (GeneralList._listDailyStock.Count() <= 0) {
                            GeneralList._listDailyStock.Clear();
                            GeneralList._listDailyStock.AddRange(TransactionList.LoadDataDailyStock());
                        }

                        if (GeneralList._listRankingProduct.Count() <= 0) {
                            GeneralList._listRankingProduct.Clear();
                            GeneralList._listRankingProduct.AddRange(TransactionList.LoadDataRangkingProduct());
                        }

                        if (GeneralList._listStockOpname.Count() <= 0) {
                            GeneralList._listStockOpname.Clear();
                            GeneralList._listStockOpname.AddRange(TransactionList.LoadDataStockOpname());
                        }

                        if (GeneralList._listPurchaseOrder.Count() <= 0) {
                            GeneralList._listPurchaseOrder.Clear();
                            GeneralList._listPurchaseOrder.AddRange(TransactionList.LoadDataPurchaseOrder());
                        }

                        if (GeneralList._listPurchaseOrderDetail.Count() <= 0) {
                            GeneralList._listPurchaseOrderDetail.Clear();
                            GeneralList._listPurchaseOrderDetail.AddRange(TransactionList.LoadDataPurchaseOrderDetail());
                        }

                        if (GeneralList._listSalesOrder.Count() <= 0) {
                            GeneralList._listSalesOrder.Clear();
                            GeneralList._listSalesOrder.AddRange(TransactionList.LoadDataSalesOrder());
                        }

                        if (GeneralList._listSalesOrderDetail.Count() <= 0) {
                            GeneralList._listSalesOrderDetail.Clear();
                            GeneralList._listSalesOrderDetail.AddRange(TransactionList.LoadDataSalesOrderDetail());
                        }

                    }
                    catch (Exception ex)
                    {
                        TransactionList.UpdateLogFinishJob();

                        trans.Commit();
                        trans.Dispose();
                        TransactionList.CloseConnectDB();

                        Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("TransactionJobList:SleepJob")));
                    }
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("TransactionJobList:SleepJob")));

                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ExecuteThreadJobTransaction";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                CancellationToken cancellationToken = new CancellationToken();
                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
            }
        }

        
    }

    public class TransactionList
    {
        public static string ConnectMySQL = "";
        public static MySqlConnection mysqlConn;

        public static string ConnectDB()
        {
            try
            {
                DecryptMD5 decryptMD5 = new DecryptMD5();
                mysqlConn = new MySqlConnection(ConnectMySQL);
                if (mysqlConn.State == System.Data.ConnectionState.Closed)
                {
                    mysqlConn.Open();
                }
                return "Successfully ConnectDB.";
            }
            catch (Exception ex)
            {
                return "Error ConnectDB " + ex.Message;
            }
        }

        public static string CloseConnectDB()
        {
            try
            {
                DecryptMD5 decryptMD5 = new DecryptMD5();
                if (mysqlConn.State == System.Data.ConnectionState.Open)
                {
                    mysqlConn.Close();
                }
                return "Successfully CloseConnectDB.";
            }
            catch (Exception ex)
            {
                return "Error CloseConnectDB " + ex.Message;
            }
        }

        public static string UpdateLogStartJob()
        {
            try
            {
                string sqlQuery = $"UPDATE log_job SET job_start = NOW(), job_finish = NULL, success = NULL, ACTIVE = 1, DESCRIPTION = 'Start Process Master Job List.' WHERE job_name = 'JobMaster'";
                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                command.ExecuteNonQuery();

                return "Successfully UpdateLogStartJob";
            }
            catch (Exception ex)
            {
                return "Error UpdateLogStartJob : " + ex.Message;
            }

        }

        public static string UpdateLogFinishJob()
        {
            try
            {
                string sqlQuery = $"UPDATE log_job SET job_finish = NOW(), success = 1, ACTIVE = 1, DESCRIPTION = 'Finish Process Master Job List.' WHERE job_name = 'JobTransaction'";
                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                command.ExecuteNonQuery();

                return "Successfully UpdateLogFinishJob";
            }
            catch (Exception ex)
            {
                return "Error UpdateLogFinishJob : " + ex.Message;
            }

        }

        public static string UpdateLogErrorJob(string Error)
        {
            try
            {
                string sqlQuery = $"UPDATE log_job SET job_finish = NOW(), success = 0, ACTIVE = 1, DESCRIPTION = '" + Error + "' WHERE job_name = 'JobMaster'";
                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                command.ExecuteNonQuery();

                return "Successfully UpdateLogErrorJob";
            }
            catch (Exception ex)
            {
                return "Error UpdateLogErrorJob : " + ex.Message;
            }

        }

        public static List<MonthlyStockResponse> LoadDataMonthlyStock()
        {
            List<MonthlyStockResponse> listMonthlyStock = new List<MonthlyStockResponse>();
            try
            {
                string sqlQuery = $"select ms.Id, ms.CompanyId, c.Name as CompanyName, ms.BranchId, b.Name AS BranchName, " +
                                   "b.CreateDate, b.UpdateBy, b.UpdateDate from branch b join company c on b.CompanyId = c.Id " +
                                   "ms.ProductTypeId, pt.ProductTypeName, ms.ProductId, p.ProductName, " +
                                   "ms.StockFirst, ms.StockBuy, ms.StockBuyPrice, ms.StockSell, ms.StockSellPrice, ms.StockLast, " +
                                   "ms.Month, ms.Year, ms.CreateBy, ms.CreateDate, ms.UpdateBy, ms.UpdateDate " +
                                   "from monthly_stock ms " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join product_type pt ON ms.ProductTypeId = pt.Id " +
                                   "join product p ON ms.ProductId = p.Id " +
                                   "where ms.Year = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataMonthlyStock = new MonthlyStockResponse();
                        dataMonthlyStock.Id = Convert.ToInt32(row["Id"]);
                        dataMonthlyStock.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataMonthlyStock.CompanyName = (string)row["CompanyName"];
                        dataMonthlyStock.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataMonthlyStock.BranchName = (string)row["BranchName"];
                        dataMonthlyStock.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataMonthlyStock.ProductTypeName = (string)row["ProductTypeName"];
                        dataMonthlyStock.ProductId = Convert.ToInt32(row["ProductId"]);
                        dataMonthlyStock.ProductName = (string)row["ProductName"];
                        dataMonthlyStock.StockFirst = Convert.ToInt64(row["StockFirst"]);
                        dataMonthlyStock.StockBuy = Convert.ToInt64(row["StockBuy"]);
                        dataMonthlyStock.StockBuyPrice = Convert.ToInt64(row["StockBuyPrice"]);
                        dataMonthlyStock.StockSell = Convert.ToInt64(row["StockSell"]);
                        dataMonthlyStock.StockSellPrice = Convert.ToInt64(row["StockSellPrice"]);
                        dataMonthlyStock.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataMonthlyStock.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataMonthlyStock.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataMonthlyStock.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listMonthlyStock.Add(dataMonthlyStock);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listMonthlyStock;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<DailyStockResponse> LoadDataDailyStock()
        {
            List<DailyStockResponse> listDailyStock = new List<DailyStockResponse>();
            try
            {
                string sqlQuery = $"select ms.Id, ms.CompanyId, c.Name as CompanyName, ms.BranchId, b.Name AS BranchName, " +
                                   "b.CreateDate, b.UpdateBy, b.UpdateDate from branch b join company c on b.CompanyId = c.Id " +
                                   "ms.ProductTypeId, pt.ProductTypeName, ms.ProductId, p.ProductName, " +
                                   "ms.StockFirst, ms.StockBuy, ms.StockBuyPrice, ms.StockSell, ms.StockSellPrice, ms.StockLast, " +
                                   "ms.StockDate, ms.CreateBy, ms.CreateDate, ms.UpdateBy, ms.UpdateDate " +
                                   "from daily_stock ms " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join product_type pt ON ms.ProductTypeId = pt.Id " +
                                   "join product p ON ms.ProductId = p.Id " +
                                   "where YEAR(ms.StockDate) = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataDailyStock = new DailyStockResponse();
                        dataDailyStock.Id = Convert.ToInt32(row["Id"]);
                        dataDailyStock.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataDailyStock.CompanyName = (string)row["CompanyName"];
                        dataDailyStock.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataDailyStock.BranchName = (string)row["BranchName"];
                        dataDailyStock.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataDailyStock.ProductTypeName = (string)row["ProductTypeName"];
                        dataDailyStock.ProductId = Convert.ToInt32(row["ProductId"]);
                        dataDailyStock.ProductName = (string)row["ProductName"];
                        dataDailyStock.StockFirst = Convert.ToInt64(row["StockFirst"]);
                        dataDailyStock.StockBuy = Convert.ToInt64(row["StockBuy"]);
                        dataDailyStock.StockBuyPrice = Convert.ToInt64(row["StockBuyPrice"]);
                        dataDailyStock.StockSell = Convert.ToInt64(row["StockSell"]);
                        dataDailyStock.StockSellPrice = Convert.ToInt64(row["StockSellPrice"]);
                        dataDailyStock.StockDate = (DateOnly)row["StockDate"];
                        dataDailyStock.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataDailyStock.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataDailyStock.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataDailyStock.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listDailyStock.Add(dataDailyStock);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listDailyStock;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<RankingProductResponse> LoadDataRangkingProduct()
        {
            List<RankingProductResponse> listRankingProduct = new List<RankingProductResponse>();
            try
            {
                string sqlQuery = $"select ms.Id, ms.CompanyId, c.Name as CompanyName, ms.BranchId, b.Name AS BranchName, " +
                                   "b.CreateDate, b.UpdateBy, b.UpdateDate from branch b join company c on b.CompanyId = c.Id " +
                                   "ms.ProductTypeId, pt.ProductTypeName, ms.ProductId, p.ProductName, " +
                                   "ms.StockFirst, ms.StockBuy, ms.StockBuyPrice, ms.StockSell, ms.StockSellPrice, ms.StockLast, " +
                                   "ms.Month, ms.Year, ms.CreateBy, ms.CreateDate, ms.UpdateBy, ms.UpdateDate " +
                                   "from monthly_stock ms " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join product_type pt ON ms.ProductTypeId = pt.Id " +
                                   "join product p ON ms.ProductId = p.Id " +
                                   "where ms.Year = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataRankingProduct = new RankingProductResponse();
                        dataRankingProduct.Id = Convert.ToInt32(row["Id"]);
                        dataRankingProduct.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataRankingProduct.CompanyName = (string)row["CompanyName"];
                        dataRankingProduct.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataRankingProduct.BranchName = (string)row["BranchName"];
                        dataRankingProduct.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataRankingProduct.ProductTypeName = (string)row["ProductTypeName"];
                        dataRankingProduct.ProductId = Convert.ToInt32(row["ProductId"]);
                        dataRankingProduct.ProductName = (string)row["ProductName"];
                        dataRankingProduct.StockBuy = Convert.ToInt64(row["StockBuy"]);
                        dataRankingProduct.StockSell = Convert.ToInt64(row["StockSell"]);
                        dataRankingProduct.Month = (int)row["Month"];
                        dataRankingProduct.Year = (int)row["Year"];
  
                        listRankingProduct.Add(dataRankingProduct);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listRankingProduct;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<StockOpnameResponse> LoadDataStockOpname()
        {
            List<StockOpnameResponse> listStockOpname = new List<StockOpnameResponse>();
            try
            {
                string sqlQuery = $"select ms.Id, ms.CompanyId, c.Name as CompanyName, ms.BranchId, b.Name as BranchName, " +
                                   "b.CreateDate, b.UpdateBy, b.UpdateDate from branch b join company c on b.CompanyId = c.Id " +
                                   "ms.ProductTypeId, pt.ProductTypeName, ms.ProductId, p.ProductName, " +
                                   "ms.StockFirst, ms.StockOpnameDefault, ms.StockOpnameDate,  " +
                                   "ms.CreateBy, ms.CreateDate, ms.UpdateBy, ms.UpdateDate  " +
                                   "from stock_opname ms " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join product_type pt ON ms.ProductTypeId = pt.Id " +
                                   "join product p ON ms.ProductId = p.Id " +
                                   "where YEAR(ms.StockOpnameDate) = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataStockOpname = new StockOpnameResponse();
                        dataStockOpname.Id = Convert.ToInt32(row["Id"]);
                        dataStockOpname.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataStockOpname.CompanyName = (string)row["CompanyName"];
                        dataStockOpname.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataStockOpname.BranchName = (string)row["BranchName"];
                        dataStockOpname.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataStockOpname.ProductTypeName = (string)row["ProductTypeName"];
                        dataStockOpname.ProductId = Convert.ToInt32(row["ProductId"]);
                        dataStockOpname.ProductName = (string)row["ProductName"];
                        dataStockOpname.Description = (string)row["Description"];
                        dataStockOpname.StockFirst = (long)row["StockBuy"];
                        dataStockOpname.StockOpnameDefault = (long)row["StockOpnameDefault"];
                        dataStockOpname.StockOpnameDate = (DateOnly)row["StockOpnameDate"];
                        dataStockOpname.Month = (int)row["Month"];
                        dataStockOpname.Year = (int)row["Year"];

                        listStockOpname.Add(dataStockOpname);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listStockOpname;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<SalesOrderResponse> LoadDataSalesOrder()
        {
            List<SalesOrderResponse> listSalesOrder = new List<SalesOrderResponse>();
            try
            {
                string sqlQuery = $"select ms.Id, ms.CompanyId, c.Name as CompanyName, ms.BranchId, b.Name as BranchName, " +
                                   "ms.InvoiceNo, ms.SalesOrderDate, ms.SalesId, '' as SalesName, ms.CustomerId, cu.Name as CustomerName, " +
                                   "ms.Description, ms.Total, ms.Quantity, " +
                                   "ms.CreateBy, ms.CreateDate, ms.UpdateBy, ms.UpdateDate " +
                                   "from sales_order_header ms " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join customer cu ON ms.CustomerId = cu.Id " +
                                   "where YEAR(ms.SalesOrderDate) = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataSalesOrder = new SalesOrderResponse();
                        dataSalesOrder.Id = Convert.ToInt32(row["Id"]);
                        dataSalesOrder.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataSalesOrder.CompanyName = (string)row["CompanyName"];
                        dataSalesOrder.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataSalesOrder.BranchName = (string)row["BranchName"];
                        dataSalesOrder.InvoiceNo = (string)row["InvoiceNo"];
                        dataSalesOrder.SalesOrderDate = Convert.ToDateTime(row["SalesOrderDate"].ToString());
                        dataSalesOrder.SalesId = (long)row["SalesId"];
                        dataSalesOrder.SalesName = (string)row["SalesName"];
                        dataSalesOrder.CustomerId = (long)row["CustomerId"];
                        dataSalesOrder.CustomerName = (string)row["CustomerName"];
                        dataSalesOrder.Description = (string)row["Description"];
                        dataSalesOrder.Total = (long)row["Total"];
                        dataSalesOrder.Quantity = (int)row["Quantity"];
                        dataSalesOrder.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataSalesOrder.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataSalesOrder.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataSalesOrder.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listSalesOrder.Add(dataSalesOrder);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listSalesOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<SalesOrderDetailResponse> LoadDataSalesOrderDetail()
        {
            List<SalesOrderDetailResponse> listSalesOrderDetail = new List<SalesOrderDetailResponse>();
            try
            {
                string sqlQuery = $"select sod.SalesHeaderId, sod.ProductTypeId, pt.ProductTypeName, sod.ProductId, p.ProductName, " +
                                   "sod.Quantity, sod.Price, sod.Subtotal, " +
                                   "sod.CreateBy, sod.CreateDate, sod.UpdateBy, sod.UpdateDate  " +
                                   "from sales_order_header ms  " +
                                   "join sales_order_detail sod ON ms.Id = sod.SalesHeaderId " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join product_type pt ON sod.ProductTypeId = pt.Id " +
                                   "JOIN product p ON sod.ProductId = p.Id " +
                                   "where YEAR(ms.SalesOrderDate) = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataSalesOrderDetail = new SalesOrderDetailResponse();
                        dataSalesOrderDetail.SalesHeaderId = Convert.ToInt32(row["SalesHeaderId"]);
                        dataSalesOrderDetail.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataSalesOrderDetail.ProductTypeName = (string)row["ProductTypeName"];
                        dataSalesOrderDetail.ProductId = Convert.ToInt32(row["ProductId"]);
                        dataSalesOrderDetail.ProductName = (string)row["ProductName"];
                        dataSalesOrderDetail.Quantity = (int)row["Quantity"];
                        dataSalesOrderDetail.Price = (int)row["Price"];
                        dataSalesOrderDetail.Subtotal = (long)row["Subtotal"];
                        dataSalesOrderDetail.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataSalesOrderDetail.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataSalesOrderDetail.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataSalesOrderDetail.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listSalesOrderDetail.Add(dataSalesOrderDetail);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listSalesOrderDetail;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<PurchaseOrderResponse> LoadDataPurchaseOrder()
        {
            List<PurchaseOrderResponse> listPurchaseOrder = new List<PurchaseOrderResponse>();
            try
            {
                string sqlQuery = $"select ms.Id, ms.CompanyId, c.Name as CompanyName, ms.BranchId, b.Name as BranchName, " +
                                   "ms.PurchaseNo, ms.PurchaseDate, ms.SupplierId, s.Name as SupplierName, ms.Quantity, ms.Total, " +
                                   "ms.CreateBy, ms.CreateDate, ms.UpdateBy, ms.UpdateDate " +
                                   "from purchase_order_header ms " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join supplier s ON ms.SupplierId = s.Id " +
                                   "where YEAR(ms.PurchaseDate) = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataPurchaseOrder = new PurchaseOrderResponse();
                        dataPurchaseOrder.Id = Convert.ToInt32(row["Id"]);
                        dataPurchaseOrder.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataPurchaseOrder.CompanyName = (string)row["CompanyName"];
                        dataPurchaseOrder.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataPurchaseOrder.BranchName = (string)row["BranchName"];
                        dataPurchaseOrder.PurchaseNo = (string)row["PurchaseNo"];
                        dataPurchaseOrder.PurchaseDate = Convert.ToDateTime(row["PurchaseDate"].ToString());
                        dataPurchaseOrder.SupplierId = Convert.ToInt32(row["SupplierId"]);
                        dataPurchaseOrder.SupplierName = (string)row["SupplierName"];
                        dataPurchaseOrder.Quantity = (int)row["Quantity"];
                        dataPurchaseOrder.Total = (long)row["Total"];
                        dataPurchaseOrder.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataPurchaseOrder.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataPurchaseOrder.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataPurchaseOrder.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listPurchaseOrder.Add(dataPurchaseOrder);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listPurchaseOrder;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static List<PurchaseOrderDetailResponse> LoadDataPurchaseOrderDetail()
        {
            List<PurchaseOrderDetailResponse> listPurchaseOrderDetail = new List<PurchaseOrderDetailResponse>();
            try
            {
                string sqlQuery = $"select pod.PurchaseHeaderId, pod.ProductTypeId, pt.ProductTypeName, pod.ProductId, p.ProductName, " +
                                   "pod.Quantity, pod.Price, pod.Subtotal, " +
                                   "pod.CreateBy, pod.CreateDate " +
                                   "from purchase_order_header ms   " +
                                   "join purchase_order_detail pod ON ms.Id = pod.PurchaseHeaderId " +
                                   "join company c ON ms.CompanyId = c.Id " +
                                   "join branch b ON ms.BranchId = b.Id " +
                                   "join product_type pt ON pod.ProductTypeId = pt.Id " +
                                   "join product p ON pod.ProductId = p.Id " +
                                   "where YEAR(ms.PurchaseDate) = YEAR(CURDATE()) ";

                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));
                MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);

                DataTable dtResults = dataSet.Tables[0];
                if (dtResults.Rows.Count > 0)
                {
                    foreach (DataRow row in dtResults.Rows)
                    {
                        var dataPurchaseOrderDetail = new PurchaseOrderDetailResponse();
                        dataPurchaseOrderDetail.PurchaseHeaderId = Convert.ToInt32(row["SalesHeaderId"]);
                        dataPurchaseOrderDetail.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataPurchaseOrderDetail.ProductTypeName = (string)row["ProductTypeName"];
                        dataPurchaseOrderDetail.ProductId = Convert.ToInt32(row["ProductId"]);
                        dataPurchaseOrderDetail.ProductName = (string)row["ProductName"];
                        dataPurchaseOrderDetail.Quantity = (int)row["Quantity"];
                        dataPurchaseOrderDetail.Price = (int)row["Price"];
                        dataPurchaseOrderDetail.Subtotal = (long)row["Subtotal"];
                        dataPurchaseOrderDetail.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataPurchaseOrderDetail.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                       
                        listPurchaseOrderDetail.Add(dataPurchaseOrderDetail);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listPurchaseOrderDetail;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
