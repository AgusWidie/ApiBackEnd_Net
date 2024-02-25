using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class ExaminationDoctorService : IExaminationDoctorService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IExaminationDoctor _examinationDoctorRepo;

        public ExaminationDoctorService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IExaminationDoctor examinationDoctorRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _examinationDoctorRepo = examinationDoctorRepo;
        }

        public async Task<List<ExaminationDoctorResponse>> GetExaminationDoctor(ExaminationDoctorSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listExaminationDoctor.Count() > 0)
                {
                    if (param.DoctorId != null && param.QueueNo == "")
                    {
                        param.Page = param.Page - 1;
                        var resultList = GeneralList._listExaminationDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.DoctorId == param.DoctorId && x.ExaminationDate >= param.ExaminationDateFrom && x.ExaminationDate <= param.ExaminationDateTo);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)param.Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        param.Page = param.Page - 1;
                        var resultList = GeneralList._listExaminationDoctor.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.DoctorId == param.DoctorId && x.ExaminationDate >= param.ExaminationDateFrom && x.ExaminationDate <= param.ExaminationDateTo);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)param.Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _examinationDoctorRepo.GetExamination(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listExaminationDoctor.Clear();
                return null;
            }

        }

        public async Task<List<ExaminationDoctorResponse>> CreateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _examinationDoctorRepo.CreateExaminationDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listExaminationDoctor.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listExaminationDoctor.Clear();
                return null;
            }
        }

        public async Task<List<ExaminationDoctorResponse>> UpdateExaminationDoctor(ExaminationDoctorRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _examinationDoctorRepo.UpdateExaminationDoctor(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listExaminationDoctor.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.Name = resultData.First().Name;
                        checkData.ExaminationDate = resultData.First().ExaminationDate;
                        checkData.DoctorId = resultData.First().DoctorId;
                        checkData.QueueNo = resultData.First().QueueNo;
                        checkData.Inspection = resultData.First().Inspection;
                        checkData.Recipe = resultData.First().Recipe;
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
                GeneralList._listExaminationDoctor.Clear();
                return null;
            }


        }
    }
}
