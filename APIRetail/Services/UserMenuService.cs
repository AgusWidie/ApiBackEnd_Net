using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class UserMenuService : IUserMenuService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IUserMenu _userMenuRepo;

        public UserMenuService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IUserMenu userMenuRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _userMenuRepo = userMenuRepo;
        }

        public async Task<List<UserMenuParentResponse>> GetUserMenuParent(UserMenuParentRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listUserMenuParent.Count() > 0)
                {
                    if (param.UserName == null || param.UserName == "")
                    {
                        return GeneralList._listUserMenuParent.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId).ToList();
                    }
                    else
                    {
                        return GeneralList._listUserMenuParent.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.UserName == param.UserName).ToList();
                    }
                }
                else
                {
                    var resultList = await _userMenuRepo.GetUserMenuParent(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listUserMenuParent.Clear();
                throw;
            }

        }

        public async Task<List<UserMenuResponse>> GetUserMenu(UserMenuRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listUserMenu.Count() > 0)
                {
                    if (param.UserName == null || param.UserName == "")
                    {
                        return GeneralList._listUserMenu.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId).ToList();

                    }
                    else
                    {
                        return GeneralList._listUserMenu.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && x.UserName == param.UserName).ToList();

                    }
                }
                else
                {
                    var resultList = await _userMenuRepo.GetUserMenu(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listUserMenuParent.Clear();
                throw;
            }

        }

        public async Task<List<CheckUserMenuResponse>> GetCheckUserMenu(CheckUserMenuRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listCheckUserMenu.Count() > 0)
                {
                    return GeneralList._listCheckUserMenu.Where(x => x.ProfilId == param.ProfilId && x.UserId == param.UserId && x.ControllerName == param.ControllerName).ToList();
                }
                else
                {
                    var resultList = await _userMenuRepo.GetCheckUserMenu(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listUserMenuParent.Clear();
                throw;
            }

        }
    }
}
