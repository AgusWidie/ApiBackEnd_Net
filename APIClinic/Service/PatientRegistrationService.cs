using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;
using System.IO;

namespace APIClinic.Service
{
    public class PatientRegistrationService : IPatientRegistrationService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly IPatientRegistration _patientRegistrationRepo;

        public PatientRegistrationService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, IPatientRegistration patientRegistrationRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _patientRegistrationRepo = patientRegistrationRepo;
        }

        public async Task<List<PatientRegistrationResponse>> GetPatientRegistration(PatientRegistrationSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listPatientRegistration.Count() > 0)
                {
                    if (param.DoctorId != null || param.QueueNo == "")
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listPatientRegistration.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.DoctorId == param.DoctorId && x.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd"));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listPatientRegistration.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.DoctorId == param.DoctorId && x.QueueNo == param.QueueNo && x.RegistrationDate.ToString("yyyy-MM-dd") == param.RegistrationDate.ToString("yyyy-MM-dd"));
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _patientRegistrationRepo.GetPatientRegistration(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                GeneralList._listPatientRegistration.Clear();
                return null;
            }

        }

        public async Task<List<PatientRegistrationResponse>> CreatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _patientRegistrationRepo.CreatePatientRegistration(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listPatientRegistration.Add(resultData.First());
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

        public async Task<List<PatientRegistrationResponse>> UpdatePatientRegistration(PatientRegistrationRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _patientRegistrationRepo.UpdatePatientRegistration(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listPatientRegistration.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.SpecialistDoctorId = resultData.First().SpecialistDoctorId;
                        checkData.RegistrationDate = DateTime.Now;
                        checkData.Ktpno = resultData.First().Ktpno;
                        checkData.FamilyCardNo = resultData.First().FamilyCardNo;
                        checkData.Name = resultData.First().Name;
                        checkData.DateOfBirth = resultData.First().DateOfBirth;
                        checkData.Gender = resultData.First().Gender;
                        checkData.Address = resultData.First().Address;
                        checkData.Religion = resultData.First().Religion;
                        checkData.Education = resultData.First().Education;
                        checkData.Work = resultData.First().Work;
                        checkData.PaymentType = resultData.First().PaymentType;
                        checkData.Bpjsno = resultData.First().Bpjsno;
                        checkData.InsuranceName = resultData.First().InsuranceName;
                        checkData.InsuranceNo = resultData.First().InsuranceNo;
                        checkData.Complaint = resultData.First().Complaint;
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
                GeneralList._listPatientRegistration.Clear();
                return null;
            }


        }
    }
}
