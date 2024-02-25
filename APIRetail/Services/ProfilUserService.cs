using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class ProfilUserService : IProfilUserService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IProfilUser _profilUserRepo;

        public ProfilUserService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IProfilUser profilUserRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _profilUserRepo = profilUserRepo;
        }

        public async Task<List<ProfilUserResponse>> GetProfilUser(ProfilUserRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listProfilUser.Count() > 0)
                {
                    if (param.ProfilId != null && param.ProfilId != 0)
                    {
                        long Page = param.Page - 1;
                        var resultUserList = GeneralList._listUser.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var resultList = GeneralList._listProfilUser.Where(x => x.ProfilId == param.ProfilId && resultUserList.First().Id.ToString().Contains(x.UserId.ToString()));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultUserList = GeneralList._listUser.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var resultList = GeneralList._listProfilUser.Where(x => resultUserList.First().Id.ToString().Contains(x.UserId.ToString()));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _profilUserRepo.GetProfilUser(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listProfilUser.Clear();
                throw;
            }

        }

        public async Task<List<ProfilUserResponse>> CreateProfilUser(ProfilUserAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _profilUserRepo.CreateProfilUser(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listProfilUser.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listProfilUser.Clear();
                throw;
            }
        }

        public async Task<List<ProfilUserResponse>> UpdateProfilUser(ProfilUserUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _profilUserRepo.UpdateProfilUser(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listProfilUser.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.ProfilId = resultData.First().ProfilId;
                        checkData.ProfilName = resultData.First().ProfilName;
                        checkData.UserId = resultData.First().UserId;
                        checkData.UserName = resultData.First().UserName;
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
                GeneralList._listProfilUser.Clear();
                throw;
            }


        }
    }
}
