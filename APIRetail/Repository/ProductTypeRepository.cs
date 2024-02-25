using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace APIRetail.Repository
{
    public class ProductTypeRepository : IProductType
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public ProductTypeRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<ProductTypeResponse>> GetProductType(ProductTypeRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProductTypeResponse>? prodTypeList = null;
            try
            {
                long Page = param.Page - 1;
                prodTypeList = (from prodType in _context.ProductType
                                join branch in _context.Branch on prodType.BranchId equals branch.Id
                                join company in _context.Company on prodType.CompanyId equals company.Id
                                where prodType.CompanyId == param.CompanyId && prodType.BranchId == param.BranchId
                                orderby prodType.ProductTypeName
                                select new ProductTypeResponse
                                {
                                    Id = prodType.Id,
                                    ProductTypeName = prodType.ProductTypeName,
                                    CompanyId = prodType.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = prodType.BranchId,
                                    BranchName = branch.Name,
                                    Active = prodType.Active,
                                    Description = prodType.Description,
                                    CreateBy = prodType.CreateBy,
                                    CreateDate = prodType.CreateDate,
                                    UpdateBy = prodType.UpdateBy,
                                    UpdateDate = prodType.UpdateDate
                                }).OrderBy(x => x.ProductTypeName).AsNoTracking();

                var TotalPageSize = Math.Ceiling((decimal)prodTypeList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = prodTypeList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetProductType";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                }
                else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return prodTypeList;
            }
        }

        public async Task<IEnumerable<ProductTypeResponse>> CreateProductType(ProductTypeAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProductTypeResponse>? prodTypeList = null;
            ProductType prodTypeAdd = new ProductType();
            try
            {
                prodTypeAdd.CompanyId = param.CompanyId;
                prodTypeAdd.BranchId = param.BranchId;
                prodTypeAdd.ProductTypeName = param.ProductTypeName;
                prodTypeAdd.Active = param.Active;
                prodTypeAdd.Description = param.Description;
                prodTypeAdd.CreateBy = param.CreateBy;
                prodTypeAdd.CreateDate = DateTime.Now;
                _context.ProductType.Add(prodTypeAdd);
                await _context.SaveChangesAsync();

                prodTypeList = (from prodType in _context.ProductType
                                join branch in _context.Branch on prodType.BranchId equals branch.Id
                                join company in _context.Company on prodType.CompanyId equals company.Id
                                where prodType.CompanyId == param.CompanyId && prodType.BranchId == param.BranchId
                                select new ProductTypeResponse
                                {
                                    Id = prodType.Id,
                                    CompanyId = prodType.Id,
                                    CompanyName = company.Name,
                                    BranchId = prodType.BranchId,
                                    BranchName = branch.Name,
                                    ProductTypeName = prodType.ProductTypeName,
                                    Description = prodType.Description,
                                    Active = prodType.Active,
                                    CreateBy = prodType.CreateBy,
                                    CreateDate = prodType.CreateDate,
                                    UpdateBy = prodType.UpdateBy,
                                    UpdateDate = prodType.UpdateDate
                                }).Take(1).AsNoTracking();


                return prodTypeList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateProductType";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return prodTypeList;
            }

        }

        public async Task<IEnumerable<ProductTypeResponse>> UpdateProductType(ProductTypeUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProductTypeResponse>? prodTypeList = null;

            try
            {
                var prodTypeUpdate = await _context.ProductType.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (prodTypeUpdate != null)
                {
                    prodTypeUpdate.CompanyId = param.CompanyId;
                    prodTypeUpdate.BranchId = param.BranchId;
                    prodTypeUpdate.ProductTypeName = param.ProductTypeName;
                    prodTypeUpdate.Active = param.Active;
                    prodTypeUpdate.Description = param.Description;
                    prodTypeUpdate.UpdateBy = param.UpdateBy;
                    prodTypeUpdate.UpdateDate = DateTime.Now;
                    _context.ProductType.Update(prodTypeUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {

                    prodTypeList = (from prodType in _context.ProductType
                                    join branch in _context.Branch on prodType.BranchId equals branch.Id
                                    join company in _context.Company on prodType.CompanyId equals company.Id
                                    where prodType.Id == param.Id
                                    select new ProductTypeResponse
                                    {
                                        Id = prodType.Id,
                                        CompanyId = prodType.Id,
                                        CompanyName = company.Name,
                                        BranchId = prodType.BranchId,
                                        BranchName = branch.Name,
                                        ProductTypeName = prodType.ProductTypeName,
                                        Description = prodType.Description,
                                        Active = prodType.Active,
                                        CreateBy = prodType.CreateBy,
                                        CreateDate = prodType.CreateDate,
                                        UpdateBy = prodType.UpdateBy,
                                        UpdateDate = prodType.UpdateDate
                                    }).Take(0).AsNoTracking();

                    return prodTypeList;
                }


                prodTypeList = (from prodType in _context.ProductType
                                join branch in _context.Branch on prodType.BranchId equals branch.Id
                                join company in _context.Company on prodType.CompanyId equals company.Id
                                where prodType.Id == param.Id
                                select new ProductTypeResponse
                                {
                                    Id = prodType.Id,
                                    CompanyId = prodType.Id,
                                    CompanyName = company.Name,
                                    BranchId = prodType.BranchId,
                                    BranchName = branch.Name,
                                    ProductTypeName = prodType.ProductTypeName,
                                    Description = prodType.Description,
                                    Active = prodType.Active,
                                    CreateBy = prodType.CreateBy,
                                    CreateDate = prodType.CreateDate,
                                    UpdateBy = prodType.UpdateBy,
                                    UpdateDate = prodType.UpdateDate
                                }).Take(1).AsNoTracking();


                return prodTypeList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateProductType";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return prodTypeList;
            }

        }
    }
}
