using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ProfilUserRepository : IProfilUser
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public ProfilUserRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ProfilUserResponse>> GetProfilUser(ProfilUserSearchRequest param, CancellationToken cancellationToken)
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
                                      join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      where proUser.ProfilId == param.ProfilId && user.ClinicId == param.ClinicId && user.BranchId == param.BranchId
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
                                      }).OrderBy(x => x.UserName).AsNoTracking().ToList();
                }

                if (param.ProfilId == null || param.ProfilId == 0)
                {
                    profilUserList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId
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
                                      }).OrderBy(x => x.UserName).AsNoTracking().ToList();
                }

                var TotalPageSize = Math.Ceiling((decimal)profilUserList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = profilUserList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetProfilUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilUserList;
            }
        }

        public async Task<IEnumerable<ProfilUserResponse>> CreateProfilUser(ProfilUserRequest param, CancellationToken cancellationToken)
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
                                  join company in _context.Clinic on user.ClinicId equals company.Id
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateProfilUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilUserList;
            }

        }

        public async Task<IEnumerable<ProfilUserResponse>> UpdateProfilUser(ProfilUserRequest param, CancellationToken cancellationToken)
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
                                      join clinic in _context.Clinic on user.ClinicId equals clinic.Id
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
                                  join company in _context.Clinic on user.ClinicId equals company.Id
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateProfilUser";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilUserList;
            }

        }
    }
}
