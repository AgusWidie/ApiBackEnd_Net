using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace APIClinic.Service
{
    public class UserService : IUserService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IUsers _userRepo;

        public UserService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IUsers userRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _userRepo = userRepo;
        }

        public async Task<List<UsersResponse>> GetUser(UserSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listUser.Count() > 0)
                {
                    if (param.UserName == null || param.UserName == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listUser.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listUser.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && param.UserName.Contains(x.UserName));
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
                return null;
            }

        }

        public async Task<List<UsersResponse>> CreateUser(UserRequest param, CancellationToken cancellationToken)
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
                return null;
            }


        }

        public async Task<List<UsersResponse>> UpdateUser(UserRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _userRepo.UpdateUser(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listUser.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.ClinicId = resultData.First().ClinicId;
                        checkData.BranchId = resultData.First().BranchId;
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
                return null;
            }


        }
    }
}
