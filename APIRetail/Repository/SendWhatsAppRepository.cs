using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class SendWhatsAppRepository : ISendWhatsApp
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public SendWhatsAppRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<SendWhatsAppResponse>> GetSendWhatsApp(SendWhatsAppRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SendWhatsAppResponse>? sendWhatsAppList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.BranchId != null && param.BranchId != 0)
                {
                    if (param.CustomerName != null && param.CustomerName != "")
                    {
                        sendWhatsAppList = (from sw in _context.SendWhatsapp
                                            join com in _context.Company on sw.CompanyId equals com.Id
                                            join br in _context.Branch on sw.BranchId equals br.Id
                                            join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                            join msg in _context.Message on sw.MessageId equals msg.Id
                                            join cus in _context.Customer on sw.CustomerId equals cus.Id
                                            where com.Id == param.CompanyId && br.Id == param.BranchId && param.CustomerName.Contains(cus.Name)
                                            orderby sw.CreateDate descending
                                            select new SendWhatsAppResponse
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
                        sendWhatsAppList = (from sw in _context.SendWhatsapp
                                            join com in _context.Company on sw.CompanyId equals com.Id
                                            join br in _context.Branch on sw.BranchId equals br.Id
                                            join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                            join msg in _context.Message on sw.MessageId equals msg.Id
                                            join cus in _context.Customer on sw.CustomerId equals cus.Id
                                            where com.Id == param.CompanyId && br.Id == param.BranchId
                                            orderby sw.CreateDate descending
                                            select new SendWhatsAppResponse
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
                    sendWhatsAppList = (from sw in _context.SendWhatsapp
                                        join com in _context.Company on sw.CompanyId equals com.Id
                                        join br in _context.Branch on sw.BranchId equals br.Id
                                        join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                        join msg in _context.Message on sw.MessageId equals msg.Id
                                        join cus in _context.Customer on sw.CustomerId equals cus.Id
                                        where com.Id == param.CompanyId && br.Id == param.BranchId
                                        orderby sw.CreateDate descending
                                        select new SendWhatsAppResponse
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



                var TotalPageSize = Math.Ceiling((decimal)sendWhatsAppList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = sendWhatsAppList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetSendWhatsApp";
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
                return sendWhatsAppList;
            }
        }

        public async Task<IEnumerable<SendWhatsAppResponse>> CreateSendWhatsApp(SendWhatsAppAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SendWhatsAppResponse>? sendWhatsAppList = null;
            SendWhatsapp sendWhatsApp = new SendWhatsapp();
            try
            {
                var checkSendWhatsApp = await _context.SendWhatsapp.Where(x => x.ScheduleId == param.ScheduleId && x.CustomerId == param.CustomerId && x.MessageId == param.MessageId).AsNoTracking().FirstOrDefaultAsync();
                if (checkSendWhatsApp != null)
                {
                    sendWhatsAppList = (from sw in _context.SendWhatsapp
                                        join com in _context.Company on sw.CompanyId equals com.Id
                                        join br in _context.Branch on sw.BranchId equals br.Id
                                        join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                        join msg in _context.Message on sw.MessageId equals msg.Id
                                        join cus in _context.Customer on sw.CustomerId equals cus.Id
                                        where com.Id == param.CompanyId && br.Id == param.BranchId && sw.ScheduleId == param.ScheduleId
                                             && sw.CustomerId == param.CustomerId && sw.MessageId == param.MessageId
                                        orderby sw.CreateDate descending
                                        select new SendWhatsAppResponse
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

                    return sendWhatsAppList;

                }
                else
                {
                    sendWhatsApp.CompanyId = param.CompanyId;
                    sendWhatsApp.BranchId = param.BranchId;
                    sendWhatsApp.ScheduleId = param.ScheduleId;
                    sendWhatsApp.MessageId = param.MessageId;
                    sendWhatsApp.CustomerId = param.CustomerId;
                    sendWhatsApp.Active = param.Active;
                    sendWhatsApp.CreateBy = param.CreateBy;
                    sendWhatsApp.CreateDate = DateTime.Now;
                    _context.SendWhatsapp.Add(sendWhatsApp);
                    await _context.SaveChangesAsync();
                }


                sendWhatsAppList = (from sw in _context.SendWhatsapp
                                    join com in _context.Company on sw.CompanyId equals com.Id
                                    join br in _context.Branch on sw.BranchId equals br.Id
                                    join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                    join msg in _context.Message on sw.MessageId equals msg.Id
                                    join cus in _context.Customer on sw.CustomerId equals cus.Id
                                    where com.Id == param.CompanyId && br.Id == param.BranchId && sw.ScheduleId == param.ScheduleId
                                         && sw.CustomerId == param.CustomerId && sw.MessageId == param.MessageId
                                    orderby sw.CreateDate descending
                                    select new SendWhatsAppResponse
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

                return sendWhatsAppList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateSendWhatsApp";
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
                return sendWhatsAppList;
            }

        }

        public async Task<IEnumerable<SendWhatsAppResponse>> UpdateSendWhatsApp(SendWhatsAppUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SendWhatsAppResponse>? sendWhatsAppList = null;
            SendWhatsapp sendWhatsApp = new SendWhatsapp();
            try
            {
                var checkSendWhatsApp = await _context.SendWhatsapp.Where(x => x.ScheduleId == param.ScheduleId && x.CustomerId == param.CustomerId && x.MessageId == param.MessageId).AsNoTracking().FirstOrDefaultAsync();
                if (checkSendWhatsApp != null)
                {
                    sendWhatsAppList = (from sw in _context.SendWhatsapp
                                        join com in _context.Company on sw.CompanyId equals com.Id
                                        join br in _context.Branch on sw.BranchId equals br.Id
                                        join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                        join msg in _context.Message on sw.MessageId equals msg.Id
                                        join cus in _context.Customer on sw.CustomerId equals cus.Id
                                        where sw.Id == param.Id
                                        orderby sw.CreateDate descending
                                        select new SendWhatsAppResponse
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

                    return sendWhatsAppList;

                }
                else
                {
                    var sendWhatsAppUpdate = await _context.SendWhatsapp.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                    if (sendWhatsAppUpdate != null)
                    {
                        sendWhatsAppUpdate.ScheduleId = param.ScheduleId;
                        sendWhatsAppUpdate.MessageId = param.MessageId;
                        sendWhatsAppUpdate.CustomerId = param.CustomerId;
                        sendWhatsAppUpdate.Active = param.Active;
                        sendWhatsAppUpdate.UpdateBy = param.UpdateBy;
                        sendWhatsAppUpdate.UpdateDate = DateTime.Now;
                        _context.SendWhatsapp.Update(sendWhatsApp);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        sendWhatsAppList = (from sw in _context.SendWhatsapp
                                            join com in _context.Company on sw.CompanyId equals com.Id
                                            join br in _context.Branch on sw.BranchId equals br.Id
                                            join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                            join msg in _context.Message on sw.MessageId equals msg.Id
                                            join cus in _context.Customer on sw.CustomerId equals cus.Id
                                            where sw.Id == param.Id
                                            orderby sw.CreateDate descending
                                            select new SendWhatsAppResponse
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

                        return sendWhatsAppList;
                    }
                }


                sendWhatsAppList = (from sw in _context.SendWhatsapp
                                    join com in _context.Company on sw.CompanyId equals com.Id
                                    join br in _context.Branch on sw.BranchId equals br.Id
                                    join sh in _context.Schedule on sw.ScheduleId equals sh.Id
                                    join msg in _context.Message on sw.MessageId equals msg.Id
                                    join cus in _context.Customer on sw.CustomerId equals cus.Id
                                    where sw.Id == param.Id
                                    orderby sw.CreateDate descending
                                    select new SendWhatsAppResponse
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

                return sendWhatsAppList;
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
                return sendWhatsAppList;
            }

        }

    }


}
