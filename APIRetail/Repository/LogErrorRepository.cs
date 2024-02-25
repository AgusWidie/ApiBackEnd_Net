using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class LogErrorRepository : ILogError
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;

        public LogErrorRepository(IConfiguration Configuration, retail_systemContext context)
        {
            _configuration = Configuration;
            _context = context;
        }

        public async Task<IEnumerable<LogErrorResponse>> GetLogError(LogErrorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LogErrorResponse>? logErrorList = null;
            try
            {
                long Page = param.Page - 1;
                logErrorList = (from logError in _context.LogError
                                where logError.ServiceName.Contains(param.ServiceName) && logError.ErrorDeskripsi.Contains(param.ErrorDeskripsi)
                                orderby logError.CreateDate descending
                                select new LogErrorResponse
                                {
                                    Id = logError.Id,
                                    ServiceName = logError.ServiceName,
                                    ErrorDeskripsi = logError.ErrorDeskripsi,
                                    CreateBy = logError.CreateBy,
                                    CreateDate = logError.CreateDate
                                }).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);

                return logErrorList;
            }
            catch (Exception ex)
            {
                return logErrorList;
            }
        }

        public async Task<IEnumerable<LogErrorResponse>> CreateLogError(LogErrorAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LogErrorResponse>? logErrorList = null;
            try
            {
                LogError logDataError = new LogError();
                logDataError.ServiceName = param.ServiceName;
                logDataError.ErrorDeskripsi = param.ErrorDeskripsi;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = param.CreateBy;
                _context.LogError.Add(logDataError);
                await _context.SaveChangesAsync();

                logErrorList = (from logError in _context.LogError
                                where logError.ServiceName == param.ServiceName && logError.ErrorDeskripsi == param.ErrorDeskripsi
                                orderby logError.CreateDate descending
                                select new LogErrorResponse
                                {
                                    Id = logError.Id,
                                    ServiceName = logError.ServiceName,
                                    ErrorDeskripsi = logError.ErrorDeskripsi,
                                    CreateBy = logError.CreateBy,
                                    CreateDate = logError.CreateDate
                                }).Take(1).AsNoTracking();

                return logErrorList;
            }
            catch (Exception ex)
            {
                return logErrorList;
            }
        }
    }
}
