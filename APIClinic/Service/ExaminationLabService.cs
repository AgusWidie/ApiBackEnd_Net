using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class ExaminationLabService : IExaminationLabService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IExaminationLab _examinationLabRepo;

        public ExaminationLabService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IExaminationLab examinationLabRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _examinationLabRepo = examinationLabRepo;
        }

        public async Task<List<ExaminationLabResponse>> GetExaminationLab(ExaminationLabSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listExaminationLab.Count() > 0)
                {
                    if (param.QueueNo != null && param.QueueNo != "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listExaminationLab.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.QueueNo == param.QueueNo && x.ExaminationDate >= param.ExaminationDateFrom && x.ExaminationDate <= param.ExaminationDateTo);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listExaminationLab.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.ExaminationDate >= param.ExaminationDateFrom && x.ExaminationDate <= param.ExaminationDateTo);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _examinationLabRepo.GetExaminationLab(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listExaminationLab.Clear();
                return null;
            }

        }

        public async Task<List<ExaminationLabResponse>> CreateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _examinationLabRepo.CreateExaminationLab(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listExaminationLab.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listExaminationLab.Clear();
                return null;
            }
        }

        public async Task<List<ExaminationLabResponse>> UpdateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _examinationLabRepo.UpdateExaminationLab(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listExaminationLab.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.ExaminationDate = resultData.First().ExaminationDate;
                        checkData.QueueNo = resultData.First().QueueNo;
                        checkData.Description = resultData.First().Description;
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
                GeneralList._listExaminationLab.Clear();
                return null;
            }


        }
    }
}
