using APIRetail.Helper;
using APIRetail.Jobs.IJobs;
using APIRetail.Models.Database;
using APIRetail.Repository.IRepository;
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

        public SendMessage(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async void SendDataWhatsApp()
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
                    var updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendWhatsApp" && x.Active == 1).FirstOrDefault();
                    if (updateLogJob != null)
                    {
                        updateLogJob.JobStart = DateTime.Now;
                        updateLogJob.JobFinish = null;
                        updateLogJob.Success = 0;
                        updateLogJob.Error = 0;
                        updateLogJob.Description = "";
                        _context.LogJob.Update(updateLogJob);
                        _context.SaveChangesAsync();
                    }

                    var checkSchedule = _context.Schedule.Where(x => x.Active == 1).ToList().OrderBy(x => x.Id);
                    if (checkSchedule.Count() > 0)
                    {
                        foreach (var data in checkSchedule)
                        {
                            var dataSendWhatsApp = _context.SendWhatsapp.Where(x => x.Active == 1 && x.Send == 0 && x.ScheduleId == data.Id).ToList();
                            if (dataSendWhatsApp.Count() > 0)
                            {
                                foreach (var dataWhatsApp in dataSendWhatsApp)
                                {
                                    if (data.ScheduleDate <= DateTime.Now)
                                    {
                                        var sendDataMessage = _context.Message.Where(x => x.Id == dataWhatsApp.MessageId).Select(x => x.MessageData).FirstOrDefault();
                                        var noWhatsApp = _context.Customer.Where(x => x.Id == dataWhatsApp.CustomerId).Select(x => x.WhatsApp).FirstOrDefault();
                                        msgDataWhatsApp.SendDataWhatsAppAsync(sendDataMessage, noWhatsApp);
                                    }
                                }
                            }
                        }
                    }

                    updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendWhatsApp" && x.Active == 1).FirstOrDefault();
                    if (updateLogJob != null)
                    {
                        updateLogJob.JobFinish = DateTime.Now;
                        updateLogJob.Success = 1;
                        updateLogJob.Error = 0;
                        updateLogJob.Description = "Successfully Job Send WhatsApp.";
                        _context.LogJob.Update(updateLogJob);
                        _context.SaveChangesAsync();
                    }
                    bolProcess = false;
                }
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
            catch (Exception ex)
            {
                var updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendWhatsApp" && x.Active == 1).FirstOrDefault();
                if (updateLogJob != null)
                {
                    updateLogJob.JobFinish = DateTime.Now;
                    updateLogJob.Success = 0;
                    updateLogJob.Error = 1;
                    updateLogJob.Description = ex.Message;
                    _context.LogJob.Update(updateLogJob);
                    _context.SaveChangesAsync();
                }
                bolProcess = false;
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
        }

        public async void SendDataSMS()
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
                    var updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendSMS" && x.Active == 1).FirstOrDefault();
                    if (updateLogJob != null)
                    {
                        updateLogJob.JobStart = DateTime.Now;
                        updateLogJob.JobFinish = null;
                        updateLogJob.Success = 0;
                        updateLogJob.Error = 0;
                        updateLogJob.Description = "";
                        _context.LogJob.Update(updateLogJob);
                        _context.SaveChangesAsync();
                    }

                    var checkSchedule = _context.Schedule.Where(x => x.Active == 1).ToList().OrderBy(x => x.Id);
                    if (checkSchedule.Count() > 0)
                    {
                        foreach (var data in checkSchedule)
                        {
                            var dataSendSMS = _context.SendSms.Where(x => x.Active == 1 && x.Send == 0 && x.ScheduleId == data.Id).ToList();
                            if (dataSendSMS.Count() > 0)
                            {
                                foreach (var dataSMS in dataSendSMS)
                                {
                                    if (data.ScheduleDate <= DateTime.Now)
                                    {
                                        var sendDataMessage = _context.Message.Where(x => x.Id == dataSMS.MessageId).Select(x => x.MessageData).FirstOrDefault();
                                        var noSMS = _context.Customer.Where(x => x.Id == dataSMS.CustomerId).Select(x => x.Telephone).FirstOrDefault();
                                        msgDataSMS.SendDataSMSAsync(sendDataMessage, noSMS);
                                    }
                                }
                            }
                        }
                    }

                    updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendSMS" && x.Active == 1).FirstOrDefault();
                    if (updateLogJob != null)
                    {
                        updateLogJob.JobFinish = DateTime.Now;
                        updateLogJob.Success = 1;
                        updateLogJob.Error = 0;
                        updateLogJob.Description = "Successfully Job Send SMS.";
                        _context.LogJob.Update(updateLogJob);
                        _context.SaveChangesAsync();
                    }
                    bolProcess = false;
                }
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
            catch (Exception ex)
            {
                var updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendSMS" && x.Active == 1).FirstOrDefault();
                if (updateLogJob != null)
                {
                    updateLogJob.JobFinish = DateTime.Now;
                    updateLogJob.Success = 0;
                    updateLogJob.Error = 1;
                    updateLogJob.Description = ex.Message;
                    _context.LogJob.Update(updateLogJob);
                    _context.SaveChangesAsync();
                }
                bolProcess = false;
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }

        }

        public async void SendDataEmail()
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
                    var updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendEmail" && x.Active == 1).FirstOrDefault();
                    if (updateLogJob != null)
                    {
                        updateLogJob.JobStart = DateTime.Now;
                        updateLogJob.JobFinish = null;
                        updateLogJob.Success = 0;
                        updateLogJob.Error = 0;
                        updateLogJob.Description = "";
                        _context.LogJob.Update(updateLogJob);
                        _context.SaveChangesAsync();
                    }

                    var checkSchedule = _context.Schedule.Where(x => x.Active == 1).ToList().OrderBy(x => x.Id);
                    if (checkSchedule.Count() > 0)
                    {
                        foreach (var data in checkSchedule)
                        {
                            var dataSendEmail = _context.SendEmail.Where(x => x.Active == 1 && x.Send == 0 && x.ScheduleId == data.Id).ToList();
                            if (dataSendEmail.Count() > 0)
                            {
                                foreach (var dataEmail in dataSendEmail)
                                {
                                    if (data.ScheduleDate <= DateTime.Now)
                                    {
                                        var sendDataMessage = _context.Message.Where(x => x.Id == dataEmail.MessageId).Select(x => x.MessageData).FirstOrDefault();
                                        var email = _context.Customer.Where(x => x.Id == dataEmail.CustomerId).Select(x => x.Email).FirstOrDefault();
                                        msgDataEmail.SendDataEmail(sendDataMessage, email);
                                    }
                                }
                            }
                        }
                    }

                    updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendEmail" && x.Active == 1).FirstOrDefault();
                    if (updateLogJob != null)
                    {
                        updateLogJob.JobFinish = DateTime.Now;
                        updateLogJob.Success = 1;
                        updateLogJob.Error = 0;
                        updateLogJob.Description = "Successfully Job Send Email.";
                        _context.LogJob.Update(updateLogJob);
                        _context.SaveChangesAsync();
                    }
                    bolProcess = false;
                }
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }
            catch (Exception ex)
            {
                var updateLogJob = _context.LogJob.Where(x => x.JobName == "JobSendEmail" && x.Active == 1).FirstOrDefault();
                if (updateLogJob != null)
                {
                    updateLogJob.JobFinish = DateTime.Now;
                    updateLogJob.Success = 0;
                    updateLogJob.Error = 1;
                    updateLogJob.Description = ex.Message;
                    _context.LogJob.Update(updateLogJob);
                    _context.SaveChangesAsync();
                }

                bolProcess = false;
                Thread.Sleep(Convert.ToInt32(_configuration.GetValue<string>("ScheduleJob:SleepJob")));
            }

        }
    }
}
