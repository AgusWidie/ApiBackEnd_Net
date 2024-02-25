using APIRetail.Models.Database;
using APIRetail.Repository.IRepository;
using System.Net.Mail;
using System.Net;

namespace APIRetail.Helper
{
    public class SendDataMessageEmail
    {
        public readonly IConfiguration _configuration;
        public readonly ILogError _logError;
        public readonly retail_systemContext _context;
        public SendDataMessageEmail(IConfiguration Configuration, ILogError logError, retail_systemContext context)
        {
            _configuration = Configuration;
            _logError = logError;
            _context = context;
        }

        public void SendDataEmail(string? SendMessage, string? ToEmail)
        {
            try
            {
                string? FromEmail = _configuration.GetValue<string>("ScheduleJob:FromEmail");
                MailAddress to = new MailAddress(ToEmail);
                MailAddress from = new MailAddress(FromEmail);
                MailMessage email = new MailMessage(from, to);
                email.Subject = "Broadcast Email";
                email.Body = SendMessage;
                email.BodyEncoding = System.Text.Encoding.UTF8;
                email.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 465;
                smtp.Credentials = new NetworkCredential(_configuration.GetValue<string>("ScheduleJob:UserEmail"), _configuration.GetValue<string>("ScheduleJob:PasswordEmail"));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                smtp.Send(email);
            }
            catch (Exception ex)
            {

                SendEmailFail sendEmailFailRequest = new SendEmailFail();
                sendEmailFailRequest.Email = ToEmail;
                sendEmailFailRequest.Message = SendMessage;
                sendEmailFailRequest.ErrorDescription = "Error SendDataEmail : " + ex.Message;
                sendEmailFailRequest.CreateBy = "System";
                sendEmailFailRequest.CreateDate = DateTime.Now;
                _context.SendEmailFail.Add(sendEmailFailRequest);
                _context.SaveChanges();
            }

        }
    }
}
