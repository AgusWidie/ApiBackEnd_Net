using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class BranchRepository : IBranch
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public BranchRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<BranchResponse>> GetBranch(BranchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<BranchResponse>? branchList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.CompanyId != null && (param.Name == null || param.Name == ""))
                {
                    branchList = (from branch in _context.Branch
                                  join company in _context.Company on branch.CompanyId equals company.Id
                                  where branch.CompanyId == param.CompanyId
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      CompanyId = company.Id,
                                      CompanyName = company.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking();
                }

                else if (param.CompanyId != null && (param.Name != null || param.Name != ""))
                {
                    branchList = (from branch in _context.Branch
                                  join company in _context.Company on branch.CompanyId equals company.Id
                                  where branch.CompanyId == param.CompanyId && branch.Name.Contains(param.Name)
                                  orderby branch.Name
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      CompanyId = company.Id,
                                      CompanyName = company.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking();
                }

                else
                {
                    branchList = (from branch in _context.Branch
                                  join company in _context.Company on branch.CompanyId equals company.Id
                                  orderby branch.Name
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      CompanyId = company.Id,
                                      CompanyName = company.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)branchList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = branchList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetBranch";
                if(ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return branchList;
            }
        }


        public async Task<IEnumerable<BranchResponse>> CreateBranch(BranchAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<BranchResponse>? branchList = null;
            Branch branchAdd = new Branch();
            try
            {
                branchAdd.CompanyId = param.CompanyId;
                branchAdd.Name = param.Name;
                branchAdd.Address = param.Address;
                branchAdd.Telp = param.Telp;
                branchAdd.Fax = param.Fax;
                branchAdd.CreateBy = param.CreateBy;
                branchAdd.CreateDate = DateTime.Now;
                _context.Branch.Add(branchAdd);
                await _context.SaveChangesAsync();

                branchList = (from branch in _context.Branch
                              join company in _context.Company on branch.CompanyId equals company.Id
                              where branch.CompanyId == param.CompanyId && branch.Name == param.Name
                              select new BranchResponse
                              {
                                  Id = branch.Id,
                                  Name = branch.Name,
                                  CompanyId = company.Id,
                                  CompanyName = company.Name,
                                  Address = branch.Address,
                                  Telp = branch.Telp,
                                  Fax = branch.Fax,
                                  CreateBy = branch.CreateBy,
                                  CreateDate = branch.CreateDate,
                                  UpdateBy = branch.UpdateBy,
                                  UpdateDate = branch.UpdateDate
                              }).Take(1).AsNoTracking();


                return branchList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateBranch";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return branchList;
            }

        }

        public async Task<IEnumerable<BranchResponse>> UpdateBranch(BranchUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<BranchResponse>? branchList = null;
            try
            {
                var branchUpdate = await _context.Branch.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (branchUpdate != null)
                {
                    branchUpdate.CompanyId = param.CompanyId;
                    branchUpdate.Name = param.Name;
                    branchUpdate.Address = param.Address;
                    branchUpdate.Telp = param.Telp;
                    branchUpdate.Fax = param.Fax;
                    branchUpdate.UpdateBy = param.UpdateBy;
                    branchUpdate.UpdateDate = DateTime.Now;
                    _context.Branch.Update(branchUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    branchList = (from branch in _context.Branch
                                  join company in _context.Company on branch.CompanyId equals company.Id
                                  where branch.Id == param.Id
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      CompanyId = company.Id,
                                      CompanyName = company.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).Take(0).AsNoTracking();

                    return branchList;
                }


                branchList = (from branch in _context.Branch
                              join company in _context.Company on branch.CompanyId equals company.Id
                              where branch.Id == param.Id
                              select new BranchResponse
                              {
                                  Id = branch.Id,
                                  Name = branch.Name,
                                  CompanyId = company.Id,
                                  CompanyName = company.Name,
                                  Address = branch.Address,
                                  Telp = branch.Telp,
                                  Fax = branch.Fax,
                                  CreateBy = branch.CreateBy,
                                  CreateDate = branch.CreateDate,
                                  UpdateBy = branch.UpdateBy,
                                  UpdateDate = branch.UpdateDate
                              }).Take(1).AsNoTracking();


                return branchList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateBranch";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return branchList;
            }

        }
    }
}
