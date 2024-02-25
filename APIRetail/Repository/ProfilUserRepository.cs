using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class ProfilUserRepository : IProfilUser
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;

        public ProfilUserRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<ProfilUserResponse>> GetProfilUser(ProfilUserRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilUserResponse>? profilUserList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProfilId != null && param.ProfilId != 0)
                {
                    profilUserList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join company in _context.Company on user.CompanyId equals company.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      where proUser.ProfilId == param.ProfilId && user.CompanyId == param.CompanyId && user.BranchId == param.BranchId
                                      select new ProfilUserResponse
                                      {
                                          Id = proUser.Id,
                                          ProfilId = proUser.ProfilId,
                                          ProfilName = pro.Name,
                                          UserId = user.Id,
                                          UserName = user.UserName,
                                          CreateBy = user.CreateBy,
                                          CreateDate = user.CreateDate,
                                          UpdateBy = user.UpdateBy,
                                          UpdateDate = user.UpdateDate
                                      }).OrderBy(x => x.UserName).AsNoTracking();
                }

                if (param.ProfilId == null || param.ProfilId == 0)
                {
                    profilUserList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join company in _context.Company on user.CompanyId equals company.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId
                                      select new ProfilUserResponse
                                      {
                                          Id = proUser.Id,
                                          ProfilId = proUser.ProfilId,
                                          ProfilName = pro.Name,
                                          UserId = user.Id,
                                          UserName = user.UserName,
                                          CreateBy = user.CreateBy,
                                          CreateDate = user.CreateDate,
                                          UpdateBy = user.UpdateBy,
                                          UpdateDate = user.UpdateDate
                                      }).OrderBy(x => x.UserName).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)profilUserList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = profilUserList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetProfilUser";
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
                return profilUserList;
            }
        }

        public async Task<IEnumerable<ProfilUserResponse>> CreateProfilUser(ProfilUserAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilUserResponse>? profilUserList = null;
            ProfilUser profilUserAdd = new ProfilUser();
            try
            {
                profilUserAdd.ProfilId = param.ProfilId;
                profilUserAdd.UserId = param.UserId;
                profilUserAdd.CreateBy = param.CreateBy;
                profilUserAdd.CreateDate = DateTime.Now;
                _context.ProfilUser.Add(profilUserAdd);
                await _context.SaveChangesAsync();

                profilUserList = (from proUser in _context.ProfilUser
                                  join user in _context.Users on proUser.UserId equals user.Id
                                  join branch in _context.Branch on user.BranchId equals branch.Id
                                  join company in _context.Company on user.CompanyId equals company.Id
                                  join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                  where proUser.ProfilId == param.ProfilId && proUser.UserId == param.UserId
                                  select new ProfilUserResponse
                                  {
                                      Id = proUser.Id,
                                      ProfilId = proUser.ProfilId,
                                      ProfilName = pro.Name,
                                      UserId = user.Id,
                                      UserName = user.UserName,
                                      CreateBy = user.CreateBy,
                                      CreateDate = user.CreateDate,
                                      UpdateBy = user.UpdateBy,
                                      UpdateDate = user.UpdateDate
                                  }).Take(1).AsNoTracking();


                return profilUserList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateProfilUser";
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
                return profilUserList;
            }

        }

        public async Task<IEnumerable<ProfilUserResponse>> UpdateProfilUser(ProfilUserUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilUserResponse>? profilUserList = null;
            try
            {
                var profilUserUpdate = await _context.ProfilUser.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (profilUserUpdate != null)
                {
                    profilUserUpdate.ProfilId = param.ProfilId;
                    profilUserUpdate.UserId = param.UserId;
                    profilUserUpdate.UpdateBy = param.UpdateBy;
                    profilUserUpdate.UpdateDate = DateTime.Now;
                    _context.ProfilUser.Update(profilUserUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    profilUserList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join company in _context.Company on user.CompanyId equals company.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      where proUser.Id == param.Id
                                      select new ProfilUserResponse
                                      {
                                          Id = proUser.Id,
                                          ProfilId = proUser.ProfilId,
                                          ProfilName = pro.Name,
                                          UserId = user.Id,
                                          UserName = user.UserName,
                                          CreateBy = user.CreateBy,
                                          CreateDate = user.CreateDate,
                                          UpdateBy = user.UpdateBy,
                                          UpdateDate = user.UpdateDate
                                      }).Take(0).AsNoTracking();

                    return profilUserList;
                }


                profilUserList = (from proUser in _context.ProfilUser
                                  join user in _context.Users on proUser.UserId equals user.Id
                                  join branch in _context.Branch on user.BranchId equals branch.Id
                                  join company in _context.Company on user.CompanyId equals company.Id
                                  join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                  where proUser.Id == param.Id
                                  select new ProfilUserResponse
                                  {
                                      Id = proUser.Id,
                                      ProfilId = proUser.ProfilId,
                                      ProfilName = pro.Name,
                                      UserId = user.Id,
                                      UserName = user.UserName,
                                      CreateBy = user.CreateBy,
                                      CreateDate = user.CreateDate,
                                      UpdateBy = user.UpdateBy,
                                      UpdateDate = user.UpdateDate
                                  }).Take(1).AsNoTracking();


                return profilUserList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateProfilUser";
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
                return profilUserList;
            }

        }
    }
}
