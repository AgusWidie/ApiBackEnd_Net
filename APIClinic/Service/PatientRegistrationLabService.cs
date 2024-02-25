using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class PatientRegistrationLabService : IPatientRegistrationLabService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IPatientRegistrationLab _patientLabRegistrationRepo;

        public PatientRegistrationLabService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IPatientRegistrationLab patientLabRegistrationRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _patientLabRegistrationRepo = patientLabRegistrationRepo;
        }

        public async Task<List<PatientRegistrationLabResponse>> GetPatientLabRegistration(PatientRegistrationLabSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listPatientRegistrationLab.Count() > 0)
                {
                    if (param.QueueNo == null || param.QueueNo == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listPatientRegistrationLab.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd"));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listPatientRegistrationLab.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.QueueNo == param.QueueNo && x.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd"));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _patientLabRegistrationRepo.GetPatientRegistrationLab(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listPatientRegistrationLab.Clear();
                return null;
            }

        }

        public async Task<List<PatientRegistrationLabResponse>> CreatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _patientLabRegistrationRepo.CreatePatientRegistrationLab(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listPatientRegistrationLab.Add(resultData.First());
                    return resultData.ToList();
                }
                else
                {
                    return resultData.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listPatientRegistrationLab.Clear();
                return null;
            }
        }

        public async Task<List<PatientRegistrationLabResponse>> UpdatePatientRegistrationLab(PatientRegistrationLabRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _patientLabRegistrationRepo.UpdatePatientRegistrationLab(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listPatientRegistrationLab.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {

                        checkData.LaboratoriumId = resultData.First().LaboratoriumId;
                        checkData.RegistrationDate = resultData.First().RegistrationDate;
                        checkData.Ktpno = resultData.First().Ktpno;
                        checkData.FamilyCardNo = resultData.First().FamilyCardNo;
                        checkData.Name = resultData.First().Name;
                        checkData.DateOfBirth = resultData.First().DateOfBirth;
                        checkData.Gender = resultData.First().Gender;
                        checkData.Address = resultData.First().Address;
                        checkData.Religion = resultData.First().Religion;
                        checkData.Education = resultData.First().Education;
                        checkData.Religion = resultData.First().Religion;
                        checkData.Work = resultData.First().Work;
                        checkData.PaymentType = resultData.First().PaymentType;
                        checkData.Bpjsno = resultData.First().Bpjsno;
                        checkData.InsuranceName = resultData.First().InsuranceName;
                        checkData.InsuranceNo = resultData.First().InsuranceNo;
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
                GeneralList._listPatientRegistrationLab.Clear();
                return null;
            }


        }
    }
}
