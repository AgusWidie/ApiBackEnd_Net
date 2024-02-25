using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ProfilMenuRepository : IProfilMenu
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public ProfilMenuRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ProfilMenuResponse>> GetProfilMenu(ProfilMenuSearchRequest param, CancellationToken cancellationToken)
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
                                      }).OrderBy(x => x.ProfilName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
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
                                      }).OrderBy(x => x.ProfilName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
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
                                      }).OrderBy(x => x.ProfilName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return profilMenuList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetProfilMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilMenuList;
            }
        }

        public async Task<IEnumerable<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken)
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateProfilMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilMenuList;
            }

        }

        public async Task<IEnumerable<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken)
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateProfilMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilMenuList;
            }

        }
    }
}
