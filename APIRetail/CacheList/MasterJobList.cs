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
    public interface IMasterJobList
    {
        void ThreadJobMasterStart();
        void ThreadJobMasterStop();
    }

    public class MasterJobList : IMasterJobList
    {
        

        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public Thread threadMaster;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public MasterJobList(IConfiguration Configuration, retail_systemContext context, ILogError logError)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;

           
        }

        public void ThreadJobMasterStart()
        {
            try
            {
                threadMaster = new Thread(new ThreadStart(ExecuteThreadJobMaster));
                threadMaster.IsBackground = true;
                if (threadMaster.IsAlive == false)
                {
                    threadMaster.Start();
                }
            }
            catch(Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ThreadJobMasterStart";
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

        public void ThreadJobMasterStop()
        {
            try
            {
                if (threadMaster.IsAlive == true)
                {
#pragma warning disable SYSLIB0006 // Type or member is obsolete
                    threadMaster.Abort();
#pragma warning restore SYSLIB0006 // Type or member is obsolete
                }
            }
            catch(Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ThreadJobMasterStop";
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


        public void ExecuteThreadJobMaster()
        {
            try
            {
                while (threadMaster.IsAlive)
                {
                    MySqlTransaction trans = null;
                    try
                    {
                       
                        GarbageCollector.GarbageCollection();
                        MasterList.ConnectDB();

                        trans = MasterList.mysqlConn.BeginTransaction(IsolationLevel.ReadUncommitted);
                        MasterList.UpdateLogStartJob();

                        if (GeneralList._listBranch.Count() <= 0) {
                            GeneralList._listBranch.Clear();
                            GeneralList._listBranch.AddRange(MasterList.LoadDataBranch());
                        }

                        if (GeneralList._listCompany.Count() <= 0) {
                            GeneralList._listCompany.Clear();
                            GeneralList._listCompany.AddRange(MasterList.LoadDataCompany());
                        }

                        if (GeneralList._listCustomer.Count() <= 0) {
                            GeneralList._listCustomer.Clear();
                            GeneralList._listCustomer.AddRange(MasterList.LoadDataCustomer());
                        }

                        if (GeneralList._listMessage.Count() <= 0) {
                            GeneralList._listMessage.Clear();
                            GeneralList._listMessage.AddRange(MasterList.LoadDataMessage());
                        }

                        if (GeneralList._listProduct.Count() <= 0) {
                            GeneralList._listProduct.Clear();
                            GeneralList._listProduct.AddRange(MasterList.LoadDataProduct());
                        }

                        if (GeneralList._listProductType.Count() <= 0) {
                            GeneralList._listProductType.Clear();
                            GeneralList._listProductType.AddRange(MasterList.LoadDataProductType());
                        }

                        if (GeneralList._listProfilMenu.Count() <= 0) {
                            GeneralList._listProfilMenu.Clear();
                            GeneralList._listProfilMenu.AddRange(MasterList.LoadDataProfilMenu());
                        }

                        if (GeneralList._listProfil.Count() <= 0) {
                            GeneralList._listProfil.Clear();
                            GeneralList._listProfil.AddRange(MasterList.LoadDataProfil());
                        }

                        if (GeneralList._listProfilUser.Count() <= 0) {
                            GeneralList._listProfilUser.Clear();
                            GeneralList._listProfilUser.AddRange(MasterList.LoadDataProfilUser());
                        }

                        if (GeneralList._listSchedule.Count() <= 0) {
                            GeneralList._listSchedule.Clear();
                            GeneralList._listSchedule.AddRange(MasterList.LoadDataSchedule());
                        }

                        if (GeneralList._listSupplier.Count() <= 0) {
                            GeneralList._listSupplier.Clear();
                            GeneralList._listSupplier.AddRange(MasterList.LoadDataSupplier());
                        }

                        if (GeneralList._listUserMenuParent.Count() <= 0) {
                            GeneralList._listUserMenuParent.Clear();
                            GeneralList._listUserMenuParent.AddRange(MasterList.LoadDataUserMenuParent());
                        }

                        if (GeneralList._listUserMenu.Count() <= 0) {
                            GeneralList._listUserMenu.Clear();
                            GeneralList._listUserMenu.AddRange(MasterList.LoadDataUserMenu());
                        }

                        if (GeneralList._listUser.Count() <= 0) {
                            GeneralList._listUser.Clear();
                            GeneralList._listUser.AddRange(MasterList.LoadDataUser());
                        }

                        MasterList.UpdateLogFinishJob();

                        trans.Commit();
                        trans.Dispose();
                        MasterList.CloseConnectDB();
                        Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("MasterJobList:SleepJob")));

                    }
                    catch (Exception ex)
                    {

                        MasterList.UpdateLogErrorJob(ex.Message);
                        trans.Commit();
                        trans.Dispose();
                        MasterList.CloseConnectDB();

                        Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("MasterJobList:SleepJob")));
                    }

                }

                
            }
            catch (Exception ex)
            {
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("MasterJobList:SleepJob")));

                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ExecuteThreadJobMaster";
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
    
    public class MasterList
    {
        public static string ConnectMySQL = "";
        public static MySqlConnection mysqlConn;
        public static string ConnectDB()
        {
            try
            {
                DecryptMD5 decryptMD5 = new DecryptMD5();
                mysqlConn = new MySqlConnection(decryptMD5.MD5Decrypt(ConnectMySQL));
                if(mysqlConn.State == System.Data.ConnectionState.Closed) {
                    mysqlConn.Open();
                }
                return "Successfully ConnectDB.";
            }
            catch(Exception ex)
            {
                return "Error ConnectDB " + ex.Message;
            }
        }

        public static string CloseConnectDB()
        {
            try
            {
                DecryptMD5 decryptMD5 = new DecryptMD5();
                if(mysqlConn.State == System.Data.ConnectionState.Open) {
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
            catch(Exception ex)
            {
                return "Error UpdateLogStartJob : " + ex.Message;
            }
           
        }

        public static string UpdateLogFinishJob()
        {
            try
            {
                string sqlQuery = $"UPDATE log_job SET job_finish = NOW(), success = 1, ACTIVE = 1, DESCRIPTION = 'Finish Process Master Job List.' WHERE job_name = 'JobMaster'";
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

        public static List<BranchResponse> LoadDataBranch()
        {
            List<BranchResponse> listBranch = new List<BranchResponse>();
            try
            {
                string sqlQuery = $"select b.Id, c.Id as CompanyId, c.Name as CompanyName, b.Name, b.Address, b.Telp, b.Fax, b.CreateBy, " +
                                   "b.CreateDate, b.UpdateBy, b.UpdateDate from branch b join company c on b.CompanyId = c.Id ";

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
                        var dataBranch = new BranchResponse();
                        dataBranch.Id = Convert.ToInt32(row["Id"]);
                        dataBranch.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataBranch.CompanyName = (string)row["CompanyName"];
                        dataBranch.Name = (string)row["Name"];
                        dataBranch.Address = (string)row["Address"];
                        dataBranch.Telp = (string)row["Telp"];
                        dataBranch.Fax = (string)row["Fax"];
                        dataBranch.CreateBy = (string)row["CreateBy"];
                        dataBranch.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataBranch.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataBranch.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataBranch.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listBranch.Add(dataBranch);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listBranch;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public static List<CompanyResponse> LoadDataCompany()
        {
            List<CompanyResponse> listCompany = new List<CompanyResponse>();
            try
            {
                string sqlQuery = $"select c.Id, c.Name, c.Address, c.Telp, c.Fax, c.CreateBy, " +
                                   "c.CreateDate, c.UpdateBy, c.UpdateDate from company c";

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
                        var dataCompany = new CompanyResponse();
                        dataCompany.Id = Convert.ToInt32(row["Id"]);
                        dataCompany.Name = (string)row["Name"];
                        dataCompany.Address = (string)row["Address"];
                        dataCompany.Telp = (string)row["Telp"];
                        dataCompany.Fax = (string)row["Fax"];
                        dataCompany.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataCompany.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataCompany.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataCompany.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listCompany.Add(dataCompany);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listCompany;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<CustomerResponse> LoadDataCustomer()
        {
            List<CustomerResponse> listCust = new List<CustomerResponse>();

            try
            {

                string sqlQuery = $"select cust.Id, c.Id AS CompanyId, c.Name as CompanyName, b.Id as BranchId, b.Name as BranchName, " +
                                   "cust.Name, cust.Address, cust.WhatsApp, cust.Email, cust.CreateBy, cust.CreateDate, cust.UpdateBy, cust.UpdateDate" +
                                   "from customer cust join company c ON cust.CompanyId = c.Id " +
                                   "join branch b ON cust.BranchId = b.Id";

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
                        var dataCust = new CustomerResponse();
                        dataCust.Id = Convert.ToInt32(row["Id"]);
                        dataCust.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataCust.CompanyName = (string)row["CompanyName"];
                        dataCust.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataCust.BranchName = (string)row["BranchName"];
                        dataCust.Name = (string)row["Name"];
                        dataCust.Address = (string)row["Address"];
                        dataCust.WhatsApp = (string)row["WhatsApp"];
                        dataCust.Email = (string)row["Email"];
                        dataCust.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataCust.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataCust.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataCust.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listCust.Add(dataCust);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listCust;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }

            
        }

        public static List<MessageResponse> LoadDataMessage()
        {
            List<MessageResponse> listMessage = new List<MessageResponse>();

            try
            {
                string sqlQuery = $"select mess.Id, c.Id as CompanyId, c.Name as CompanyName, mess.MessageData, mess.Active, " +
                                   "mess.CreateBy, mess.CreateDate, mess.UpdateBy, mess.UpdateDate " +
                                   "from message mess join company c ON mess.CompanyId = c.Id ";

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
                        var dataMessage = new MessageResponse();
                        dataMessage.Id = Convert.ToInt32(row["Id"]);
                        dataMessage.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataMessage.CompanyName = (string)row["CompanyName"];
                        dataMessage.MessageData = (string)row["MessageData"];
                        dataMessage.Active = (ulong)row["Active"];
                        dataMessage.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataMessage.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataMessage.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataMessage.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listMessage.Add(dataMessage);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listMessage;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<ProductResponse> LoadDataProduct()
        {
            List<ProductResponse> listProd = new List<ProductResponse>();
            try
            {
                string sqlQuery = $"select p.Id, c.Id AS CompanyId, c.Name as CompanyName, b.Id as BranchId, b.Name AS BranchName, " +
                                   "pt.Id as ProductTypeId, pt.ProductTypeName, p.ProductNo, p.ProductName, p.BuyPrice, p.SellPrice, p.Description, p.Active, " +
                                   "p.Createby, p.CreateDate, p.UpdateBy, p.UpdateDate " +
                                   "from product p join company c ON p.CompanyId = c.Id " +
                                   "join branch b ON p.BranchId = b.Id " +
                                   "join product_type pt ON p.ProductTypeId = pt.Id ";

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
                        var dataProd = new ProductResponse();
                        dataProd.Id = Convert.ToInt32(row["Id"]);
                        dataProd.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataProd.CompanyName = (string)row["CompanyName"];
                        dataProd.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataProd.BranchName = (string)row["BranchName"];
                        dataProd.ProductTypeId = Convert.ToInt32(row["ProductTypeId"]);
                        dataProd.ProductTypeName = (string)row["ProductTypeName"];
                        dataProd.ProductNo = (string)row["ProductNo"];
                        dataProd.ProductName = (string)row["ProductName"];
                        dataProd.BuyPrice = Convert.ToInt32(row["BuyPrice"]);
                        dataProd.SellPrice = Convert.ToInt32(row["SellPrice"]);
                        dataProd.Description = (string)row["Description"];
                        dataProd.Active = (ulong)row["Active"];
                        dataProd.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataProd.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataProd.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataProd.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listProd.Add(dataProd);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listProd;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<ProductTypeResponse> LoadDataProductType()
        {
            List<ProductTypeResponse> listProdType = new List<ProductTypeResponse>();

            try
            {
                string sqlQuery = $"select p.Id, c.Id as CompanyId, c.Name AS CompanyName, b.Id as BranchId, b.Name AS BranchName, " +
                                   "p.Id as ProductTypeId, p.ProductTypeName, p.Description, p.Active, " +
                                   "p.Createby, p.CreateDate, p.UpdateBy, p.UpdateDate " +
                                   "from product_type p JOIN company c ON p.CompanyId = c.Id " +
                                   "JOIN branch b ON p.BranchId = b.Id ";

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
                        var dataProdType = new ProductTypeResponse();
                        dataProdType.Id = Convert.ToInt32(row["Id"]);
                        dataProdType.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataProdType.CompanyName = (string)row["CompanyName"];
                        dataProdType.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataProdType.BranchName = (string)row["BranchName"];
                        dataProdType.ProductTypeName = (string)row["ProductTypeName"];
                        dataProdType.Description = (string)row["Description"];
                        dataProdType.Active = (ulong)row["Active"];
                        dataProdType.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataProdType.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataProdType.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataProdType.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listProdType.Add(dataProdType);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listProdType;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<ProfilMenuResponse> LoadDataProfilMenu()
        {
            List<ProfilMenuResponse> listProfilMenu = new List<ProfilMenuResponse>();

            try
            {
                string sqlQuery = $"select pm.Id, pm.ProfilId, p.Name as ProfilName, mp.Id as ParentMenuId, mp.Name as ParentMenuName, " +
                                   "mc.Id as MenuId, mc.Name as MenuName, pm.CreateBy, pm.CreateDate, pm.UpdateBy, pm.UpdateDate " +
                                   "from profil p join profil_menu pm ON p.Id = pm.ProfilId " +
                                   "join menu mp ON pm.ParentMenuId = mp.Id " +
                                   "join menu mc ON pm.MenuId = mc.Id order by p.Name ";

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
                        var dataProfilMenu = new ProfilMenuResponse();
                        dataProfilMenu.Id = Convert.ToInt32(row["Id"]);
                        dataProfilMenu.ProfilId = Convert.ToInt32(row["ProfilId"]);
                        dataProfilMenu.ProfilName = (string)row["ProfilName"];
                        dataProfilMenu.ParentMenuId = Convert.ToInt32(row["ParentMenuId"]);
                        dataProfilMenu.ParentMenuName = (string)row["ParentMenuName"];
                        dataProfilMenu.MenuId = Convert.ToInt32(row["MenuId"]);
                        dataProfilMenu.MenuName = (string)row["MenuName"];
                        dataProfilMenu.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataProfilMenu.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataProfilMenu.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataProfilMenu.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }

                        listProfilMenu.Add(dataProfilMenu);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listProfilMenu;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }

            
        }

        public static List<ProfilResponse> LoadDataProfil()
        {
            List<ProfilResponse> listProfil = new List<ProfilResponse>();

            try
            {
                string sqlQuery = $"select p.Id, p.Name, p.Description, p.Active, p.CreateBy, p.CreateDate, p.UpdateBy, p.UpdateDate " +
                                   "from profil p";

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
                        var dataProfil = new ProfilResponse();
                        dataProfil.Id = Convert.ToInt32(row["Id"]);
                        dataProfil.Name = (string)row["Name"];
                        dataProfil.Description = (string)row["Description"];
                        dataProfil.Active = (ulong)row["Active"];
                        dataProfil.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataProfil.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataProfil.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataProfil.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }
                        listProfil.Add(dataProfil);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listProfil;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<ProfilUserResponse> LoadDataProfilUser()
        {
            List<ProfilUserResponse> listProfilUser = new List<ProfilUserResponse>();

            try
            {
                string sqlQuery = $"select pu.Id, pu.ProfilId, p.Name as ProfilName, pu.UserId, u.UserName, " +
                                   "pu.CreateBy, pu.CreateDate, pu.UpdateBy, pu.UpdateDate " +
                                   "from profil_user pu join users u ON pu.UserId = u.Id " +
                                   "join profil p ON pu.ProfilId = p.Id ";

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
                        var dataProfilUser = new ProfilUserResponse();
                        dataProfilUser.Id = Convert.ToInt32(row["Id"]);
                        dataProfilUser.ProfilId = Convert.ToInt32(row["ProfilId"]);
                        dataProfilUser.ProfilName = (string)row["ProfilName"];
                        dataProfilUser.UserId = Convert.ToInt32(row["UserId"]);
                        dataProfilUser.UserName = (string)row["UserName"];
                        dataProfilUser.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataProfilUser.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataProfilUser.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataProfilUser.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }
                        listProfilUser.Add(dataProfilUser);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listProfilUser;
            }
            catch (Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<ScheduleResponse> LoadDataSchedule()
        {
            List<ScheduleResponse> listSchedule = new List<ScheduleResponse>();

            try
            {
                string sqlQuery = $"select s.Id, s.CompanyId, c.Name AS CompanyName, s.ScheduleDate, s.Active, " +
                                   "s.CreateBy, s.CreateDate, s.UpdateBy, s.UpdateDate " +
                                   "from schedule s join company c ON s.CompanyId = c.Id ";

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
                        var dataSchedule = new ScheduleResponse();
                        dataSchedule.Id = Convert.ToInt32(row["Id"]);
                        dataSchedule.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataSchedule.CompanyName = (string)row["CompanyName"];
                        dataSchedule.ScheduleDate = Convert.ToDateTime(row["ScheduleDate"]);
                        dataSchedule.Active = (ulong)row["Active"];
                        dataSchedule.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataSchedule.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataSchedule.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataSchedule.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }
                        listSchedule.Add(dataSchedule);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listSchedule;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<SupplierResponse> LoadDataSupplier()
        {
            List<SupplierResponse> listSupplier = new List<SupplierResponse>();

            try
            {
                string sqlQuery = $"select s.Id, s.CompanyId, c.Name as CompanyName, s.BranchId, b.Name as BranchName, " +
                                   "s.Name, s.Address, s.Telp, s.Fax, s.Active, " +
                                   "s.CreateBy, s.CreateDate, s.UpdateBy, s.UpdateDate " +
                                   "from supplier s join company c ON s.CompanyId = c.Id " +
                                   "join branch b ON s.BranchId = b.Id ";

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
                        var dataSupplier = new SupplierResponse();
                        dataSupplier.Id = Convert.ToInt32(row["Id"]);
                        dataSupplier.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataSupplier.CompanyName = (string)row["CompanyName"];
                        dataSupplier.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataSupplier.BranchName = (string)row["BranchName"];
                        dataSupplier.Name = (string)row["Name"];
                        dataSupplier.Address = (string)row["Address"];
                        dataSupplier.Telp = (string)row["Telp"];
                        dataSupplier.Fax = (string)row["Fax"];
                        dataSupplier.Active = (ulong)row["Active"];
                        dataSupplier.CreateBy = (string)row["CreateBy"];
                        if (row["CreateDate"] != null) {
                            dataSupplier.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataSupplier.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataSupplier.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }
                        listSupplier.Add(dataSupplier);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listSupplier;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<UserMenuParentResponse> LoadDataUserMenuParent()
        {
            List<UserMenuParentResponse> listUserMenuParent = new List<UserMenuParentResponse>();


            try
            {
                string sqlQuery = $"select distinct u.CompanyId, c.Name as CompanyName, u.BranchId, b.Name as BranchName, pu.ProfilId, p.Name as ProfilName, " +
                                   "u.Id AS UserId, u.Name as UserName, mp.Id as ParentMenuId, mp.Name AS ParentMenuName " +
                                   "from profil_user pu join users u ON pu.UserId = u.Id " +
                                   "join profil p ON pu.ProfilId = p.Id " +
                                   "join company c ON u.CompanyId = c.Id " +
                                   "join branch b ON u.BranchId = b.Id " +
                                   "join profil_menu pm ON pu.ProfilId = pm.ProfilId " +
                                   "join menu mp ON pm.ParentMenuId = mp.Id " +
                                   "where mp.IsHeader = 1 "; ;

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
                        var dataUserMenuParent = new UserMenuParentResponse();
                        dataUserMenuParent.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataUserMenuParent.CompanyName = (string)row["CompanyName"];
                        dataUserMenuParent.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataUserMenuParent.BranchName = (string)row["BranchName"];
                        dataUserMenuParent.ProfilId = Convert.ToInt32(row["ProfilId"]);
                        dataUserMenuParent.ProfilName = (string)row["ProfilName"];
                        dataUserMenuParent.ParentMenuId = Convert.ToInt32(row["ParentMenuId"]);
                        dataUserMenuParent.ParentMenuName = (string)row["ParentMenuName"];
                        dataUserMenuParent.UserId = Convert.ToInt32(row["UserId"]);
                        dataUserMenuParent.UserName = (string)row["UserName"];
                        dataUserMenuParent.Sort = Convert.ToInt32(row["Sort"]);
                        listUserMenuParent.Add(dataUserMenuParent);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listUserMenuParent;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<UserMenuResponse> LoadDataUserMenu()
        {
            List<UserMenuResponse> listUserMenu = new List<UserMenuResponse>();

            try
            {
                string sqlQuery = $"select distinct u.CompanyId, c.Name as CompanyName, u.BranchId, b.Name as BranchName, pu.ProfilId, p.Name as ProfilName, " +
                                   "u.Id AS UserId, u.Name as UserName, mp.Id as ParentMenuId, mp.Name AS ParentMenuName, mc.Id as MenuId, mc.Name as MenuName " +
                                   "from profil_user pu join users u ON pu.UserId = u.Id " +
                                   "join profil p ON pu.ProfilId = p.Id " +
                                   "join company c ON u.CompanyId = c.Id " +
                                   "join branch b ON u.BranchId = b.Id " +
                                   "join profil_menu pm ON pu.ProfilId = pm.ProfilId " +
                                   "join menu mp ON pm.ParentMenuId = mp.Id " +
                                   "join menu mc ON pm.MenuId = mc.Id " +
                                   "where mp.IsHeader = 1 and mc.IsHeader != 1 "; 

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
                        var dataUserMenu = new UserMenuResponse();
                        dataUserMenu.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataUserMenu.CompanyName = (string)row["CompanyName"];
                        dataUserMenu.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataUserMenu.BranchName = (string)row["BranchName"];
                        dataUserMenu.ProfilId = Convert.ToInt32(row["ProfilId"]);
                        dataUserMenu.ProfilName = (string)row["ProfilName"];
                        dataUserMenu.ParentMenuId = Convert.ToInt32(row["ParentMenuId"]);
                        dataUserMenu.ParentMenuName = (string)row["ParentMenuName"];
                        dataUserMenu.MenuId = Convert.ToInt32(row["MenuId"]);
                        dataUserMenu.MenuName = (string)row["MenuName"];
                        dataUserMenu.UserId = Convert.ToInt32(row["UserId"]);
                        dataUserMenu.UserName = (string)row["UserName"];
                        dataUserMenu.Sort = Convert.ToInt32(row["Sort"]);
                        listUserMenu.Add(dataUserMenu);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listUserMenu;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }

        public static List<UsersResponse> LoadDataUser()
        {
            List<UsersResponse> listUser = new List<UsersResponse>();

            try
            {
                string sqlQuery = $"select u.Id, u.CompanyId, c.Name as CompanyName, u.BranchId, b.Name as BranchName, " +
                                   "u.UserName, u.Name, u.Email, u.PasswordExpired, u.Description, u.Active, " +
                                   "u.CreateBy, u.CreateDate, u.UpdateBy, u.UpdateDate " +
                                   "from users u " +
                                   "join company c ON u.CompanyId = c.Id " +
                                   "join branch b ON u.BranchId = b.Id ";

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
                        var dataUser = new UsersResponse();
                        dataUser.Id = Convert.ToInt32(row["Id"]);
                        dataUser.CompanyId = Convert.ToInt32(row["CompanyId"]);
                        dataUser.CompanyName = (string)row["CompanyName"];
                        dataUser.BranchId = Convert.ToInt32(row["BranchId"]);
                        dataUser.BranchName = (string)row["BranchName"];
                        dataUser.UserName = (string)row["UserName"];
                        dataUser.Name = (string)row["Name"];
                        dataUser.Email = (string)row["Email"];
                        dataUser.PasswordExpired = Convert.ToDateTime(row["PasswordExpired"]);
                        dataUser.Description = (string)row["Description"];
                        dataUser.Active = (ulong)row["Active"];
                        dataUser.LastLogin = Convert.ToDateTime(row["LastLogin"]);
                        dataUser.CreateBy = (string)row["CreateBy"];
                        if(row["CreateDate"] != null) {
                            dataUser.CreateDate = Convert.ToDateTime(row["CreateDate"].ToString());
                        }
                        dataUser.UpdateBy = (string)row["UpdateBy"];
                        if (row["UpdateDate"] != null) {
                            dataUser.UpdateDate = Convert.ToDateTime(row["UpdateDate"].ToString());
                        }
                      
                        listUser.Add(dataUser);
                    }

                }

                command.Dispose();
                dataAdapter.Dispose();
                dataSet.Dispose();
                dtResults.Dispose();

                return listUser;
            }
            catch(Exception ex)
            {
                UpdateLogErrorJob(ex.Message);
                throw;
            }
            
        }
    }
}
