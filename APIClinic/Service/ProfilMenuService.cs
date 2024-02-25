using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class ProfilMenuService : IProfilMenuService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IProfilMenu _profilMenuRepo;

        public ProfilMenuService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IProfilMenu profilMenuRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _profilMenuRepo = profilMenuRepo;
        }

        public async Task<List<ProfilMenuResponse>> GetProfilMenu(ProfilMenuSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listProfilMenu.Count() > 0)
                {
                    if (param.ProfilId != null && param.ProfilId != 0 && param.ParentMenuId != null && param.ParentMenuId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProfilMenu.Where(x => x.ProfilId == param.ProfilId && x.ParentMenuId == param.ParentMenuId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else if (param.ProfilId != null && param.ProfilId != 0 && (param.ParentMenuId == null || param.ParentMenuId == 0))
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProfilMenu.Where(x => x.ProfilId == param.ProfilId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProfilMenu.Where(x => x.ProfilId == param.ProfilId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _profilMenuRepo.GetProfilMenu(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ProfilMenuResponse>> CreateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _profilMenuRepo.CreateProfilMenu(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listProfilMenu.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ProfilMenuResponse>> UpdateProfilMenu(ProfilMenuRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _profilMenuRepo.UpdateProfilMenu(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listProfilMenu.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.ProfilId = resultData.First().ProfilId;
                        checkData.ProfilName = resultData.First().ProfilName;
                        checkData.ParentMenuId = resultData.First().ParentMenuId;
                        checkData.ParentMenuName = resultData.First().ParentMenuName;
                        checkData.MenuId = resultData.First().MenuId;
                        checkData.MenuName = resultData.First().MenuName;
                        checkData.UpdateBy = resultData.First().UpdateBy;
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
                return null;
            }


        }
    }
}
