using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class CustomerService : ICustomerService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly ICustomer _customerRepo;

        public CustomerService(IConfiguration Configuration, retail_systemContext context, ILogError logError, ICustomer customerRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _customerRepo = customerRepo;
        }

        public async Task<List<CustomerResponse>> GetCustomer(CustomerRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listCustomer.Count() > 0)
                {
                    if (param.Name != null && param.Name != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listCustomer.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listCustomer;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _customerRepo.GetCustomer(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listCustomer.Clear();
                throw;
            }

        }

        public async Task<List<CustomerResponse>> CreateCustomer(CustomerAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _customerRepo.CreateCustomer(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listCustomer.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listCustomer.Clear();
                throw;
            }


        }
        public async Task<List<CustomerResponse>> UpdateCustomer(CustomerUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _customerRepo.UpdateCustomer(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listCustomer.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
                        checkData.Address = resultData.First().Address;
                        checkData.Telephone = resultData.First().Telephone;
                        checkData.WhatsApp = resultData.First().WhatsApp;
                        checkData.Email = resultData.First().Email;
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
                GeneralList._listCustomer.Clear();
                throw;
            }


        }

    }
}
