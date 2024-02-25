using APIClinic.Crypto;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class UserRepository : IUsers
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public UserRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<UsersResponse>> GetUser(UserSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.UserName == null || param.UserName == "")
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    ClinicId = user.ClinicId,
                                    ClinicName = clinic.Name,
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
                                }).OrderBy(x => x.UserName).AsNoTracking().ToList();
                }

                if (param.UserName != null || param.UserName != "")
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId && user.UserName == param.UserName
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    ClinicId = user.ClinicId,
                                    ClinicName = clinic.Name,
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
                                }).OrderBy(x => x.UserName).AsNoTracking().ToList();
                }

                var TotalPageSize = Math.Ceiling((decimal)userList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = userList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return userList;
            }
        }

        public async Task<IEnumerable<UsersResponse>> CreateUser(UserRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            Users UsersAdd = new Users();
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                var checkUser = await _context.Users.Where(x => x.UserName == param.UserName).AsNoTracking().FirstOrDefaultAsync();
                if (checkUser != null)
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                where user.UserName == param.UserName
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    ClinicId = user.ClinicId,
                                    ClinicName = clinic.Name,
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

                UsersAdd.ClinicId = param.ClinicId;
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
                            join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                            where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId && user.UserName == param.UserName
                            select new UsersResponse
                            {
                                Id = user.Id,
                                ClinicId = user.ClinicId,
                                ClinicName = clinic.Name,
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return userList;
            }

        }

        public async Task<IEnumerable<UsersResponse>> UpdateUser(UserRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            try
            {
                var usersUpdate = await _context.Users.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (usersUpdate != null)
                {
                    usersUpdate.ClinicId = param.ClinicId;
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
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                where user.Id == param.Id
                                select new UsersResponse
                                {
                                    Id = user.Id,
                                    ClinicId = user.ClinicId,
                                    ClinicName = clinic.Name,
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
                            join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                            where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId && user.UserName == param.UserName
                            select new UsersResponse
                            {
                                Id = user.Id,
                                ClinicId = user.ClinicId,
                                ClinicName = clinic.Name,
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return userList;
            }

        }
    }
}
