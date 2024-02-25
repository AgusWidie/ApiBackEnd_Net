using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class SpecialistRepository : ISpecialist
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public SpecialistRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<SpecialistResponse>> GetSpecialist(SpecialistSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SpecialistResponse>? specialistList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.Name != null && param.Name != "")
                {
                    specialistList = (from spe in _context.Specialist
                                      where spe.Name.Contains(param.Name)
                                      orderby spe.Name
                                      select new SpecialistResponse
                                      {
                                          Id = spe.Id,
                                          Name = spe.Name,
                                          Active = spe.Active,
                                          CreateBy = spe.CreateBy,
                                          CreateDate = spe.CreateDate,
                                          UpdateBy = spe.UpdateBy,
                                          UpdateDate = spe.UpdateDate
                                      }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }


                return specialistList;
            }
            catch (Exception ex)
            {
                return specialistList;
            }
        }

        public async Task<IEnumerable<SpecialistResponse>> CreateSpecialist(SpecialistRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SpecialistResponse>? specialistList = null;
            Specialist specialistAdd = new Specialist();
            try
            {
                specialistAdd.Name = param.Name;
                specialistAdd.Active = param.Active;
                specialistAdd.CreateBy = param.CreateBy;
                specialistAdd.CreateDate = DateTime.Now;
                _context.Specialist.Add(specialistAdd);
                await _context.SaveChangesAsync();

                specialistList = (from spe in _context.Specialist
                                  where spe.Name == param.Name
                                  select new SpecialistResponse
                                  {
                                      Id = spe.Id,
                                      Name = spe.Name,
                                      Active = spe.Active,
                                      CreateBy = spe.CreateBy,
                                      CreateDate = spe.CreateDate,
                                      UpdateBy = spe.UpdateBy,
                                      UpdateDate = spe.UpdateDate
                                  }).Take(1).AsNoTracking();


                return specialistList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateSpecialist";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return specialistList;
            }

        }

        public async Task<IEnumerable<SpecialistResponse>> UpdateSpecialist(SpecialistRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<SpecialistResponse>? specialistList = null;
            try
            {
                var specialistUpdate = await _context.Specialist.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (specialistUpdate != null)
                {
                    specialistUpdate.Name = param.Name;
                    specialistUpdate.Active = param.Active;
                    specialistUpdate.UpdateBy = param.UpdateBy;
                    specialistUpdate.UpdateDate = DateTime.Now;
                    _context.Specialist.Update(specialistUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    specialistList = (from spe in _context.Specialist
                                      where spe.Id == param.Id
                                      select new SpecialistResponse
                                      {
                                          Id = spe.Id,
                                          Name = spe.Name,
                                          Active = spe.Active,
                                          CreateBy = spe.CreateBy,
                                          CreateDate = spe.CreateDate,
                                          UpdateBy = spe.UpdateBy,
                                          UpdateDate = spe.UpdateDate
                                      }).Take(0).AsNoTracking();


                    return specialistList;
                }


                specialistList = (from spe in _context.Specialist
                                  where spe.Id == param.Id
                                  select new SpecialistResponse
                                  {
                                      Id = spe.Id,
                                      Name = spe.Name,
                                      Active = spe.Active,
                                      CreateBy = spe.CreateBy,
                                      CreateDate = spe.CreateDate,
                                      UpdateBy = spe.UpdateBy,
                                      UpdateDate = spe.UpdateDate
                                  }).Take(1).AsNoTracking();


                return specialistList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateSpecialist";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return specialistList;
            }

        }
    }
}
