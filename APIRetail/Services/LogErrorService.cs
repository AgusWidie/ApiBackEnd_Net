using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class LogErrorService : ILogErrorService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logErrorRepo;

        public LogErrorService(IConfiguration Configuration, retail_systemContext context, ILogError logErrorRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logErrorRepo = logErrorRepo;

        }

        public async Task<List<LogErrorResponse>> GetCompany(LogErrorRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listError.Count() > 0)
                {
                    long Page = param.Page - 1;
                    var resultList = GeneralList._listError.Where(x => param.ServiceName.Contains(x.ServiceName));
                    var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                    param.TotalPageSize = (long)TotalPageSize;
                    return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                }
                else
                {
                    var resultList = await _logErrorRepo.GetLogError(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<LogErrorResponse>> CreateLogError(LogErrorAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _logErrorRepo.CreateLogError(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listError.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }
    }
}
