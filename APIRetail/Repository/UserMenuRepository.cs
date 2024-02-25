using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class UserMenuRepository : IUserMenu
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public UserMenuRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<UserMenuParentResponse>> GetUserMenuParent(UserMenuParentRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<UserMenuParentResponse>? userMenuparentList = null;
            try
            {

                userMenuparentList = (from proUser in _context.ProfilUser
                                      join user in _context.Users on proUser.UserId equals user.Id
                                      join branch in _context.Branch on user.BranchId equals branch.Id
                                      join company in _context.Company on user.CompanyId equals company.Id
                                      join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                      join proMenu in _context.ProfilMenu on proUser.ProfilId equals proMenu.ProfilId
                                      join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                      where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId && user.UserName == param.UserName && menuParent.IsHeader == 1
                                      select new UserMenuParentResponse
                                      {
                                          CompanyId = user.CompanyId,
                                          CompanyName = company.Name,
                                          BranchId = user.BranchId,
                                          BranchName = branch.Name,
                                          ProfilId = pro.Id,
                                          ProfilName = pro.Name,
                                          UserId = user.Id,
                                          UserName = user.UserName,
                                          ParentMenuId = proMenu.ParentMenuId,
                                          ParentMenuName = menuParent.Name,
                                          Sort = menuParent.Sort
                                      }).OrderBy(x => x.Sort).Distinct().AsNoTracking();

                return userMenuparentList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetUserMenuParent";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
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
                                join company in _context.Company on user.CompanyId equals company.Id
                                join pro in _context.Profil on proUser.ProfilId equals pro.Id
                                join proMenu in _context.ProfilMenu on proUser.ProfilId equals proMenu.ProfilId
                                join menuParent in _context.Menu on proMenu.ParentMenuId equals menuParent.Id
                                join menu in _context.Menu on proMenu.MenuId equals menu.Id
                                where user.CompanyId == param.CompanyId && user.BranchId == param.BranchId && user.UserName == param.UserName
                                    && menuParent.IsHeader == 1 && menu.IsHeader != 1
                                select new UserMenuResponse
                                {
                                    CompanyId = user.CompanyId,
                                    CompanyName = company.Name,
                                    BranchId = user.BranchId,
                                    BranchName = branch.Name,
                                    ProfilId = pro.Id,
                                    ProfilName = pro.Name,
                                    UserId = user.Id,
                                    UserName = user.UserName,
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
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetUserMenu";
                if (ex.InnerException != null) {
                    logDataError.ErrorDeskripsi = "Error : " + ex.InnerException.Message + ", Source : " + ex.StackTrace;
                } else {
                    logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                }
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
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
                                join company in _context.Company on user.CompanyId equals company.Id
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
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetCheckUserMenu";
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

                return userMenuList;
            }
        }
    }
}
