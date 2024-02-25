using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class BranchService : IBranchService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IBranch _branchRepo;

        public BranchService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IBranch branchRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _branchRepo = branchRepo;
        }

        public async Task<List<BranchResponse>> GetBranch(BranchSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listBranch.Count() > 0)
                {
                    if (param.ClinicId != null && (param.Name == null || param.Name == ""))
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listBranch.Where(x => x.ClinicId == param.ClinicId);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listBranch.Where(x => x.ClinicId == param.ClinicId && param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _branchRepo.GetBranch(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<BranchResponse>> CreateBranch(BranchRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _branchRepo.CreateBranch(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listBranch.Add(resultData.First());
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

        public async Task<List<BranchResponse>> UpdateBranch(BranchRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _branchRepo.UpdateBranch(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listBranch.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
                        checkData.Address = resultData.First().Address;
                        checkData.Telp = resultData.First().Telp;
                        checkData.Fax = resultData.First().Fax;
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
