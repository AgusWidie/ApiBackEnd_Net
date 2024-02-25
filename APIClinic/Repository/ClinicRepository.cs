using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ClinicRepository : IClinic
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public ClinicRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ClinicResponse>> GetClinic(ClinicSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ClinicResponse>? clinicList = null;
            long Page = param.Page - 1;
            try
            {
                if (param.Name != null && param.Name != "")
                {
                    clinicList = (from clinic in _context.Clinic
                                  where clinic.Name == param.Name
                                  select new ClinicResponse
                                  {
                                      Id = clinic.Id,
                                      Name = clinic.Name,
                                      Address = clinic.Address,
                                      Telp = clinic.Telp,
                                      Fax = clinic.Fax,
                                      CreateBy = clinic.CreateBy,
                                      CreateDate = clinic.CreateDate,
                                      UpdateBy = clinic.UpdateBy,
                                      UpdateDate = clinic.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.Name == null || param.Name == "")
                {
                    clinicList = (from clinic in _context.Clinic
                                  select new ClinicResponse
                                  {
                                      Id = clinic.Id,
                                      Name = clinic.Name,
                                      Address = clinic.Address,
                                      Telp = clinic.Telp,
                                      Fax = clinic.Fax,
                                      CreateBy = clinic.CreateBy,
                                      CreateDate = clinic.CreateDate,
                                      UpdateBy = clinic.UpdateBy,
                                      UpdateDate = clinic.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return clinicList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetClinic";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return clinicList;
            }
        }

        public async Task<IEnumerable<ClinicResponse>> CreateClinic(ClinicRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ClinicResponse>? clinicList = null;
            Clinic clinicAdd = new Clinic();
            try
            {
                clinicAdd.Name = param.Name;
                clinicAdd.Address = param.Address;
                clinicAdd.Telp = param.Telp;
                clinicAdd.Fax = param.Fax;
                clinicAdd.CreateBy = param.CreateBy;
                clinicAdd.CreateDate = DateTime.Now;
                _context.Clinic.Add(clinicAdd);
                await _context.SaveChangesAsync();

                clinicList = (from clinic in _context.Clinic
                              where clinic.Name == param.Name
                              select new ClinicResponse
                              {
                                  Id = clinic.Id,
                                  Name = clinic.Name,
                                  Address = clinic.Address,
                                  Telp = clinic.Telp,
                                  Fax = clinic.Fax,
                                  CreateBy = clinic.CreateBy,
                                  CreateDate = clinic.CreateDate,
                                  UpdateBy = clinic.UpdateBy,
                                  UpdateDate = clinic.UpdateDate
                              }).Take(1).AsNoTracking();


                return clinicList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateClinic";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return clinicList;
            }

        }

        public async Task<IEnumerable<ClinicResponse>> UpdateClinic(ClinicRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ClinicResponse>? clinicList = null;
            try
            {
                var clinicUpdate = await _context.Clinic.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (clinicUpdate != null)
                {
                    clinicUpdate.Name = param.Name;
                    clinicUpdate.Address = param.Address;
                    clinicUpdate.Telp = param.Telp;
                    clinicUpdate.Fax = param.Fax;
                    clinicUpdate.UpdateBy = param.UpdateBy;
                    clinicUpdate.UpdateDate = DateTime.Now;
                    _context.Clinic.Update(clinicUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    clinicList = (from clinic in _context.Clinic
                                  where clinic.Id == param.Id
                                  select new ClinicResponse
                                  {
                                      Id = clinic.Id,
                                      Name = clinic.Name,
                                      Address = clinic.Address,
                                      Telp = clinic.Telp,
                                      Fax = clinic.Fax,
                                      CreateBy = clinic.CreateBy,
                                      CreateDate = clinic.CreateDate,
                                      UpdateBy = clinic.UpdateBy,
                                      UpdateDate = clinic.UpdateDate
                                  }).Take(0).AsNoTracking();

                    return clinicList;
                }

                clinicList = (from clinic in _context.Clinic
                              where clinic.Id == param.Id
                              select new ClinicResponse
                              {
                                  Id = clinic.Id,
                                  Name = clinic.Name,
                                  Address = clinic.Address,
                                  Telp = clinic.Telp,
                                  Fax = clinic.Fax,
                                  CreateBy = clinic.CreateBy,
                                  CreateDate = clinic.CreateDate,
                                  UpdateBy = clinic.UpdateBy,
                                  UpdateDate = clinic.UpdateDate
                              }).Take(1).AsNoTracking();


                return clinicList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateClinic";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return clinicList;
            }

        }
    }
}
