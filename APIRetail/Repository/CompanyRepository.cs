using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{

    public class CompanyRepository : ICompany
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public CompanyRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<CompanyResponse>> GetCompany(CompanyRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CompanyResponse>? companyList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Name != null && param.Name != "")
                {
                    companyList = (from company in _context.Company
                                   where company.Name == param.Name
                                   select new CompanyResponse
                                   {
                                       Id = company.Id,
                                       Name = company.Name,
                                       Address = company.Address,
                                       Telp = company.Telp,
                                       Fax = company.Fax,
                                       CreateBy = company.CreateBy,
                                       CreateDate = company.CreateDate,
                                       UpdateBy = company.UpdateBy,
                                       UpdateDate = company.UpdateDate
                                   }).OrderBy(x => x.Name).AsNoTracking();
                }

                if (param.Name == null || param.Name == "")
                {
                    companyList = (from company in _context.Company
                                   select new CompanyResponse
                                   {
                                       Id = company.Id,
                                       Name = company.Name,
                                       Address = company.Address,
                                       Telp = company.Telp,
                                       Fax = company.Fax,
                                       CreateBy = company.CreateBy,
                                       CreateDate = company.CreateDate,
                                       UpdateBy = company.UpdateBy,
                                       UpdateDate = company.UpdateDate
                                   }).OrderBy(x => x.Name).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)companyList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = companyList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;

            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetCompany";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return companyList;
            }
        }

        public async Task<IEnumerable<CompanyResponse>> CreateCompany(CompanyAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CompanyResponse>? companyList = null;
            Company companyAdd = new Company();
            try
            {
                companyAdd.Name = param.Name;
                companyAdd.Address = param.Address;
                companyAdd.Telp = param.Telp;
                companyAdd.Fax = param.Fax;
                companyAdd.CreateBy = param.CreateBy;
                companyAdd.CreateDate = DateTime.Now;
                _context.Company.Add(companyAdd);
                await _context.SaveChangesAsync();

                companyList = (from company in _context.Company
                               where company.Name == param.Name
                               select new CompanyResponse
                               {
                                   Id = company.Id,
                                   Name = company.Name,
                                   Address = company.Address,
                                   Telp = company.Telp,
                                   Fax = company.Fax,
                                   CreateBy = company.CreateBy,
                                   CreateDate = company.CreateDate,
                                   UpdateBy = company.UpdateBy,
                                   UpdateDate = company.UpdateDate
                               }).Take(1).AsNoTracking();


                return companyList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateCompany";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return companyList;
            }

        }

        public async Task<IEnumerable<CompanyResponse>> UpdateCompany(CompanyUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CompanyResponse>? companyList = null;
            try
            {
                var companyUpdate = await _context.Company.Where(x => x.Id == param.Id).FirstOrDefaultAsync();
                if (companyUpdate != null)
                {
                    companyUpdate.Name = param.Name;
                    companyUpdate.Address = param.Address;
                    companyUpdate.Telp = param.Telp;
                    companyUpdate.Fax = param.Fax;
                    companyUpdate.UpdateBy = param.UpdateBy;
                    companyUpdate.UpdateDate = DateTime.Now;
                    _context.Company.Update(companyUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    companyList = (from company in _context.Company
                                   where company.Id == param.Id
                                   select new CompanyResponse
                                   {
                                       Id = company.Id,
                                       Name = company.Name,
                                       Address = company.Address,
                                       Telp = company.Telp,
                                       Fax = company.Fax,
                                       CreateBy = company.CreateBy,
                                       CreateDate = company.CreateDate,
                                       UpdateBy = company.UpdateBy,
                                       UpdateDate = company.UpdateDate
                                   }).Take(0).AsNoTracking();

                    return companyList;
                }

                companyList = (from company in _context.Company
                               where company.Id == param.Id
                               select new CompanyResponse
                               {
                                   Id = company.Id,
                                   Name = company.Name,
                                   Address = company.Address,
                                   Telp = company.Telp,
                                   Fax = company.Fax,
                                   CreateBy = company.CreateBy,
                                   CreateDate = company.CreateDate,
                                   UpdateBy = company.UpdateBy,
                                   UpdateDate = company.UpdateDate
                               }).Take(1).AsNoTracking();


                return companyList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateCompany";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return companyList;
            }

        }

    }
}
