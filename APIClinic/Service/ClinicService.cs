using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class ClinicService : IClinicService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IClinic _clinicRepo;

        public ClinicService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IClinic clinicRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _clinicRepo = clinicRepo;
        }

        public async Task<List<ClinicResponse>> GetClinic(ClinicSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listClinic.Count() > 0)
                {
                    if (param.Name != null && param.Name != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listClinic.Where(x => param.Name.Contains(x.Name));
                        var TotalPageSize = Math.Ceiling((decimal)GeneralList._listClinic.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listClinic.ToList();
                        var TotalPageSize = Math.Ceiling((decimal)GeneralList._listClinic.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _clinicRepo.GetClinic(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listClinic.Clear();
                return null;
            }

        }

        public async Task<List<ClinicResponse>> CreateClinic(ClinicRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _clinicRepo.CreateClinic(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listClinic.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listClinic.Clear();
                return null;
            }


        }

        public async Task<List<ClinicResponse>> UpdateClinic(ClinicRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _clinicRepo.UpdateClinic(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listClinic.Where(x => x.Id == param.Id).FirstOrDefault();
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
                GeneralList._listClinic.Clear();
                return null;
            }


        }
    }
}
