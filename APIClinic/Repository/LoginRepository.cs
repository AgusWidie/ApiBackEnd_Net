using APIClinic.Crypto;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace APIClinic.Repository
{
    public class LoginRepository : ILogin
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public LoginRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<LoginResponse>> LoginUser(LoginRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LoginResponse>? loginList = null;
            EncryptMD5 encryptMD5 = new EncryptMD5();
            try
            {
                var checkUser = await _context.Users.Where(x => x.UserName == param.UserName && x.Password == encryptMD5.MD5Encryption(param.Password)).AsNoTracking().FirstOrDefaultAsync();
                if (checkUser == null)
                {

                    loginList = (from user in _context.Users
                                 join branch in _context.Branch on user.BranchId equals branch.Id
                                 join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                 where user.UserName == param.UserName
                                 select new LoginResponse
                                 {
                                     ClinicId = 0,
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
                                     join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                     where user.UserName == param.UserName
                                     select new LoginResponse
                                     {
                                         ClinicId = 0,
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
                                     join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                     where user.UserName == param.UserName
                                     select new LoginResponse
                                     {
                                         ClinicId = 0,
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
                    var checkUserToken = await _context.UserToken.Where(x => x.UserId == checkUser.Id).AsNoTracking().FirstOrDefaultAsync();
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
                                 join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                 where user.UserName == param.UserName
                                 select new LoginResponse
                                 {
                                     ClinicId = clinic.Id,
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
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                where user.UserName == param.UserName
                                select new UsersResponse
                                {
                                    ClinicId = user.ClinicId,
                                    ClinicName = clinic.Name,
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
                                CreateBy = branch.CreateBy,
                                CreateDate = branch.CreateDate,
                                UpdateBy = branch.UpdateBy,
                                UpdateDate = branch.UpdateDate
                            }).Take(1).AsNoTracking(); ;


                return userList;
            }
            catch (Exception ex)
            {
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
                return TokenUser;
            }

        }
    }
}
