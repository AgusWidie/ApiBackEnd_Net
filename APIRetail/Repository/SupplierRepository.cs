using APIRetail.Crypto;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class SupplierRepository : ISupplier
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public SupplierRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<SupplierResponse>> GetSupplier(SupplierRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SupplierResponse>? supplierList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.SupplierName == null || param.SupplierName == "")
                {
                    supplierList = (from supplier in _context.Supplier
                                    join branch in _context.Branch on supplier.BranchId equals branch.Id
                                    join company in _context.Company on supplier.CompanyId equals company.Id
                                    where supplier.CompanyId == param.CompanyId && supplier.BranchId == param.BranchId
                                    select new SupplierResponse
                                    {
                                        Id = supplier.Id,
                                        CompanyId = supplier.CompanyId,
                                        CompanyName = company.Name,
                                        BranchId = supplier.BranchId,
                                        BranchName = branch.Name,
                                        Name = supplier.Name,
                                        Address = supplier.Address,
                                        Telp = supplier.Telp,
                                        Fax = supplier.Fax,
                                        Active = supplier.Active,
                                        CreateBy = supplier.CreateBy,
                                        CreateDate = supplier.CreateDate,
                                        UpdateBy = supplier.UpdateBy,
                                        UpdateDate = supplier.UpdateDate
                                    }).OrderBy(x => x.Name).AsNoTracking();
                }

                if (param.SupplierName != null && param.SupplierName != "")
                {
                    supplierList = (from supplier in _context.Supplier
                                    join branch in _context.Branch on supplier.BranchId equals branch.Id
                                    join company in _context.Company on supplier.CompanyId equals company.Id
                                    where supplier.CompanyId == param.CompanyId && supplier.BranchId == param.BranchId && supplier.Name.Contains(param.SupplierName)
                                    select new SupplierResponse
                                    {
                                        Id = supplier.Id,
                                        CompanyId = supplier.CompanyId,
                                        CompanyName = company.Name,
                                        BranchId = supplier.BranchId,
                                        BranchName = branch.Name,
                                        Name = supplier.Name,
                                        Address = supplier.Address,
                                        Telp = supplier.Telp,
                                        Fax = supplier.Fax,
                                        Active = supplier.Active,
                                        CreateBy = supplier.CreateBy,
                                        CreateDate = supplier.CreateDate,
                                        UpdateBy = supplier.UpdateBy,
                                        UpdateDate = supplier.UpdateDate
                                    }).OrderBy(x => x.Name).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)supplierList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = supplierList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetSupplier";
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
                return supplierList;
            }
        }

        public async Task<IEnumerable<SupplierResponse>> CreateSupplier(SupplierAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SupplierResponse>? supplierList = null;
            Supplier supplierAdd = new Supplier();
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                supplierAdd.CompanyId = param.CompanyId;
                supplierAdd.BranchId = param.BranchId;
                supplierAdd.Name = param.Name;
                supplierAdd.Address = param.Address;
                supplierAdd.Telp = param.Telp;
                supplierAdd.Fax = param.Fax;
                supplierAdd.Active = param.Active;
                supplierAdd.CreateBy = param.CreateBy;
                supplierAdd.CreateDate = DateTime.Now;
                _context.Supplier.Add(supplierAdd);
                await _context.SaveChangesAsync();

                supplierList = (from supplier in _context.Supplier
                                join branch in _context.Branch on supplier.BranchId equals branch.Id
                                join company in _context.Company on supplier.CompanyId equals company.Id
                                where supplier.CompanyId == param.CompanyId && supplier.BranchId == param.BranchId && supplier.Name == param.Name
                                select new SupplierResponse
                                {
                                    Id = supplier.Id,
                                    CompanyId = supplier.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = supplier.BranchId,
                                    BranchName = branch.Name,
                                    Name = supplier.Name,
                                    Address = supplier.Address,
                                    Telp = supplier.Telp,
                                    Fax = supplier.Fax,
                                    Active = supplier.Active,
                                    CreateBy = supplier.CreateBy,
                                    CreateDate = supplier.CreateDate,
                                    UpdateBy = supplier.UpdateBy,
                                    UpdateDate = supplier.UpdateDate
                                }).Take(1).AsNoTracking();


                return supplierList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateSupplier";
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
                return supplierList;
            }

        }

        public async Task<IEnumerable<SupplierResponse>> UpdateSupplier(SupplierUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SupplierResponse>? supplierList = null;
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                var supplierUpdate = await _context.Supplier.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (supplierUpdate != null)
                {
                    supplierUpdate.CompanyId = param.CompanyId;
                    supplierUpdate.BranchId = param.BranchId;
                    supplierUpdate.Name = param.Name;
                    supplierUpdate.Address = param.Address;
                    supplierUpdate.Telp = param.Telp;
                    supplierUpdate.Fax = param.Fax;
                    supplierUpdate.Active = param.Active;
                    supplierUpdate.UpdateBy = param.UpdateBy;
                    supplierUpdate.UpdateDate = DateTime.Now;
                    _context.Supplier.Update(supplierUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    supplierList = (from supplier in _context.Supplier
                                    join branch in _context.Branch on supplier.BranchId equals branch.Id
                                    join company in _context.Company on supplier.CompanyId equals company.Id
                                    where supplier.Id == param.Id
                                    select new SupplierResponse
                                    {
                                        Id = supplier.Id,
                                        CompanyId = supplier.CompanyId,
                                        CompanyName = company.Name,
                                        BranchId = supplier.BranchId,
                                        BranchName = branch.Name,
                                        Name = supplier.Name,
                                        Address = supplier.Address,
                                        Telp = supplier.Telp,
                                        Fax = supplier.Fax,
                                        Active = supplier.Active,
                                        CreateBy = supplier.CreateBy,
                                        CreateDate = supplier.CreateDate,
                                        UpdateBy = supplier.UpdateBy,
                                        UpdateDate = supplier.UpdateDate
                                    }).Take(0).AsNoTracking();

                    return supplierList;
                }


                supplierList = (from supplier in _context.Supplier
                                join branch in _context.Branch on supplier.BranchId equals branch.Id
                                join company in _context.Company on supplier.CompanyId equals company.Id
                                where supplier.Id == param.Id
                                select new SupplierResponse
                                {
                                    Id = supplier.Id,
                                    CompanyId = supplier.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = supplier.BranchId,
                                    BranchName = branch.Name,
                                    Name = supplier.Name,
                                    Address = supplier.Address,
                                    Telp = supplier.Telp,
                                    Fax = supplier.Fax,
                                    Active = supplier.Active,
                                    CreateBy = supplier.CreateBy,
                                    CreateDate = supplier.CreateDate,
                                    UpdateBy = supplier.UpdateBy,
                                    UpdateDate = supplier.UpdateDate
                                }).Take(1).AsNoTracking();


                return supplierList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateSupplier";
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
                return supplierList;
            }

        }
    }
}
