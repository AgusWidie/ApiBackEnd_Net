using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Request.LogError;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIRetail.Repository
{
    public class MenuRepository : IMenu
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public MenuRepository(IConfiguration Configuration, retail_systemContext context, ILogError logError)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
        }

        public async Task<IEnumerable<MenuResponse>> GetMenu(MenuRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MenuResponse>? menuList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Name == null || param.Name == "")
                {
                    menuList = (from menu in _context.Menu
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.ControllerName,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Active = menu.Active,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.Name != null || param.Name != "")
                {
                    menuList = (from menu in _context.Menu
                                where menu.Name.Contains(param.Name)
                                orderby menu.Sort
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.ControllerName,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Active = menu.Active,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return menuList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "GetMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return menuList;
            }
        }

        public async Task<IEnumerable<MenuResponse>> CreateMenu(MenuAddRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MenuResponse>? menuList = null;
            Menu menuAdd = new Menu();
            try
            {
                menuList = (from menu in _context.Menu
                            where menu.ControllerName == param.ControllerName && menu.ActionName == param.ActionName
                                  && menu.IsHeader == param.IsHeader && menu.Sort == param.Sort
                            select new MenuResponse
                            {
                                Id = menu.Id,
                                Name = menu.Name,
                                ControllerName = menu.ControllerName,
                                ActionName = menu.ActionName,
                                Description = menu.Description,
                                IsHeader = menu.IsHeader,
                                Icon = menu.Icon,
                                Sort = menu.Sort,
                                CreateBy = menu.CreateBy,
                                CreateDate = menu.CreateDate,
                                UpdateBy = menu.UpdateBy,
                                UpdateDate = menu.UpdateDate
                            }).Take(1).AsNoTracking();

                if (menuList.Count() > 0)
                {
                    return menuList;
                }

                menuAdd.Name = param.Name;
                menuAdd.ControllerName = param.ControllerName;
                menuAdd.ActionName = param.ActionName;
                menuAdd.Description = param.Description;
                menuAdd.IsHeader = param.IsHeader;
                menuAdd.Sort = param.Sort;
                menuAdd.CreateBy = param.CreateBy;
                menuAdd.CreateDate = DateTime.Now;
                _context.Menu.Add(menuAdd);
                await _context.SaveChangesAsync();

                menuList = (from menu in _context.Menu
                            where menu.Name == param.Name
                            select new MenuResponse
                            {
                                Id = menu.Id,
                                Name = menu.Name,
                                ControllerName = menu.ControllerName,
                                ActionName = menu.ActionName,
                                Description = menu.Description,
                                IsHeader = menu.IsHeader,
                                Icon = menu.Icon,
                                Sort = menu.Sort,
                                CreateBy = menu.CreateBy,
                                CreateDate = menu.CreateDate,
                                UpdateBy = menu.UpdateBy,
                                UpdateDate = menu.UpdateDate
                            }).Take(1).AsNoTracking();


                return menuList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "CreateMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return menuList;
            }

        }

        public async Task<IEnumerable<MenuResponse>> UpdateMenu(MenuUpdateRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MenuResponse>? menuList = null;
            try
            {

                var menuUpdate = await _context.Menu.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (menuUpdate != null)
                {
                    menuUpdate.Name = param.Name;
                    menuUpdate.ControllerName = param.ControllerName;
                    menuUpdate.ActionName = param.ActionName;
                    menuUpdate.Description = param.Description;
                    menuUpdate.IsHeader = param.IsHeader;
                    menuUpdate.Sort = param.Sort;
                    menuUpdate.UpdateBy = param.UpdateBy;
                    menuUpdate.UpdateDate = DateTime.Now;
                    _context.Menu.Update(menuUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    menuList = (from menu in _context.Menu
                                where menu.Name == param.Name
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.ControllerName,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Icon = menu.Icon,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).Take(0).AsNoTracking();

                    return menuList;
                }


                menuList = (from menu in _context.Menu
                            where menu.Name == param.Name
                            select new MenuResponse
                            {
                                Id = menu.Id,
                                Name = menu.Name,
                                ControllerName = menu.ControllerName,
                                ActionName = menu.ActionName,
                                Description = menu.Description,
                                IsHeader = menu.IsHeader,
                                Icon = menu.Icon,
                                Sort = menu.Sort,
                                CreateBy = menu.CreateBy,
                                CreateDate = menu.CreateDate,
                                UpdateBy = menu.UpdateBy,
                                UpdateDate = menu.UpdateDate
                            }).Take(1).AsNoTracking();


                return menuList;
            }
            catch (Exception ex)
            {
                LogErrorAddRequest logDataError = new LogErrorAddRequest();
                logDataError.ServiceName = "UpdateMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _logError.CreateLogError(logDataError, cancellationToken);
                return menuList;
            }

        }
    }
}
