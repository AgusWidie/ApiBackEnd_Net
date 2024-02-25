using APIRetail.Crypto;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class UserRepository : IUser
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public UserRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }
        public async Task<IEnumerable<UsersResponse>> GetUser(UsersRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.UserName == null || param.UserName == "")
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join company in _context.Company on user.CompanyId equals company.Id
                                where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    CompanyId = user.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = user.BranchId,
                                    BranchName = branch.Name,
                                    UserName = user.UserName,
                                    Name = user.Name,
                                    PasswordExpired = user.PasswordExpired,
                                    Description = user.Description,
                                    Active = user.Active,
                                    CreateBy = user.CreateBy,
                                    CreateDate = user.CreateDate,
                                    UpdateBy = user.UpdateBy,
                                    UpdateDate = user.UpdateDate
                                }).OrderBy(x => x.UserName).AsNoTracking();
                }

                if (param.UserName != null || param.UserName != "")
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join company in _context.Company on user.CompanyId equals company.Id
                                where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId && user.UserName == param.UserName
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    CompanyId = user.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = user.BranchId,
                                    BranchName = branch.Name,
                                    UserName = user.UserName,
                                    Name = user.Name,
                                    PasswordExpired = user.PasswordExpired,
                                    Description = user.Description,
                                    Active = user.Active,
                                    CreateBy = user.CreateBy,
                                    CreateDate = user.CreateDate,
                                    UpdateBy = user.UpdateBy,
                                    UpdateDate = user.UpdateDate
                                }).OrderBy(x => x.UserName).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)userList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = userList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetUser";
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
                return userList;
            }
        }

        public async Task<IEnumerable<UsersResponse>> CreateUser(UsersAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            Users UsersAdd = new Users();
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                UsersAdd.CompanyId = param.CompanyId;
                UsersAdd.BranchId = param.BranchId;
                UsersAdd.UserName = param.UserName;
                UsersAdd.Name = param.Name;
                UsersAdd.Password = encryptMD5.MD5Encryption(param.Password);
                UsersAdd.PasswordExpired = DateTime.Now.AddDays(90);
                UsersAdd.Description = param.Description;
                UsersAdd.Active = param.Active;
                UsersAdd.CreateBy = param.CreateBy;
                UsersAdd.CreateDate = DateTime.Now;
                _context.Users.Add(UsersAdd);
                await _context.SaveChangesAsync();

                userList = (from user in _context.Users
                            join branch in _context.Branch on user.BranchId equals branch.Id
                            join company in _context.Company on user.CompanyId equals company.Id
                            where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId && user.UserName == param.UserName
                            select new UsersResponse
                            {
                                Id = user.Id,
                                CompanyId = user.CompanyId,
                                CompanyName = company.Name,
                                BranchId = user.BranchId,
                                BranchName = branch.Name,
                                UserName = user.UserName,
                                Name = user.Name,
                                PasswordExpired = user.PasswordExpired,
                                Description = user.Description,
                                Active = user.Active,
                                CreateBy = user.CreateBy,
                                CreateDate = user.CreateDate,
                                UpdateBy = user.UpdateBy,
                                UpdateDate = user.UpdateDate
                            }).Take(1).AsNoTracking();


                return userList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateUser";
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
                return userList;
            }

        }

        public async Task<IEnumerable<UsersResponse>> UpdateUser(UsersUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            try
            {
                var usersUpdate = await _context.Users.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (usersUpdate != null)
                {
                    usersUpdate.CompanyId = param.CompanyId;
                    usersUpdate.BranchId = param.BranchId;
                    usersUpdate.UserName = param.UserName;
                    usersUpdate.Name = param.Name;
                    usersUpdate.Description = param.Description;
                    usersUpdate.Active = param.Active;
                    usersUpdate.UpdateBy = param.UpdateBy;
                    usersUpdate.UpdateDate = DateTime.Now;
                    _context.Users.Update(usersUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join company in _context.Company on user.CompanyId equals company.Id
                                where user.Id == param.Id
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    CompanyId = user.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = user.BranchId,
                                    BranchName = branch.Name,
                                    UserName = user.UserName,
                                    Name = user.Name,
                                    PasswordExpired = user.PasswordExpired,
                                    Description = user.Description,
                                    Active = user.Active,
                                    CreateBy = user.CreateBy,
                                    CreateDate = user.CreateDate,
                                    UpdateBy = user.UpdateBy,
                                    UpdateDate = user.UpdateDate
                                }).Take(0).AsNoTracking();

                    return userList;
                }


                userList = (from user in _context.Users
                            join branch in _context.Branch on user.BranchId equals branch.Id
                            join company in _context.Company on user.CompanyId equals company.Id
                            where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId && user.UserName == param.UserName
                            select new UsersResponse
                            {
                                Id = user.Id,
                                CompanyId = user.CompanyId,
                                CompanyName = company.Name,
                                BranchId = user.BranchId,
                                BranchName = branch.Name,
                                UserName = user.UserName,
                                Name = user.Name,
                                PasswordExpired = user.PasswordExpired,
                                Description = user.Description,
                                Active = user.Active,
                                CreateBy = user.CreateBy,
                                CreateDate = user.CreateDate,
                                UpdateBy = user.UpdateBy,
                                UpdateDate = user.UpdateDate
                            }).Take(1).AsNoTracking();


                return userList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateUser";
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
                return userList;
            }

        }
    }
}
