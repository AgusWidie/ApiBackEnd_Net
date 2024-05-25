using APIRetail.Helper;
using APIRetail.Jobs.IJobs;
using APIRetail.Models.Database;
using APIRetail.Repository.IRepository;
using MySql.Data.MySqlClient;
using System.Data;

namespace APIRetail.Jobs
{
    public class SendMessage : ISendMessage
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        private bool bolProcess = false;
        private Thread threadSendWhatsApp;
        private Thread threadSendSMS;
        private Thread threadSendEmail;
        private SendDataMessageWhatsApp msgDataWhatsApp;
        private SendDataMessageSMS msgDataSMS;
        private SendDataMessageEmail msgDataEmail;
        public MySqlConnection mysqlConn;

        public SendMessage(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public void SendDataWhatsApp()
        {
            msgDataWhatsApp = new SendDataMessageWhatsApp(_configuration, _logError, _context);
            threadSendWhatsApp = new Thread(SendWhatsApp);
            threadSendWhatsApp.Priority = ThreadPriority.Normal;
            threadSendWhatsApp.Start();

        }

        public void SendWhatsApp()
        {
           
            try
            {
                if (bolProcess == false)
                {
                    bolProcess = true;

                    string sendDataMessage = "";
                    string noWhatsApp = "";

                    string sqlQuery = $"UPDATE log_job SET job_start = NOW(), job_finish = NULL, success = NULL, ACTIVE = 1, DESCRIPTION = 'Start Process Send WhatsApp' WHERE job_name = 'JobSendWhatsApp'";
                    MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                    command.ExecuteNonQuery();
                    command.Dispose();

                    sqlQuery = $"select * from schedule where active = 1 ";

                    MySqlCommand cmdSchedule = new MySqlCommand(sqlQuery, mysqlConn);
                    //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                    MySqlDataAdapter dataAdapterSchedule = new MySqlDataAdapter(cmdSchedule);
                    DataSet dataSetSchedule = new DataSet();
                    dataAdapterSchedule.Fill(dataSetSchedule);
                    DataTable dtSchedule = dataSetSchedule.Tables[0];

                    if (dtSchedule.Rows.Count > 0)
                    {
                        foreach (DataRow data in dtSchedule.Rows)
                        {
                            sqlQuery = $"select * from send_whatsapp where active = 1 AND send = 0 AND ScheduleId =  " + Convert.ToInt64(data["Id"]) + " ";
                            MySqlCommand cmdWA = new MySqlCommand(sqlQuery, mysqlConn);
                            //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                            MySqlDataAdapter dataAdapterWA = new MySqlDataAdapter(cmdWA);
                            DataSet dataSetWA = new DataSet();
                            dataAdapterWA.Fill(dataSetWA);
                            DataTable dtWA = dataSetWA.Tables[0];

                            if (dtWA.Rows.Count > 0)
                            {
                                foreach (DataRow dataWhatsApp in dtWA.Rows)
                                {
                                    if (Convert.ToDateTime(data["ScheduleDate"]) <= DateTime.Now)
                                    {
                                        sqlQuery = $"select * from message where Id = " + Convert.ToInt64(dataWhatsApp["MessageId"]) + " ";
                                        MySqlCommand cmdMsg = new MySqlCommand(sqlQuery, mysqlConn);
                                        //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                                        MySqlDataAdapter dataAdapterMsg = new MySqlDataAdapter(cmdMsg);
                                        DataSet dataSetMsg = new DataSet();
                                        dataAdapterMsg.Fill(dataSetMsg);
                                        DataTable dtMsg = dataSetMsg.Tables[0];

                                        if (dtMsg.Rows.Count > 0)
                                        {
                                            foreach (DataRow dataMessage in dtMsg.Rows)
                                            {
                                                sendDataMessage = dataMessage["MessageData"].ToString();
                                            }
                                        }

                                        sqlQuery = $"select * from customer where Id = " + Convert.ToInt64(data["CustomerId"]) + " ";
                                        MySqlCommand cmdCustomer = new MySqlCommand(sqlQuery, mysqlConn);
                                        //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                                        MySqlDataAdapter dataAdapterCustomer = new MySqlDataAdapter(cmdCustomer);
                                        DataSet dataSetCustomer = new DataSet();
                                        dataAdapterCustomer.Fill(dataSetMsg);
                                        DataTable dtCustomer = dataSetCustomer.Tables[0];

                                        if (dtCustomer.Rows.Count > 0)
                                        {
                                            foreach (DataRow dataCust in dtCustomer.Rows)
                                            {
                                                noWhatsApp = dataCust["WhatsApp"].ToString();
                                            }
                                        }

                                        msgDataWhatsApp.SendDataWhatsAppAsync(sendDataMessage, noWhatsApp);

                                        dataAdapterMsg.Dispose();
                                        dataSetMsg.Dispose();
                                        dtMsg.Dispose();

                                        dataAdapterCustomer.Dispose();
                                        dataSetCustomer.Dispose();
                                        dtCustomer.Dispose();
                                    }
                                }
                            }

                            dataAdapterWA.Dispose();
                            dataSetWA.Dispose();
                            dtWA.Dispose();
                        }
                    }

                    sqlQuery = "";
                    sqlQuery = $"UPDATE log_job SET job_finish = NOW(), success = 1, error = 0, ACTIVE = 1, DESCRIPTION = 'Successfully Job Send WhatsApp.' WHERE job_name = 'JobSendWhatsApp'";
                    MySqlCommand command1 = new MySqlCommand(sqlQuery, mysqlConn);
                    command1.ExecuteNonQuery();
                    command1.Dispose();

                    bolProcess = false;
                }
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
            catch (Exception ex)
            {
                string sqlQuery = $"UPDATE log_job SET job_finish = NOW(), error = 1, success = 0, ACTIVE = 1, DESCRIPTION = '" + ex.Message + "' WHERE job_name = 'JobSendWhatsApp'";
                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                command.ExecuteNonQuery();
                command.Dispose();

                bolProcess = false;
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
        }

        public void SendDataSMS()
        {
            msgDataSMS = new SendDataMessageSMS(_configuration, _logError, _context);
            threadSendSMS = new Thread(SendSMS);
            threadSendSMS.Priority = ThreadPriority.Normal;
            threadSendSMS.Start();

        }

        public void SendSMS()
        {
            try
            {
                if (bolProcess == false)
                {
                    bolProcess = true;
                    string sendDataMessage = "";
                    string noSMS = "";

                    string sqlQuery = $"UPDATE log_job SET job_start = NOW(), job_finish = NULL, success = NULL, ACTIVE = 1, DESCRIPTION = 'Start Process Send SMS' WHERE job_name = 'JobSendSMS'";
                    MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                    command.ExecuteNonQuery();
                    command.Dispose();

                    sqlQuery = $"select * from schedule where active = 1 ";

                    MySqlCommand cmdSchedule = new MySqlCommand(sqlQuery, mysqlConn);
                    //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                    MySqlDataAdapter dataAdapterSchedule = new MySqlDataAdapter(cmdSchedule);
                    DataSet dataSetSchedule = new DataSet();
                    dataAdapterSchedule.Fill(dataSetSchedule);
                    DataTable dtSchedule = dataSetSchedule.Tables[0];

                    if (dtSchedule.Rows.Count > 0)
                    {
                        foreach (DataRow data in dtSchedule.Rows)
                        {
                            sqlQuery = $"select * from send_sms where Active = 1 AND Send = 0 AND ScheduleId =  " + Convert.ToInt64(data["Id"]) + " ";
                            MySqlCommand cmdSMS = new MySqlCommand(sqlQuery, mysqlConn);
                            //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                            MySqlDataAdapter dataAdapterSMS = new MySqlDataAdapter(cmdSMS);
                            DataSet dataSetSMS = new DataSet();
                            dataAdapterSMS.Fill(dataSetSMS);
                            DataTable dtSMS = dataSetSMS.Tables[0];


                            if(dtSMS.Rows.Count > 0)
                            {
                                foreach (DataRow dataSMS in dtSMS.Rows)
                                {

                                    if (Convert.ToDateTime(data["ScheduleDate"]) <= DateTime.Now)

                                    {
                                        sqlQuery = $"select * from message where Id = " + Convert.ToInt64(dataSMS["MessageId"]) + " ";
                                        MySqlCommand cmdMsg = new MySqlCommand(sqlQuery, mysqlConn);
                                        //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                                        MySqlDataAdapter dataAdapterMsg = new MySqlDataAdapter(cmdMsg);
                                        DataSet dataSetMsg = new DataSet();
                                        dataAdapterMsg.Fill(dataSetMsg);
                                        DataTable dtMsg = dataSetMsg.Tables[0];

                                        if (dtMsg.Rows.Count > 0)
                                        {
                                            foreach (DataRow dataMessage in dtMsg.Rows)
                                            {
                                                sendDataMessage = dataMessage["MessageData"].ToString();
                                            }
                                        }

                                        sqlQuery = $"select * from customer where Id = " + Convert.ToInt64(data["CustomerId"]) + " ";
                                        MySqlCommand cmdCustomer = new MySqlCommand(sqlQuery, mysqlConn);
                                        //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                                        MySqlDataAdapter dataAdapterCustomer = new MySqlDataAdapter(cmdCustomer);
                                        DataSet dataSetCustomer = new DataSet();
                                        dataAdapterCustomer.Fill(dataSetMsg);
                                        DataTable dtCustomer = dataSetCustomer.Tables[0];

                                        if (dtCustomer.Rows.Count > 0)
                                        {
                                            foreach (DataRow dataCust in dtCustomer.Rows)
                                            {
                                                noSMS = dataCust["Telephone"].ToString();
                                            }
                                        }


                                        msgDataSMS.SendDataSMSAsync(sendDataMessage, noSMS);

                                        dataAdapterMsg.Dispose();
                                        dataSetMsg.Dispose();
                                        dtMsg.Dispose();

                                        dataAdapterCustomer.Dispose();
                                        dataSetCustomer.Dispose();
                                        dtCustomer.Dispose();
                                    }
                                }
                            }

                            dataAdapterSMS.Dispose();
                            dataSetSMS.Dispose();
                            dtSMS.Dispose();

                        }
                    }

                    sqlQuery = "";
                    sqlQuery = $"UPDATE log_job SET job_finish = NOW(), success = 1, error = 0, ACTIVE = 1, DESCRIPTION = 'Successfully Job Send SMS.' WHERE job_name = 'JobSendSMS'";
                    MySqlCommand command1 = new MySqlCommand(sqlQuery, mysqlConn);
                    command1.ExecuteNonQuery();
                    command1.Dispose();

                    bolProcess = false;

                    dataAdapterSchedule.Dispose();
                    dataSetSchedule.Dispose();
                    dtSchedule.Dispose();

                   

                }
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
            catch (Exception ex)
            {
                string sqlQuery = $"UPDATE log_job SET job_finish = NOW(), error = 1, success = 0, ACTIVE = 1, DESCRIPTION = '" + ex.Message + "' WHERE job_name = 'JobSendSMS'";
                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                command.ExecuteNonQuery();
                command.Dispose();

                bolProcess = false;
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }

        }

        public void SendDataEmail()
        {
            msgDataEmail = new SendDataMessageEmail(_configuration, _logError, _context);
            threadSendEmail = new Thread(SendEmail);
            threadSendEmail.Priority = ThreadPriority.Normal;
            threadSendEmail.Start();
        }

        public void SendEmail()
        {
            try
            {
                if (bolProcess == false)
                {
                    bolProcess = true;
                    string sendDataMessage = "";
                    string email = "";

                    string sqlQuery = $"UPDATE log_job SET job_start = NOW(), job_finish = NULL, success = NULL, ACTIVE = 1, DESCRIPTION = 'Start Process Send Email' WHERE job_name = 'JobSendEmail'";
                    MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                    command.ExecuteNonQuery();
                    command.Dispose();

                    sqlQuery = $"select * from schedule where active = 1 ";

                    MySqlCommand cmdSchedule = new MySqlCommand(sqlQuery, mysqlConn);
                    //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                    MySqlDataAdapter dataAdapterSchedule = new MySqlDataAdapter(cmdSchedule);
                    DataSet dataSetSchedule = new DataSet();
                    dataAdapterSchedule.Fill(dataSetSchedule);
                    DataTable dtSchedule = dataSetSchedule.Tables[0];

                    if (dtSchedule.Rows.Count > 0)
                    {
                        foreach (DataRow data in dtSchedule.Rows)
                        {
                            sqlQuery = $"select * from send_email where Active = 1 AND Send = 0 AND ScheduleId =  " + Convert.ToInt64(data["Id"]) + " ";
                            MySqlCommand cmdEmail = new MySqlCommand(sqlQuery, mysqlConn);
                            //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                            MySqlDataAdapter dataAdapterEmail = new MySqlDataAdapter(cmdEmail);
                            DataSet dataSetEmail = new DataSet();
                            dataAdapterEmail.Fill(dataSetEmail);
                            DataTable dtEmail = dataSetEmail.Tables[0];

                            if (dtEmail.Rows.Count > 0)
                            {
                                foreach (DataRow dataEmail in dtEmail.Rows)
                                {
                                    if (Convert.ToDateTime(data["ScheduleDate"]) <= DateTime.Now)
                                    {
                                        sqlQuery = $"select * from message where Id = " + Convert.ToInt64(dataEmail["MessageId"]) + " ";
                                        MySqlCommand cmdMsg = new MySqlCommand(sqlQuery, mysqlConn);
                                        //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                                        MySqlDataAdapter dataAdapterMsg = new MySqlDataAdapter(cmdMsg);
                                        DataSet dataSetMsg = new DataSet();
                                        dataAdapterMsg.Fill(dataSetMsg);
                                        DataTable dtMsg = dataSetMsg.Tables[0];

                                        if (dtMsg.Rows.Count > 0)
                                        {
                                            foreach (DataRow dataMessage in dtMsg.Rows)
                                            {
                                                sendDataMessage = dataMessage["MessageData"].ToString();
                                            }
                                        }

                                        sqlQuery = $"select * from customer where Id = " + Convert.ToInt64(data["CustomerId"]) + " ";
                                        MySqlCommand cmdCustomer = new MySqlCommand(sqlQuery, mysqlConn);
                                        //command.Parameters.Add(new MySqlParameter("@userName", userCtrl.UserName));

                                        MySqlDataAdapter dataAdapterCustomer = new MySqlDataAdapter(cmdCustomer);
                                        DataSet dataSetCustomer = new DataSet();
                                        dataAdapterCustomer.Fill(dataSetMsg);
                                        DataTable dtCustomer = dataSetCustomer.Tables[0];

                                        if (dtCustomer.Rows.Count > 0)
                                        {
                                            foreach (DataRow dataCust in dtCustomer.Rows)
                                            {
                                                email = dataCust["Email"].ToString();
                                            }
                                        }

                                      
                                        msgDataEmail.SendDataEmail(sendDataMessage, email);

                                        dataAdapterMsg.Dispose();
                                        dataSetMsg.Dispose();
                                        dtMsg.Dispose();

                                        dataAdapterCustomer.Dispose();
                                        dataSetCustomer.Dispose();
                                        dtCustomer.Dispose();
                                    }
                                }
                            }

                            dataAdapterEmail.Dispose();
                            dataSetEmail.Dispose();
                            dtEmail.Dispose();
                        }
                    }

                    sqlQuery = "";
                    sqlQuery = $"UPDATE log_job SET job_finish = NOW(), success = 1, error = 0, ACTIVE = 1, DESCRIPTION = 'Successfully Job Send Email.' WHERE job_name = 'JobSendEmail'";
                    MySqlCommand command1 = new MySqlCommand(sqlQuery, mysqlConn);
                    command1.ExecuteNonQuery();
                    command1.Dispose();

                    bolProcess = false;

                    dataAdapterSchedule.Dispose();
                    dataSetSchedule.Dispose();
                    dtSchedule.Dispose();
                }
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
            catch (Exception ex)
            {
               
                string sqlQuery = $"UPDATE log_job SET job_finish = NOW(), error = 1, success = 0, ACTIVE = 1, DESCRIPTION = '" + ex.Message + "' WHERE job_name = 'JobSendEmail'";
                MySqlCommand command = new MySqlCommand(sqlQuery, mysqlConn);
                command.ExecuteNonQuery();
                command.Dispose();

                bolProcess = false;
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }

        }
    }
}
