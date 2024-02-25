using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class CompanyService : ICompanyService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly ICompany _companyRepo;

        public CompanyService(IConfiguration Configuration, retail_systemContext context, ILogError logError, ICompany companyRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _companyRepo = companyRepo;
        }

        public async Task<List<CompanyResponse>> GetCompany(CompanyRequest param, CancellationToken cancellationToken)
        {

            try
            {

                GarbageCollector.GarbageCollection();
                if (GeneralList._listCompany.Count() > 0)
                {
                    if (param.Name != null && param.Name != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listCompany.Where(x => param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();

                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listCompany;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _companyRepo.GetCompany(param, cancellationToken);
                    return resultList.ToList();
                }
            }

            catch (Exception ex)
            {
                GeneralList._listCompany.Clear();
                throw;
            }

        }

        public async Task<List<CompanyResponse>> CreateCompany(CompanyAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _companyRepo.CreateCompany(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listCompany.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listCompany.Clear();
                throw;
            }


        }

        public async Task<List<CompanyResponse>> UpdateCompany(CompanyUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _companyRepo.UpdateCompany(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listCompany.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
                        checkData.Address = resultData.First().Address;
                        checkData.Telp = resultData.First().Telp;
                        checkData.Fax = resultData.First().Fax;
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
                GeneralList._listCompany.Clear();
                throw;
            }


        }
    }
}
