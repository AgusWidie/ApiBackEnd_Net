using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class MessageService : IMessageService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IMessage _messageRepo;

        public MessageService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IMessage messageRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _messageRepo = messageRepo;
        }

        public async Task<List<MessageResponse>> GetMessage(MessageRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listMessage.Count() > 0)
                {
                    if (param.CompanyId != null && param.CompanyId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listMessage.Where(x => x.CompanyId == param.CompanyId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listMessage;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _messageRepo.GetMessage(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listMenu.Clear();
                throw;
            }

        }

        public async Task<List<MessageResponse>> CreateMessage(MessageAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _messageRepo.CreateMessage(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listMessage.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listMenu.Clear();
                throw;
            }


        }

        public async Task<List<MessageResponse>> UpdateMessage(MessageUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _messageRepo.UpdateMessage(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listMessage.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.MessageData = resultData.First().MessageData;
                        checkData.Active = resultData.First().Active;
                        checkData.UpdateBy = resultData.First().UpdateBy;
                        checkData.UpdateDate = DateTime.Now;

                    }
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listMessage.Clear();
                throw;
            }


        }
    }
}
