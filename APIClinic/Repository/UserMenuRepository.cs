using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class UserMenuRepository : IUserMenu
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public UserMenuRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<UserMenuParentResponse>> GetUserMenuParent(UserMenuParentRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UserMenuParentResponse>? userMenuparentList = null;
            try
            {
                userMenuparentList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      join proMenu in _context.ProfilMenu on proUser.ProfilId equals proMenu.ProfilId
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId && user.UserName == param.UserName && menuParent.IsHeader == 1
                                      select new UserMenuParentResponse
                                      {
                                          ClinicId = user.ClinicId,
                                          BranchId = user.BranchId,
                                          ProfilId = pro.Id,
                                          ProfilName = pro.Name,
                                          UserId = user.Id,
                                          UserName = user.Name,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          Sort = menuParent.Sort
                                      }).OrderBy(x => x.Sort).Distinct().AsNoTracking();

                return userMenuparentList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetUserMenuParent";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return userMenuparentList;
            }
        }

        public async Task<IEnumerable<UserMenuResponse>> GetUserMenu(UserMenuRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UserMenuResponse>? userMenuList = null;
            try
            {
                userMenuList = (from proUser in _context.ProfilUser
                                join user in _context.Users on proUser.UserId equals user.Id
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                join proMenu in _context.ProfilMenu on proUser.ProfilId equals proMenu.ProfilId
                                join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                where user.ClinicId == param.ClinicId && user.BranchId == param.BranchId && user.UserName == param.UserName
                                    && menuParent.IsHeader == 1 && menu.IsHeader != 1
                                select new UserMenuResponse
                                {
                                    ClinicId = user.ClinicId,
                                    BranchId = user.BranchId,
                                    ProfilId = pro.Id,
                                    ProfilName = pro.Name,
                                    UserId = user.Id,
                                    UserName = user.Name,
                                    ParentMenuId = menuParent.Id,
                                    ParentMenuName = menuParent.Name,
                                    MenuId = menu.Id,
                                    MenuName = menu.Name,
                                    Sort = menu.Sort
                                }).OrderBy(x => x.Sort).Distinct().AsNoTracking();

                return userMenuList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetUserMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return userMenuList;
            }
        }

        public async Task<IEnumerable<CheckUserMenuResponse>> GetCheckUserMenu(CheckUserMenuRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<CheckUserMenuResponse>? userMenuList = null;
            try
            {
                userMenuList = (from proUser in _context.ProfilUser
                                join user in _context.Users on proUser.UserId equals user.Id
                                join branch in _context.Branch on user.BranchId equals branch.Id
                                join clinic in _context.Clinic on user.ClinicId equals clinic.Id
                                join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                join proMenu in _context.ProfilMenu on proUser.ProfilId equals proMenu.ProfilId
                                join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                where user.Id == param.UserId && pro.Id == param.ProfilId && menu.ControllerName == param.ControllerName
                                select new CheckUserMenuResponse
                                {
                                    ProfilId = pro.Id,
                                    UserId = user.Id,
                                    ControllerName = menu.ControllerName,
                                    ParentMenuId = menuParent.Id,
                                    ParentMenuName = menuParent.Name,
                                    MenuId = menu.Id,
                                    MenuName = menu.Name,
                                }).Take(1).AsNoTracking();

                return userMenuList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetCheckUserMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return userMenuList;
            }
        }
    }
}
