using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class MessageRepository : IMessage
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public MessageRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<MessageResponse>> GetMessage(MessageRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MessageResponse>? messageList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.CompanyId != null)
                {
                    messageList = (from msg in _context.Message
                                   join company in _context.Company on msg.CompanyId equals company.Id
                                   where msg.CompanyId == param.CompanyId
                                   select new MessageResponse
                                   {
                                       Id = msg.Id,
                                       MessageData = msg.MessageData,
                                       CompanyId = company.Id,
                                       CompanyName = company.Name,
                                       Active = msg.Active,
                                       CreateBy = msg.CreateBy,
                                       CreateDate = msg.CreateDate,
                                       UpdateBy = msg.UpdateBy,
                                       UpdateDate = msg.UpdateDate
                                   }).OrderBy(x => x.Id).AsNoTracking();
                }

                if (param.CompanyId == null)
                {
                    messageList = (from msg in _context.Message
                                   join company in _context.Company on msg.CompanyId equals company.Id
                                   orderby msg.Id
                                   select new MessageResponse
                                   {
                                       Id = msg.Id,
                                       MessageData = msg.MessageData,
                                       CompanyId = company.Id,
                                       CompanyName = company.Name,
                                       Active = msg.Active,
                                       CreateBy = msg.CreateBy,
                                       CreateDate = msg.CreateDate,
                                       UpdateBy = msg.UpdateBy,
                                       UpdateDate = msg.UpdateDate
                                   }).Take(0).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)messageList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = messageList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetMessage";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return messageList;
            }
        }

        public async Task<IEnumerable<MessageResponse>> CreateMessage(MessageAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MessageResponse>? messageList = null;
            Message messageAdd = new Message();
            try
            {
                messageAdd.CompanyId = param.CompanyId;
                messageAdd.MessageData = param.MessageData;
                messageAdd.Active = param.Active;
                messageAdd.CreateBy = param.CreateBy;
                messageAdd.CreateDate = DateTime.Now;
                _context.Message.Add(messageAdd);
                await _context.SaveChangesAsync();

                messageList = (from com in _context.Company
                               join msg in _context.Message on com.Id equals msg.CompanyId
                               where msg.CompanyId == param.CompanyId && msg.MessageData == param.MessageData
                               select new MessageResponse
                               {
                                   Id = msg.Id,
                                   CompanyId = com.Id,
                                   CompanyName = com.Name,
                                   MessageData = msg.MessageData,
                                   Active = msg.Active,
                                   CreateBy = msg.CreateBy,
                                   CreateDate = msg.CreateDate,
                                   UpdateBy = msg.UpdateBy,
                                   UpdateDate = msg.UpdateDate
                               }).Take(1).AsNoTracking();


                return messageList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateMessage";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return messageList;
            }

        }

        public async Task<IEnumerable<MessageResponse>> UpdateMessage(MessageUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MessageResponse>? messageList = null;
            try
            {
                var messageUpdate = await _context.Message.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (messageUpdate != null)
                {
                    messageUpdate.MessageData = param.MessageData;
                    messageUpdate.Active = param.Active;
                    messageUpdate.UpdateBy = param.UpdateBy;
                    messageUpdate.UpdateDate = DateTime.Now;
                    _context.Message.Update(messageUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    messageList = (from com in _context.Company
                                   join msg in _context.Message on com.Id equals msg.CompanyId
                                   where msg.Id == param.Id
                                   select new MessageResponse
                                   {
                                       Id = msg.Id,
                                       CompanyId = com.Id,
                                       CompanyName = com.Name,
                                       MessageData = msg.MessageData,
                                       Active = msg.Active,
                                       CreateBy = msg.CreateBy,
                                       CreateDate = msg.CreateDate,
                                       UpdateBy = msg.UpdateBy,
                                       UpdateDate = msg.UpdateDate
                                   }).Take(0).AsNoTracking();

                    return messageList;
                }

                messageList = (from com in _context.Company
                               join msg in _context.Message on com.Id equals msg.CompanyId
                               where msg.Id == param.Id
                               select new MessageResponse
                               {
                                   Id = msg.Id,
                                   CompanyId = com.Id,
                                   CompanyName = com.Name,
                                   MessageData = msg.MessageData,
                                   Active = msg.Active,
                                   CreateBy = msg.CreateBy,
                                   CreateDate = msg.CreateDate,
                                   UpdateBy = msg.UpdateBy,
                                   UpdateDate = msg.UpdateDate
                               }).Take(1).AsNoTracking();


                return messageList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateMessage";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return messageList;
            }

        }
    }
}
