using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Service
{
    public class MenuService : IMenuService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IMenu _menuRepo;

        public MenuService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IMenu menuRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _menuRepo = menuRepo;
        }

        public async Task<List<MenuResponse>> GetMenu(MenuSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listMenu.Count() > 0)
                {
                    if (param.Name != null && param.Name != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listMenu.Where(x => param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listMenu;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _menuRepo.GetMenu(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listMenu.Clear();
                return null;
            }

        }

        public async Task<List<MenuResponse>> CreateMenu(MenuRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _menuRepo.CreateMenu(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listMenu.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listMenu.Clear();
                return null;
            }


        }

        public async Task<List<MenuResponse>> UpdateMenu(MenuRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _menuRepo.UpdateMenu(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listMenu.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.Name = resultData.First().Name;
                        checkData.ControllerName = resultData.First().ControllerName;
                        checkData.ActionName = resultData.First().ActionName;
                        checkData.Description = resultData.First().Description;
                        checkData.IsHeader = resultData.First().IsHeader;
                        checkData.Active = resultData.First().Active;
                        checkData.Sort = resultData.First().Sort;
                        checkData.UpdateBy = resultData.First().CreateBy;
                        checkData.UpdateDate = DateTime.Now;

                    }
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listMenu.Clear();
                return null;
            }


        }
    }


}
