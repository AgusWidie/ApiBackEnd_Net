using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class SpecialDoctorRepository : ISpecialDoctor
    {

        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public SpecialDoctorRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<SpecialistDoctorResponse>> GetSpecialistDoctor(SpecialistDoctorSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SpecialistDoctorResponse>? specialDoctorList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Name == null || param.Name == "")
                {
                    specialDoctorList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join spe in _context.SpecialistDoctor on branch.Id equals spe.BranchId
                                         join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                         join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                         where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId
                                         select new SpecialistDoctorResponse
                                         {
                                             Id = spe.Id,
                                             ClinicId = clinic.Id,
                                             ClinicName = clinic.Name,
                                             BranchId = branch.Id,
                                             BranchName = branch.Name,
                                             DoctorId = spe.DoctorId,
                                             DoctorName = doc.DoctorName,
                                             SpecialistId = specialist.Id,
                                             SpecialistName = specialist.Name,
                                             Description = spe.Description,
                                             Active = spe.Active,
                                             CreateBy = spe.CreateBy,
                                             CreateDate = spe.CreateDate,
                                             UpdateBy = spe.UpdateBy,
                                             UpdateDate = spe.UpdateDate
                                         }).OrderBy(x => x.SpecialistName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.Name != null || param.Name != "")
                {
                    specialDoctorList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join spe in _context.SpecialistDoctor on branch.Id equals spe.BranchId
                                         join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                         join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                         where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId && param.Name.Contains(specialist.Name)
                                         select new SpecialistDoctorResponse
                                         {
                                             Id = spe.Id,
                                             ClinicId = clinic.Id,
                                             ClinicName = clinic.Name,
                                             BranchId = branch.Id,
                                             BranchName = branch.Name,
                                             DoctorId = spe.DoctorId,
                                             DoctorName = doc.DoctorName,
                                             SpecialistId = specialist.Id,
                                             SpecialistName = specialist.Name,
                                             Description = spe.Description,
                                             Active = spe.Active,
                                             CreateBy = spe.CreateBy,
                                             CreateDate = spe.CreateDate,
                                             UpdateBy = spe.UpdateBy,
                                             UpdateDate = spe.UpdateDate
                                         }).OrderBy(x => x.SpecialistName).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return specialDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetSpecialistDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return specialDoctorList;
            }
        }

        public async Task<IEnumerable<SpecialistDoctorResponse>> CreateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SpecialistDoctorResponse>? specialDoctorList = null;
            SpecialistDoctor specialistAdd = new SpecialistDoctor();
            try
            {
                specialistAdd.ClinicId = param.ClinicId;
                specialistAdd.BranchId = param.BranchId;
                specialistAdd.DoctorId = param.DoctorId;
                specialistAdd.SpecialistId = param.SpecialistId;
                specialistAdd.Description = param.Description;
                specialistAdd.Active = param.Active;
                specialistAdd.CreateBy = param.CreateBy;
                specialistAdd.CreateDate = DateTime.Now;
                _context.SpecialistDoctor.Add(specialistAdd);
                await _context.SaveChangesAsync();

                specialDoctorList = (from branch in _context.Branch
                                     join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                     join spe in _context.SpecialistDoctor on branch.Id equals spe.BranchId
                                     join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                     join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                     where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId && spe.Id == param.Id
                                     select new SpecialistDoctorResponse
                                     {
                                         Id = spe.Id,
                                         ClinicId = clinic.Id,
                                         ClinicName = clinic.Name,
                                         BranchId = branch.Id,
                                         BranchName = branch.Name,
                                         DoctorId = spe.DoctorId,
                                         DoctorName = doc.DoctorName,
                                         SpecialistId = specialist.Id,
                                         SpecialistName = specialist.Name,
                                         Description = spe.Description,
                                         Active = spe.Active,
                                         CreateBy = spe.CreateBy,
                                         CreateDate = spe.CreateDate,
                                         UpdateBy = spe.UpdateBy,
                                         UpdateDate = spe.UpdateDate
                                     }).Take(1).AsNoTracking();


                return specialDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateSpecialistDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return specialDoctorList;
            }

        }
        public async Task<IEnumerable<SpecialistDoctorResponse>> UpdateSpecialistDoctor(SpecialistDoctorRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SpecialistDoctorResponse>? specialDoctorList = null;
            try
            {
                var specialistUpdate = await _context.SpecialistDoctor.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (specialistUpdate != null)
                {
                    specialistUpdate.ClinicId = param.ClinicId;
                    specialistUpdate.BranchId = param.BranchId;
                    specialistUpdate.DoctorId = param.DoctorId;
                    specialistUpdate.SpecialistId = param.SpecialistId;
                    specialistUpdate.Description = param.Description;
                    specialistUpdate.Active = param.Active;
                    specialistUpdate.UpdateBy = param.UpdateBy;
                    specialistUpdate.UpdateDate = DateTime.Now;
                    _context.SpecialistDoctor.Update(specialistUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    specialDoctorList = (from branch in _context.Branch
                                         join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                         join spe in _context.SpecialistDoctor on branch.Id equals spe.BranchId
                                         join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                         join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                         where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId && spe.Id == param.Id
                                         select new SpecialistDoctorResponse
                                         {
                                             Id = spe.Id,
                                             ClinicId = clinic.Id,
                                             ClinicName = clinic.Name,
                                             BranchId = branch.Id,
                                             BranchName = branch.Name,
                                             DoctorId = spe.DoctorId,
                                             DoctorName = doc.DoctorName,
                                             SpecialistId = specialist.Id,
                                             SpecialistName = specialist.Name,
                                             Description = spe.Description,
                                             Active = spe.Active,
                                             CreateBy = spe.CreateBy,
                                             CreateDate = spe.CreateDate,
                                             UpdateBy = spe.UpdateBy,
                                             UpdateDate = spe.UpdateDate
                                         }).Take(0).AsNoTracking();

                    return specialDoctorList;
                }


                specialDoctorList = (from branch in _context.Branch
                                     join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                     join spe in _context.SpecialistDoctor on branch.Id equals spe.BranchId
                                     join doc in _context.Doctor on spe.DoctorId equals doc.Id
                                     join specialist in _context.Specialist on spe.SpecialistId equals specialist.Id
                                     where spe.ClinicId == param.ClinicId && spe.BranchId == param.BranchId && spe.Id == param.Id
                                     select new SpecialistDoctorResponse
                                     {
                                         Id = spe.Id,
                                         ClinicId = clinic.Id,
                                         ClinicName = clinic.Name,
                                         BranchId = branch.Id,
                                         BranchName = branch.Name,
                                         DoctorId = spe.DoctorId,
                                         DoctorName = doc.DoctorName,
                                         SpecialistId = specialist.Id,
                                         SpecialistName = specialist.Name,
                                         Description = spe.Description,
                                         Active = spe.Active,
                                         CreateBy = spe.CreateBy,
                                         CreateDate = spe.CreateDate,
                                         UpdateBy = spe.UpdateBy,
                                         UpdateDate = spe.UpdateDate
                                     }).Take(1).AsNoTracking();

                return specialDoctorList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateSpecialistDoctor";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return specialDoctorList;
            }

        }

    }
}
