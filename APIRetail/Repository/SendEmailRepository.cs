using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class SendEmailRepository : ISendEmail
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public SendEmailRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<SendEmailResponse>> GetSendEmail(SendEmailRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SendEmailResponse>? sendEmailList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.BranchId != null && param.BranchId != 0)
                {
                    if (param.CustomerName != null && param.CustomerName != "")
                    {
                        sendEmailList = (from sw in _context.SendEmail
                                         join com in _context.Company on sw.CompanyId equals com.Id
                                         join br in _context.Branch on sw.BranchId equals br.Id
                                         join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                         join msg in _context.Message on sw.MessageId equals msg.Id
                                         join cus in _context.Customer on sw.CustomerId equals cus.Id
                                         where com.Id == param.CompanyId && br.Id == param.BranchId && param.CustomerName.Contains(cus.Name)
                                         orderby sw.CreateDate descending
                                         select new SendEmailResponse
                                         {
                                             Id = sw.Id,
                                             CompanyId = com.Id,
                                             CompanyName = com.Name,
                                             BranchId = br.Id,
                                             BranchName = br.Name,
                                             ScheduleId = sh.Id,
                                             ScheduleDate = sh.ScheduleDate,
                                             CustomerId = cus.Id,
                                             CustomerName = cus.Name,
                                             MessageId = msg.Id,
                                             MessageData = msg.MessageData,
                                             CreateBy = sw.CreateBy,
                                             CreateDate = sw.CreateDate,
                                             UpdateBy = sw.UpdateBy,
                                             UpdateDate = sw.UpdateDate
                                         }).OrderBy(x => x.CustomerName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                    }
                    else
                    {
                        sendEmailList = (from sw in _context.SendEmail
                                         join com in _context.Company on sw.CompanyId equals com.Id
                                         join br in _context.Branch on sw.BranchId equals br.Id
                                         join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                         join msg in _context.Message on sw.MessageId equals msg.Id
                                         join cus in _context.Customer on sw.CustomerId equals cus.Id
                                         where com.Id == param.CompanyId && br.Id == param.BranchId
                                         orderby sw.CreateDate descending
                                         select new SendEmailResponse
                                         {
                                             Id = sw.Id,
                                             CompanyId = com.Id,
                                             CompanyName = com.Name,
                                             BranchId = br.Id,
                                             BranchName = br.Name,
                                             ScheduleId = sh.Id,
                                             ScheduleDate = sh.ScheduleDate,
                                             CustomerId = cus.Id,
                                             CustomerName = cus.Name,
                                             MessageId = msg.Id,
                                             MessageData = msg.MessageData,
                                             CreateBy = sw.CreateBy,
                                             CreateDate = sw.CreateDate,
                                             UpdateBy = sw.UpdateBy,
                                             UpdateDate = sw.UpdateDate
                                         }).OrderBy(x => x.CustomerName).AsNoTracking();
                    }

                }
                else
                {
                    sendEmailList = (from sw in _context.SendEmail
                                     join com in _context.Company on sw.CompanyId equals com.Id
                                     join br in _context.Branch on sw.BranchId equals br.Id
                                     join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                     join msg in _context.Message on sw.MessageId equals msg.Id
                                     join cus in _context.Customer on sw.CustomerId equals cus.Id
                                     where com.Id == param.CompanyId && br.Id == param.BranchId
                                     orderby sw.CreateDate descending
                                     select new SendEmailResponse
                                     {
                                         Id = sw.Id,
                                         CompanyId = com.Id,
                                         CompanyName = com.Name,
                                         BranchId = br.Id,
                                         BranchName = br.Name,
                                         ScheduleId = sh.Id,
                                         ScheduleDate = sh.ScheduleDate,
                                         CustomerId = cus.Id,
                                         CustomerName = cus.Name,
                                         MessageId = msg.Id,
                                         MessageData = msg.MessageData,
                                         CreateBy = sw.CreateBy,
                                         CreateDate = sw.CreateDate,
                                         UpdateBy = sw.UpdateBy,
                                         UpdateDate = sw.UpdateDate
                                     }).Take(0).AsNoTracking();
                }



                var TotalPageSize = Math.Ceiling((decimal)sendEmailList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = sendEmailList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetSendEmail";
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
                return sendEmailList;
            }
        }

        public async Task<IEnumerable<SendEmailResponse>> CreateSendEmail(SendEmailAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SendEmailResponse>? sendEmailList = null;
            SendEmail sendEmail = new SendEmail();
            try
            {
                var checkSendEmail = await _context.SendEmail.Where(x => x.ScheduleId == param.ScheduleId && x.CustomerId == param.CustomerId && x.MessageId == param.MessageId).AsNoTracking().FirstOrDefaultAsync();
                if (checkSendEmail != null)
                {
                    sendEmailList = (from sw in _context.SendEmail
                                     join com in _context.Company on sw.CompanyId equals com.Id
                                     join br in _context.Branch on sw.BranchId equals br.Id
                                     join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                     join msg in _context.Message on sw.MessageId equals msg.Id
                                     join cus in _context.Customer on sw.CustomerId equals cus.Id
                                     where com.Id == param.CompanyId && br.Id == param.BranchId && sw.ScheduleId == param.ScheduleId
                                             && sw.CustomerId == param.CustomerId && sw.MessageId == param.MessageId
                                     orderby sw.CreateDate descending
                                     select new SendEmailResponse
                                     {
                                         Id = sw.Id,
                                         CompanyId = com.Id,
                                         CompanyName = com.Name,
                                         BranchId = br.Id,
                                         BranchName = br.Name,
                                         ScheduleId = sh.Id,
                                         ScheduleDate = sh.ScheduleDate,
                                         CustomerId = cus.Id,
                                         CustomerName = cus.Name,
                                         MessageId = msg.Id,
                                         MessageData = msg.MessageData,
                                         CreateBy = sw.CreateBy,
                                         CreateDate = sw.CreateDate,
                                         UpdateBy = sw.UpdateBy,
                                         UpdateDate = sw.UpdateDate
                                     }).Take(0).AsNoTracking();

                    return sendEmailList;

                }
                else
                {
                    sendEmail.CompanyId = param.CompanyId;
                    sendEmail.BranchId = param.BranchId;
                    sendEmail.ScheduleId = param.ScheduleId;
                    sendEmail.MessageId = param.MessageId;
                    sendEmail.CustomerId = param.CustomerId;
                    sendEmail.Active = param.Active;
                    sendEmail.CreateBy = param.CreateBy;
                    sendEmail.CreateDate = DateTime.Now;
                    _context.SendEmail.Add(sendEmail);
                    await _context.SaveChangesAsync();
                }


                sendEmailList = (from sw in _context.SendEmail
                                 join com in _context.Company on sw.CompanyId equals com.Id
                                 join br in _context.Branch on sw.BranchId equals br.Id
                                 join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                 join msg in _context.Message on sw.MessageId equals msg.Id
                                 join cus in _context.Customer on sw.CustomerId equals cus.Id
                                 where com.Id == param.CompanyId && br.Id == param.BranchId && sw.ScheduleId == param.ScheduleId
                                         && sw.CustomerId == param.CustomerId && sw.MessageId == param.MessageId
                                 orderby sw.CreateDate descending
                                 select new SendEmailResponse
                                 {
                                     Id = sw.Id,
                                     CompanyId = com.Id,
                                     CompanyName = com.Name,
                                     BranchId = br.Id,
                                     BranchName = br.Name,
                                     ScheduleId = sh.Id,
                                     ScheduleDate = sh.ScheduleDate,
                                     CustomerId = cus.Id,
                                     CustomerName = cus.Name,
                                     MessageId = msg.Id,
                                     MessageData = msg.MessageData,
                                     CreateBy = sw.CreateBy,
                                     CreateDate = sw.CreateDate,
                                     UpdateBy = sw.UpdateBy,
                                     UpdateDate = sw.UpdateDate
                                 }).Take(1).AsNoTracking();

                return sendEmailList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateSendEmail";
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
                return sendEmailList;
            }

        }

        public async Task<IEnumerable<SendEmailResponse>> UpdateSendEmail(SendEmailUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SendEmailResponse>? sendEmailList = null;
            SendEmail sendEmail = new SendEmail();
            try
            {
                var checkSendEmail = await _context.SendEmail.Where(x => x.ScheduleId == param.ScheduleId && x.CustomerId == param.CustomerId && x.MessageId == param.MessageId).AsNoTracking().FirstOrDefaultAsync();
                if (checkSendEmail != null)
                {
                    sendEmailList = (from sw in _context.SendEmail
                                     join com in _context.Company on sw.CompanyId equals com.Id
                                     join br in _context.Branch on sw.BranchId equals br.Id
                                     join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                     join msg in _context.Message on sw.MessageId equals msg.Id
                                     join cus in _context.Customer on sw.CustomerId equals cus.Id
                                     where sw.Id == param.Id
                                     orderby sw.CreateDate descending
                                     select new SendEmailResponse
                                     {
                                         Id = sw.Id,
                                         CompanyId = com.Id,
                                         CompanyName = com.Name,
                                         BranchId = br.Id,
                                         BranchName = br.Name,
                                         ScheduleId = sh.Id,
                                         ScheduleDate = sh.ScheduleDate,
                                         CustomerId = cus.Id,
                                         CustomerName = cus.Name,
                                         MessageId = msg.Id,
                                         MessageData = msg.MessageData,
                                         CreateBy = sw.CreateBy,
                                         CreateDate = sw.CreateDate,
                                         UpdateBy = sw.UpdateBy,
                                         UpdateDate = sw.UpdateDate
                                     }).Take(0).AsNoTracking();

                    return sendEmailList;

                }
                else
                {
                    var sendEmailUpdate = await _context.SendEmail.Where(x => x.Id == param.Id).FirstOrDefaultAsync();
                    if (sendEmailUpdate != null)
                    {
                        sendEmailUpdate.ScheduleId = param.ScheduleId;
                        sendEmailUpdate.MessageId = param.MessageId;
                        sendEmailUpdate.CustomerId = param.CustomerId;
                        sendEmailUpdate.Active = param.Active;
                        sendEmailUpdate.UpdateBy = param.UpdateBy;
                        sendEmailUpdate.UpdateDate = DateTime.Now;
                        _context.SendEmail.Update(sendEmailUpdate);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        sendEmailList = (from sw in _context.SendEmail
                                         join com in _context.Company on sw.CompanyId equals com.Id
                                         join br in _context.Branch on sw.BranchId equals br.Id
                                         join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                         join msg in _context.Message on sw.MessageId equals msg.Id
                                         join cus in _context.Customer on sw.CustomerId equals cus.Id
                                         where sw.Id == param.Id
                                         orderby sw.CreateDate descending
                                         select new SendEmailResponse
                                         {
                                             Id = sw.Id,
                                             CompanyId = com.Id,
                                             CompanyName = com.Name,
                                             BranchId = br.Id,
                                             BranchName = br.Name,
                                             ScheduleId = sh.Id,
                                             ScheduleDate = sh.ScheduleDate,
                                             CustomerId = cus.Id,
                                             CustomerName = cus.Name,
                                             MessageId = msg.Id,
                                             MessageData = msg.MessageData,
                                             CreateBy = sw.CreateBy,
                                             CreateDate = sw.CreateDate,
                                             UpdateBy = sw.UpdateBy,
                                             UpdateDate = sw.UpdateDate
                                         }).Take(0).AsNoTracking();

                        return sendEmailList;
                    }
                }


                sendEmailList = (from sw in _context.SendEmail
                                 join com in _context.Company on sw.CompanyId equals com.Id
                                 join br in _context.Branch on sw.BranchId equals br.Id
                                 join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                 join msg in _context.Message on sw.MessageId equals msg.Id
                                 join cus in _context.Customer on sw.CustomerId equals cus.Id
                                 where sw.Id == param.Id
                                 orderby sw.CreateDate descending
                                 select new SendEmailResponse
                                 {
                                     Id = sw.Id,
                                     CompanyId = com.Id,
                                     CompanyName = com.Name,
                                     BranchId = br.Id,
                                     BranchName = br.Name,
                                     ScheduleId = sh.Id,
                                     ScheduleDate = sh.ScheduleDate,
                                     CustomerId = cus.Id,
                                     CustomerName = cus.Name,
                                     MessageId = msg.Id,
                                     MessageData = msg.MessageData,
                                     CreateBy = sw.CreateBy,
                                     CreateDate = sw.CreateDate,
                                     UpdateBy = sw.UpdateBy,
                                     UpdateDate = sw.UpdateDate
                                 }).Take(1).AsNoTracking();

                return sendEmailList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateSendWhatsApp";
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
                return sendEmailList;
            }

        }
    }
}
