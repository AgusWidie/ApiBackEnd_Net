using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class MenuRepository : IMenu
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public MenuRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<MenuResponse>> GetMenu(MenuSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MenuResponse>? menuList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Name != null && param.Name != "")
                {
                    menuList = (from menu in _context.Menu
                                where param.Name.Contains(menu.Name)
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.Name,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Active = menu.Active,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).OrderBy(x => x.Sort).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);

                }
                else
                {
                    menuList = (from menu in _context.Menu
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.Name,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Active = menu.Active,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).OrderBy(x => x.Sort).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return menuList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return menuList;
            }
        }

        public async Task<IEnumerable<MenuResponse>> CreateMenu(MenuRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<MenuResponse>? menuList = null;
            Menu menuAdd = new Menu();
            try
            {
                menuAdd.Name = param.Name;
                menuAdd.ControllerName = param.ControllerName;
                menuAdd.ActionName = param.ActionName;
                menuAdd.Description = param.Description;
                menuAdd.IsHeader = param.IsHeader;
                menuAdd.Active = param.Active;
                menuAdd.Sort = param.Sort;
                menuAdd.CreateBy = param.CreateBy;
                menuAdd.CreateDate = DateTime.Now;
                _context.Menu.Add(menuAdd);
                await _context.SaveChangesAsync();

                menuList = (from menu in _context.Menu
                            where param.Name.Contains(menu.Name)
                            select new MenuResponse
                            {
                                Id = menu.Id,
                                Name = menu.Name,
                                ControllerName = menu.Name,
                                ActionName = menu.ActionName,
                                Description = menu.Description,
                                IsHeader = menu.IsHeader,
                                Active = menu.Active,
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return menuList;
            }

        }

        public async Task<IEnumerable<MenuResponse>> UpdateMenu(MenuRequest param, CancellationToken cancellationToken)
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
                    menuUpdate.Active = param.Active;
                    menuUpdate.Sort = param.Sort;
                    menuUpdate.UpdateBy = param.UpdateBy;
                    menuUpdate.UpdateDate = DateTime.Now;
                    _context.Menu.Update(menuUpdate);
                    await _context.SaveChangesAsync();


                }
                else
                {
                    menuList = (from menu in _context.Menu
                                where menu.Id == param.Id
                                select new MenuResponse
                                {
                                    Id = menu.Id,
                                    Name = menu.Name,
                                    ControllerName = menu.Name,
                                    ActionName = menu.ActionName,
                                    Description = menu.Description,
                                    IsHeader = menu.IsHeader,
                                    Active = menu.Active,
                                    Sort = menu.Sort,
                                    CreateBy = menu.CreateBy,
                                    CreateDate = menu.CreateDate,
                                    UpdateBy = menu.UpdateBy,
                                    UpdateDate = menu.UpdateDate
                                }).Take(0).AsNoTracking();

                    return menuList;
                }


                menuList = (from menu in _context.Menu
                            where menu.Id == param.Id
                            select new MenuResponse
                            {
                                Id = menu.Id,
                                Name = menu.Name,
                                ControllerName = menu.Name,
                                ActionName = menu.ActionName,
                                Description = menu.Description,
                                IsHeader = menu.IsHeader,
                                Active = menu.Active,
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
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateMenu";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return menuList;
            }

        }
    }
}
