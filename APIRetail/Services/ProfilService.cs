using APIRetail.CacheList;
using APIRetail.Helper;
using APIRetail.Models.Database;
using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;
using APIRetail.Repository.IRepository;
using APIRetail.Services.Interface;

namespace APIRetail.Services
{
    public class ProfilService : IProfilService
    {
        public readonly IConfiguration _configuration;
        public readonly retail_systemContext _context;
        public readonly ILogError _logError;
        public readonly IProfil _profilRepo;

        public ProfilService(IConfiguration Configuration, retail_systemContext context, ILogError logError, IProfil profilRepo)
        {
            _configuration = Configuration;
            _context = context;
            _logError = logError;
            _profilRepo = profilRepo;
        }

        public async Task<List<ProfilResponse>> GetProfil(ProfilRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listProfil.Count() > 0)
                {
                    if (param.Name != null && param.Name != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProfil.Where(x => param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listProfil;
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _profilRepo.GetProfil(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listProfil.Clear();
                throw;
            }

        }

        public async Task<List<ProfilResponse>> CreateProfil(ProfilAddRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _profilRepo.CreateProfil(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listProfil.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listProfil.Clear();
                throw;
            }
        }


        public async Task<List<ProfilResponse>> UpdateProfil(ProfilUpdateRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _profilRepo.UpdateProfil(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listProfil.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
                        checkData.Description = resultData.First().Description;
                        checkData.Active = resultData.First().Active;
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
                GeneralList._listProfil.Clear();
                throw;
            }


        }
    }
}
