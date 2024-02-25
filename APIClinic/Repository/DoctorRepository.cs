using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APIClinic.Repository
{
    public class DoctorRepository : IDoctor
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public DoctorRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<DoctorResponse>> GetDoctor(DoctorSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DoctorResponse>? doctorList = null;
            long Page = param.Page - 1;
            try
            {
                if (param.ClinicId != null && (param.DoctorName == null || param.DoctorName == ""))
                {
                    doctorList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  join doc in _context.Doctor on branch.Id equals doc.BranchId
                                  where doc.ClinicId == param.ClinicId && doc.BranchId == param.BranchId
                                  select new DoctorResponse
                                  {
                                      Id = doc.Id,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      BranchId = branch.Id,
                                      BranchName = branch.Name,
                                      DoctorName = doc.DoctorName,
                                      DateOfBirth = doc.DateOfBirth,
                                      Address = doc.Address,
                                      NoTelephone = doc.NoTelephone,
                                      MobilePhone = doc.MobilePhone,
                                      Active = doc.Active,
                                      CreateBy = doc.CreateBy,
                                      CreateDate = doc.CreateDate,
                                      UpdateBy = doc.UpdateBy,
                                      UpdateDate = doc.UpdateDate
                                  }).OrderBy(x => x.DoctorName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.ClinicId != null && (param.DoctorName != null || param.DoctorName != ""))
                {
                    doctorList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  join doc in _context.Doctor on branch.Id equals doc.BranchId
                                  where doc.ClinicId == param.ClinicId && doc.BranchId == param.BranchId && param.DoctorName.Contains(doc.DoctorName)
                                  select new DoctorResponse
                                  {
                                      Id = doc.Id,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      BranchId = branch.Id,
                                      BranchName = branch.Name,
                                      DoctorName = doc.DoctorName,
                                      DateOfBirth = doc.DateOfBirth,
                                      Address = doc.Address,
                                      NoTelephone = doc.NoTelephone,
                                      MobilePhone = doc.MobilePhone,
                                      Active = doc.Active,
                                      CreateBy = doc.CreateBy,
                                      CreateDate = doc.CreateDate,
                                      UpdateBy = doc.UpdateBy,
                                      UpdateDate = doc.UpdateDate
                                  }).OrderBy(x => x.DoctorName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return doctorList;
            }
            catch (Exception ex)
            {
                return doctorList;
            }
        }

        public async Task<IEnumerable<DoctorResponse>> CreateDoctor(DoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DoctorResponse>? doctorList = null;
            Doctor doctorAdd = new Doctor();
            try
            {
                doctorAdd.ClinicId = param.ClinicId;
                doctorAdd.BranchId = param.BranchId;
                doctorAdd.DoctorName = param.DoctorName;
                doctorAdd.DateOfBirth = param.DateOfBirth;
                doctorAdd.Address = param.Address;
                doctorAdd.Gender = param.Gender;
                doctorAdd.Education = param.Education;
                doctorAdd.NoTelephone = param.NoTelephone;
                doctorAdd.MobilePhone = param.MobilePhone;
                doctorAdd.StatusEmployee = param.StatusEmployee;
                doctorAdd.StatusDoctor = param.StatusDoctor;
                doctorAdd.Active = param.Active;
                doctorAdd.CreateBy = param.CreateBy;
                doctorAdd.CreateDate = DateTime.Now;
                _context.Doctor.Add(doctorAdd);
                await _context.SaveChangesAsync();

                doctorList = (from branch in _context.Branch
                              join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                              join doc in _context.Doctor on branch.Id equals doc.BranchId
                              where doc.ClinicId == param.ClinicId && doc.BranchId == param.BranchId && doc.DoctorName == param.DoctorName
                              select new DoctorResponse
                              {
                                  Id = doc.Id,
                                  ClinicId = clinic.Id,
                                  ClinicName = clinic.Name,
                                  BranchId = branch.Id,
                                  BranchName = branch.Name,
                                  DoctorName = doc.DoctorName,
                                  DateOfBirth = doc.DateOfBirth,
                                  Address = doc.Address,
                                  NoTelephone = doc.NoTelephone,
                                  MobilePhone = doc.MobilePhone,
                                  Active = doc.Active,
                                  CreateBy = doc.CreateBy,
                                  CreateDate = doc.CreateDate,
                                  UpdateBy = doc.UpdateBy,
                                  UpdateDate = doc.UpdateDate
                              }).Take(1).AsNoTracking();


                return doctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return doctorList;
            }

        }

        public async Task<IEnumerable<DoctorResponse>> UpdateDoctor(DoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DoctorResponse>? doctorList = null;
            try
            {
                var doctorUpdate = await _context.Doctor.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (doctorUpdate != null)
                {
                    doctorUpdate.DoctorName = param.DoctorName;
                    doctorUpdate.DateOfBirth = param.DateOfBirth;
                    doctorUpdate.Address = param.Address;
                    doctorUpdate.Gender = param.Gender;
                    doctorUpdate.Education = param.Education;
                    doctorUpdate.NoTelephone = param.NoTelephone;
                    doctorUpdate.MobilePhone = param.MobilePhone;
                    doctorUpdate.StatusEmployee = param.StatusEmployee;
                    doctorUpdate.StatusDoctor = param.StatusDoctor;
                    doctorUpdate.Active = param.Active;
                    doctorUpdate.CreateBy = param.CreateBy;
                    doctorUpdate.CreateDate = DateTime.Now;
                    _context.Doctor.Update(doctorUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    doctorList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  join doc in _context.Doctor on branch.Id equals doc.BranchId
                                  where doc.ClinicId == param.ClinicId && doc.BranchId == param.BranchId && doc.Id == param.Id
                                  select new DoctorResponse
                                  {
                                      Id = doc.Id,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      BranchId = branch.Id,
                                      BranchName = branch.Name,
                                      DoctorName = doc.DoctorName,
                                      DateOfBirth = doc.DateOfBirth,
                                      Address = doc.Address,
                                      NoTelephone = doc.NoTelephone,
                                      MobilePhone = doc.MobilePhone,
                                      Active = doc.Active,
                                      CreateBy = doc.CreateBy,
                                      CreateDate = doc.CreateDate,
                                      UpdateBy = doc.UpdateBy,
                                      UpdateDate = doc.UpdateDate
                                  }).Take(0).AsNoTracking();

                    return doctorList;
                }


                doctorList = (from branch in _context.Branch
                              join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                              join doc in _context.Doctor on branch.Id equals doc.BranchId
                              where doc.ClinicId == param.ClinicId && doc.BranchId == param.BranchId && doc.Id == param.Id
                              select new DoctorResponse
                              {
                                  Id = doc.Id,
                                  ClinicId = clinic.Id,
                                  ClinicName = clinic.Name,
                                  BranchId = branch.Id,
                                  BranchName = branch.Name,
                                  DoctorName = doc.DoctorName,
                                  DateOfBirth = doc.DateOfBirth,
                                  Address = doc.Address,
                                  NoTelephone = doc.NoTelephone,
                                  MobilePhone = doc.MobilePhone,
                                  Active = doc.Active,
                                  CreateBy = doc.CreateBy,
                                  CreateDate = doc.CreateDate,
                                  UpdateBy = doc.UpdateBy,
                                  UpdateDate = doc.UpdateDate
                              }).Take(1).AsNoTracking();

                return doctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return doctorList;
            }

        }
    }
}
