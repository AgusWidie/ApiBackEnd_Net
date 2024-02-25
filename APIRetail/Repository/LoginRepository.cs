using APIRetail.Crypto;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIRetail.Repository
{
    public class LoginRepository : ILogin
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public LoginRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<LoginResponse>> LoginUser(LoginRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LoginResponse>? loginList = null;
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                var checkUser = await _context.Users.Where(x => x.UserName == param.UserName && x.Password == encryptMD5.MD5Encryption(param.Password)).FirstOrDefaultAsync();
                if (checkUser == null)
                {

                    loginList = (from user in _context.Users
                                 join branch in _context.Branch on user.BranchId equals branch.Id
                                 join company in _context.Company on user.CompanyId equals company.Id
                                 where user.UserName == param.UserName
                                 select new LoginResponse
                                 {
                                     CompanyId = 0,
                                     BranchId = 0,
                                     UserName = "",
                                     Token = "",
                                     TokenExpired = "",
                                     Email = "",
                                     Message = "User tidak ada atau password salah."
                                 }).Take(1).AsNoTracking();

                    return loginList;

                }
                else
                {
                    if (checkUser.Active == 0)
                    {
                        loginList = (from user in _context.Users
                                     join branch in _context.Branch on user.BranchId equals branch.Id
                                     join company in _context.Company on user.CompanyId equals company.Id
                                     where user.UserName == param.UserName
                                     select new LoginResponse
                                     {
                                         CompanyId = 0,
                                         BranchId = 0,
                                         UserName = "",
                                         Token = "",
                                         TokenExpired = "",
                                         Email = "",
                                         Message = "User tidak aktif."
                                     }).Take(1).AsNoTracking();

                        return loginList;
                    }

                    if (checkUser.PasswordExpired < DateTime.Now)
                    {
                        loginList = (from user in _context.Users
                                     join branch in _context.Branch on user.BranchId equals branch.Id
                                     join company in _context.Company on user.CompanyId equals company.Id
                                     where user.UserName == param.UserName
                                     select new LoginResponse
                                     {
                                         CompanyId = 0,
                                         BranchId = 0,
                                         UserName = "",
                                         Token = "",
                                         TokenExpired = "",
                                         Email = "",
                                         Message = "Password user sudah expired."
                                     }).Take(1).AsNoTracking();

                        return loginList;
                    }

                    var token = GenerateTokenUser(checkUser.UserName, checkUser.Name, checkUser.Email, cancellationToken);
                    var checkUserToken = await _context.UserToken.Where(x => x.UserId == checkUser.Id).FirstOrDefaultAsync();
                    if (checkUserToken == null)
                    {

                        UserToken userToken = new UserToken();
                        userToken.UserId = checkUser.Id;
                        userToken.Token = token;
                        userToken.TokenExpired = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration.GetValue<string>("Jwt:ExpiredJwt")));
                        userToken.CreateBy = param.UserName;
                        userToken.CreateDate = DateTime.Now;
                        _context.UserToken.Add(userToken);
                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        checkUserToken.Token = token;
                        checkUserToken.TokenExpired = DateTime.UtcNow.AddHours(Convert.ToInt32(_configuration.GetValue<string>("Jwt:ExpiredJwt")));
                        checkUserToken.UpdateBy = param.UserName;
                        checkUserToken.UpdateDate = DateTime.Now;
                        _context.UserToken.Update(checkUserToken);
                        await _context.SaveChangesAsync();
                    }

                    loginList = (from user in _context.Users
                                 join branch in _context.Branch on user.BranchId equals branch.Id
                                 join company in _context.Company on user.CompanyId equals company.Id
                                 where user.UserName == param.UserName
                                 select new LoginResponse
                                 {
                                     CompanyId = company.Id,
                                     BranchId = branch.Id,
                                     UserName = user.UserName,
                                     Token = token,
                                     TokenExpired = DateTime.Now.AddHours(Convert.ToInt32(_configuration.GetValue<string>("Jwt:ExpiredJwt"))).ToString(),
                                     Email = user.Email,
                                     Message = "User berhasil login."
                                 }).Take(1).AsNoTracking();

                }

                return loginList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "LoginUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return loginList;
            }

        }

        public async Task<IEnumerable<UsersResponse>> ChangePasswordUser(ChangePasswordRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UsersResponse>? userList = null;
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                var usersUpdate = await _context.Users.Where(x => x.UserName == param.UserName && x.Password == encryptMD5.MD5Encryption(param.Password) && x.Active == 1).FirstOrDefaultAsync();
                if (usersUpdate == null)
                {
                    userList = (from user in _context.Users
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join company in _context.Company on user.CompanyId equals company.Id
                                where user.UserName == param.UserName
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
                                    CreateBy = branch.CreateBy,
                                    CreateDate = branch.CreateDate,
                                    UpdateBy = branch.UpdateBy,
                                    UpdateDate = branch.UpdateDate
                                }).Take(0).AsNoTracking();

                    return userList;

                }
                else
                {
                    usersUpdate.Password = encryptMD5.MD5Encryption(param.PasswordNew);
                    usersUpdate.PasswordExpired = DateTime.Now.AddDays(90);
                    usersUpdate.UpdateBy = param.UserName;
                    usersUpdate.UpdateDate = DateTime.Now;
                    _context.Users.Update(usersUpdate);
                    await _context.SaveChangesAsync();

                }

                userList = (from user in _context.Users
                            join branch in _context.Branch on user.BranchId equals branch.Id
                            join company in _context.Company on user.CompanyId equals company.Id
                            where user.UserName == param.UserName
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
                                CreateBy = branch.CreateBy,
                                CreateDate = branch.CreateDate,
                                UpdateBy = branch.UpdateBy,
                                UpdateDate = branch.UpdateDate
                            }).Take(1).AsNoTracking(); ;


                return userList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "ChangePasswordUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return userList;
            }

        }


        public string GenerateTokenUser(string UserName, string Name, string Email, CancellationToken cancellationToken)
        {
            string TokenUser = "";
            try
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Jwt:Key"));
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserName", UserName),
                        new Claim("Name", Name),
                        new Claim("Email", Email)
                    }),
                    Issuer = _configuration.GetValue<string>("Jwt:Issuer"),
                    Audience = _configuration.GetValue<string>("Jwt:Audience"),
                    Expires = DateTime.Now.AddHours(Convert.ToInt32(_configuration.GetValue<string>("Jwt:ExpiredJwt"))),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return tokenString;

            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GenerateTokenUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return TokenUser;
            }

        }
    }
}
