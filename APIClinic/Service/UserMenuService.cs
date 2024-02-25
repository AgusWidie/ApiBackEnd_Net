using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class UserMenuService : IUserMenuService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IUserMenu _userMenuRepo;

        public UserMenuService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IUserMenu userMenuRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
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
                        return GeneralList._listUserMenuParent.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId).ToList();
                    }
                    else
                    {
                        return GeneralList._listUserMenuParent.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.UserName == param.UserName).ToList();
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
                return null;
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
                        return GeneralList._listUserMenu.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId).ToList();

                    }
                    else
                    {
                        return GeneralList._listUserMenu.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.UserName == param.UserName).ToList();

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
                return null;
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
                return null;
            }

        }
    }
}
