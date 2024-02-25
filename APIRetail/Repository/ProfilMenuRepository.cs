using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class ProfilMenuRepository : IProfilMenu
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public ProfilMenuRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<ProfilMenuResponse>> GetProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilMenuResponse>? profilMenuList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.ProfilId != null && param.ProfilId != 0 && param.ParentMenuId != null && param.ParentMenuId != 0)
                {
                    profilMenuList = (from proMenu in _context.ProfilMenu
                                      join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                      where proMenu.ProfilId == param.ProfilId && proMenu.ParentMenuId == param.ParentMenuId
                                      select new ProfilMenuResponse
                                      {
                                          Id = proMenu.Id,
                                          ProfilId = proMenu.ProfilId,
                                          ProfilName = pro.Name,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          MenuId = menu.Id,
                                          MenuName = menu.Name,
                                          CreateBy = proMenu.CreateBy,
                                          CreateDate = proMenu.CreateDate,
                                          UpdateBy = proMenu.UpdateBy,
                                          UpdateDate = proMenu.UpdateDate
                                      }).OrderBy(x => x.ProfilName).AsNoTracking();
                }


                else if (param.ProfilId != null && param.ProfilId != 0 && (param.ParentMenuId == null || param.ParentMenuId == 0))
                {
                    profilMenuList = (from proMenu in _context.ProfilMenu
                                      join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                      where proMenu.ProfilId == param.ProfilId
                                      select new ProfilMenuResponse
                                      {
                                          Id = proMenu.Id,
                                          ProfilId = proMenu.ProfilId,
                                          ProfilName = pro.Name,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          MenuId = menu.Id,
                                          MenuName = menu.Name,
                                          CreateBy = proMenu.CreateBy,
                                          CreateDate = proMenu.CreateDate,
                                          UpdateBy = proMenu.UpdateBy,
                                          UpdateDate = proMenu.UpdateDate
                                      }).OrderBy(x => x.ProfilName).AsNoTracking();
                }

                else
                {
                    profilMenuList = (from proMenu in _context.ProfilMenu
                                      join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                      select new ProfilMenuResponse
                                      {
                                          Id = proMenu.Id,
                                          ProfilId = proMenu.ProfilId,
                                          ProfilName = pro.Name,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          MenuId = menu.Id,
                                          MenuName = menu.Name,
                                          CreateBy = proMenu.CreateBy,
                                          CreateDate = proMenu.CreateDate,
                                          UpdateBy = proMenu.UpdateBy,
                                          UpdateDate = proMenu.UpdateDate
                                      }).OrderBy(x => x.ProfilName).AsNoTracking();
                }

                var TotalPageSize = Math.Ceiling((decimal)profilMenuList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = profilMenuList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetProfilMenu";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return profilMenuList;
            }
        }

        public async Task<IEnumerable<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilMenuResponse>? profilMenuList = null;
            ProfilMenu profilMenuAdd = new ProfilMenu();
            try
            {
                profilMenuAdd.ProfilId = param.ProfilId;
                profilMenuAdd.ParentMenuId = param.ParentMenuId;
                profilMenuAdd.MenuId = param.MenuId;
                profilMenuAdd.CreateBy = param.CreateBy;
                profilMenuAdd.CreateDate = DateTime.Now;
                _context.ProfilMenu.Add(profilMenuAdd);
                await _context.SaveChangesAsync();

                profilMenuList = (from proMenu in _context.ProfilMenu
                                  join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                  join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                  join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                  where proMenu.ProfilId == param.ProfilId && proMenu.ParentMenuId == param.ParentMenuId && proMenu.MenuId == param.MenuId
                                  select new ProfilMenuResponse
                                  {
                                      Id = proMenu.Id,
                                      ProfilId = proMenu.ProfilId,
                                      ProfilName = pro.Name,
                                      ParentMenuId = proMenu.ParentMenuId,
                                      ParentMenuName = menuParent.Name,
                                      MenuId = menu.Id,
                                      MenuName = menu.Name,
                                      CreateBy = proMenu.CreateBy,
                                      CreateDate = proMenu.CreateDate,
                                      UpdateBy = proMenu.UpdateBy,
                                      UpdateDate = proMenu.UpdateDate
                                  }).Take(1).AsNoTracking();


                return profilMenuList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateProfilMenu";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return profilMenuList;
            }

        }

        public async Task<IEnumerable<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilMenuResponse>? profilMenuList = null;
            try
            {
                var profilMenuUpdate = await _context.ProfilMenu.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (profilMenuUpdate != null)
                {
                    profilMenuUpdate.ProfilId = param.ProfilId;
                    profilMenuUpdate.ParentMenuId = param.ParentMenuId;
                    profilMenuUpdate.MenuId = param.MenuId;
                    profilMenuUpdate.UpdateBy = param.UpdateBy;
                    profilMenuUpdate.UpdateDate = DateTime.Now;
                    _context.ProfilMenu.Update(profilMenuUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    profilMenuList = (from proMenu in _context.ProfilMenu
                                      join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                      where proMenu.Id == param.Id
                                      select new ProfilMenuResponse
                                      {
                                          Id = proMenu.Id,
                                          ProfilId = proMenu.ProfilId,
                                          ProfilName = pro.Name,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          MenuId = menu.Id,
                                          MenuName = menu.Name,
                                          CreateBy = proMenu.CreateBy,
                                          CreateDate = proMenu.CreateDate,
                                          UpdateBy = proMenu.UpdateBy,
                                          UpdateDate = proMenu.UpdateDate
                                      }).Take(0).AsNoTracking();

                    return profilMenuList;
                }


                profilMenuList = (from proMenu in _context.ProfilMenu
                                  join pro in _context.Profil on proMenu.ProfilId equals pro.Id
                                  join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                  join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                  where proMenu.Id == param.Id
                                  select new ProfilMenuResponse
                                  {
                                      Id = proMenu.Id,
                                      ProfilId = proMenu.ProfilId,
                                      ProfilName = pro.Name,
                                      ParentMenuId = proMenu.ParentMenuId,
                                      ParentMenuName = menuParent.Name,
                                      MenuId = menu.Id,
                                      MenuName = menu.Name,
                                      CreateBy = proMenu.CreateBy,
                                      CreateDate = proMenu.CreateDate,
                                      UpdateBy = proMenu.UpdateBy,
                                      UpdateDate = proMenu.UpdateDate
                                  }).Take(1).AsNoTracking();


                return profilMenuList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateProfilMenu";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return profilMenuList;
            }

        }
    }
}
