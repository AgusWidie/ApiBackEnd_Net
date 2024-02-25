using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class UserService : IUserService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IUser _userRepo;

        public UserService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IUser userRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _userRepo = userRepo;
        }

        public async Task<List<UsersResponse>> GetUser(UsersRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listUser.Count() > 0)
                {
                    if (param.UserName == null || param.UserName == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listUser.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listUser.Where(x => x.CompanyId == param.CompanyId && x.BranchId == param.BranchId && param.UserName.Contains(x.UserName));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                }
                else
                {
                    var resultList = await _userRepo.GetUser(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listUser.Clear();
                throw;
            }

        }

        public async Task<List<UsersResponse>> CreateUser(UsersAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _userRepo.CreateUser(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listUser.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listUser.Clear();
                throw;
            }


        }

        public async Task<List<UsersResponse>> UpdateUser(UsersUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _userRepo.UpdateUser(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listUser.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.UserName = resultData.First().UserName;
                        checkData.Name = resultData.First().Name;
                        checkData.Description = resultData.First().Description;
                        checkData.Active = resultData.First().Active;
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
                GeneralList._listUser.Clear();
                throw;
            }


        }
    }
}
