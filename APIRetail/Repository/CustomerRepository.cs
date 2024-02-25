using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class CustomerRepository : ICustomer
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public CustomerRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<CustomerResponse>> GetCustomer(CustomerRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerResponse>? customerList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Name != null && param.Name != "")
                {
                    customerList = (from customer in _context.Customer
                                    join com in _context.Company on customer.CompanyId equals com.Id
                                    join br in _context.Branch on customer.BranchId equals br.Id
                                    where com.Id == param.CompanyId && br.Id == param.BranchId && customer.Name.Contains(param.Name)
                                    select new CustomerResponse
                                    {
                                        Id = customer.Id,
                                        CompanyId = com.Id,
                                        CompanyName = com.Name,
                                        BranchId = br.Id,
                                        BranchName = br.Name,
                                        Name = customer.Name,
                                        Address = customer.Address,
                                        Telephone = customer.Telephone,
                                        WhatsApp = customer.WhatsApp,
                                        Email = customer.Email,
                                        CreateBy = customer.CreateBy,
                                        CreateDate = customer.CreateDate,
                                        UpdateBy = customer.UpdateBy,
                                        UpdateDate = customer.UpdateDate
                                    }).OrderBy(x => x.Name).AsNoTracking();
                }

                if (param.Name == null || param.Name == "")
                {
                    customerList = (from customer in _context.Customer
                                    join com in _context.Company on customer.CompanyId equals com.Id
                                    join br in _context.Branch on customer.BranchId equals br.Id
                                    where com.Id == param.CompanyId && br.Id == param.BranchId
                                    select new CustomerResponse
                                    {
                                        Id = customer.Id,
                                        CompanyId = com.Id,
                                        CompanyName = com.Name,
                                        BranchId = br.Id,
                                        BranchName = br.Name,
                                        Name = customer.Name,
                                        Address = customer.Address,
                                        Telephone = customer.Telephone,
                                        WhatsApp = customer.WhatsApp,
                                        Email = customer.Email,
                                        CreateBy = customer.CreateBy,
                                        CreateDate = customer.CreateDate,
                                        UpdateBy = customer.UpdateBy,
                                        UpdateDate = customer.UpdateDate
                                    }).OrderBy(x => x.Name).AsNoTracking();
                }



                var TotalPageSize = Math.Ceiling((decimal)customerList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = customerList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetCustomer";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return customerList;
            }
        }

        public async Task<IEnumerable<CustomerResponse>> CreateCustomer(CustomerAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerResponse>? customerList = null;
            Customer customerAdd = new Customer();
            try
            {
                customerAdd.CompanyId = param.CompanyId;
                customerAdd.BranchId = param.BranchId;
                customerAdd.Name = param.Name;
                customerAdd.Address = param.Address;
                customerAdd.Telephone = param.Telephone;
                customerAdd.WhatsApp = param.WhatsApp;
                customerAdd.Email = param.Email;
                customerAdd.CreateBy = param.CreateBy;
                customerAdd.CreateDate = DateTime.Now;
                _context.Customer.Add(customerAdd);
                await _context.SaveChangesAsync();

                customerList = (from customer in _context.Customer
                                join com in _context.Company on customer.CompanyId equals com.Id
                                join br in _context.Branch on customer.BranchId equals br.Id
                                where com.Id == param.CompanyId && br.Id == param.BranchId && customer.Name == param.Name
                                select new CustomerResponse
                                {
                                    Id = customer.Id,
                                    CompanyId = com.Id,
                                    CompanyName = com.Name,
                                    BranchId = br.Id,
                                    BranchName = br.Name,
                                    Name = customer.Name,
                                    Address = customer.Address,
                                    Telephone = customer.Telephone,
                                    WhatsApp = customer.WhatsApp,
                                    Email = customer.Email,
                                    CreateBy = customer.CreateBy,
                                    CreateDate = customer.CreateDate,
                                    UpdateBy = customer.UpdateBy,
                                    UpdateDate = customer.UpdateDate
                                }).Take(1).AsNoTracking();


                return customerList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateCustomer";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return customerList;
            }

        }

        public async Task<IEnumerable<CustomerResponse>> UpdateCustomer(CustomerUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CustomerResponse>? customerList = null;
            try
            {
                var customerUpdate = await _context.Customer.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (customerUpdate != null)
                {
                    customerUpdate.CompanyId = param.CompanyId;
                    customerUpdate.BranchId = param.BranchId;
                    customerUpdate.Name = param.Name;
                    customerUpdate.Address = param.Address;
                    customerUpdate.Telephone = param.Telephone;
                    customerUpdate.WhatsApp = param.WhatsApp;
                    customerUpdate.Email = param.Email;
                    customerUpdate.UpdateBy = param.UpdateBy;
                    customerUpdate.UpdateDate = DateTime.Now;
                    _context.Customer.Update(customerUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    customerList = (from customer in _context.Customer
                                    join com in _context.Company on customer.CompanyId equals com.Id
                                    join br in _context.Branch on customer.BranchId equals br.Id
                                    where customer.Id == param.Id
                                    select new CustomerResponse
                                    {
                                        Id = customer.Id,
                                        CompanyId = com.Id,
                                        CompanyName = com.Name,
                                        BranchId = br.Id,
                                        BranchName = br.Name,
                                        Name = customer.Name,
                                        Address = customer.Address,
                                        Telephone = customer.Telephone,
                                        WhatsApp = customer.WhatsApp,
                                        Email = customer.Email,
                                        CreateBy = customer.CreateBy,
                                        CreateDate = customer.CreateDate,
                                        UpdateBy = customer.UpdateBy,
                                        UpdateDate = customer.UpdateDate
                                    }).Take(0).AsNoTracking();

                    return customerList;
                }


                customerList = (from customer in _context.Customer
                                join com in _context.Company on customer.CompanyId equals com.Id
                                join br in _context.Branch on customer.BranchId equals br.Id
                                where customer.Id == param.Id
                                select new CustomerResponse
                                {
                                    Id = customer.Id,
                                    CompanyId = com.Id,
                                    CompanyName = com.Name,
                                    BranchId = br.Id,
                                    BranchName = br.Name,
                                    Name = customer.Name,
                                    Address = customer.Address,
                                    Telephone = customer.Telephone,
                                    WhatsApp = customer.WhatsApp,
                                    Email = customer.Email,
                                    CreateBy = customer.CreateBy,
                                    CreateDate = customer.CreateDate,
                                    UpdateBy = customer.UpdateBy,
                                    UpdateDate = customer.UpdateDate
                                }).Take(1).AsNoTracking();


                return customerList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateCustomer";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return customerList;
            }

        }
    }
}
