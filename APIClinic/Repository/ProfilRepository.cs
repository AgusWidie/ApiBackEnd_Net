using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ProfilRepository : IProfil
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public ProfilRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ProfilResponse>> GetProfil(ProfilSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilResponse>? profilList = null;
            long Page = param.Page - 1;
            try
            {
                if (param.Name != null && param.Name != "")
                {
                    profilList = (from pro in _context.Profil
                                  where pro.Name.Contains(param.Name)
                                  orderby pro.Name
                                  select new ProfilResponse
                                  {
                                      Id = pro.Id,
                                      Name = pro.Name,
                                      Description = pro.Description,
                                      Active = pro.Active,
                                      CreateBy = pro.CreateBy,
                                      CreateDate = pro.CreateDate,
                                      UpdateBy = pro.UpdateBy,
                                      UpdateDate = pro.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking().ToList();
                }


                var TotalPageSize = Math.Ceiling((decimal)profilList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = profilList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetProfil";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilList;
            }
        }

        public async Task<IEnumerable<ProfilResponse>> CreateProfil(ProfilRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilResponse>? profilList = null;
            Profil profilAdd = new Profil();
            try
            {
                profilAdd.Name = param.Name;
                profilAdd.Description = param.Description;
                profilAdd.Active = param.Active;
                profilAdd.CreateBy = param.CreateBy;
                profilAdd.CreateDate = DateTime.Now;
                _context.Profil.Add(profilAdd);
                await _context.SaveChangesAsync();

                profilList = (from pro in _context.Profil
                              where pro.Name == param.Name
                              select new ProfilResponse
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Description = pro.Description,
                                  Active = pro.Active,
                                  CreateBy = pro.CreateBy,
                                  CreateDate = pro.CreateDate,
                                  UpdateBy = pro.UpdateBy,
                                  UpdateDate = pro.UpdateDate
                              }).Take(1).AsNoTracking();


                return profilList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateProfil";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilList;
            }

        }

        public async Task<IEnumerable<ProfilResponse>> UpdateProfil(ProfilRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ProfilResponse>? profilList = null;
            try
            {
                var profilUpdate = await _context.Profil.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (profilUpdate != null)
                {
                    profilUpdate.Name = param.Name;
                    profilUpdate.Description = param.Description;
                    profilUpdate.Active = param.Active;
                    profilUpdate.UpdateBy = param.UpdateBy;
                    profilUpdate.UpdateDate = DateTime.Now;
                    _context.Profil.Update(profilUpdate);
                    await _context.SaveChangesAsync();

                }
                else
                {
                    profilList = (from pro in _context.Profil
                                  where pro.Id == param.Id
                                  select new ProfilResponse
                                  {
                                      Id = pro.Id,
                                      Name = pro.Name,
                                      Description = pro.Description,
                                      Active = pro.Active,
                                      CreateBy = pro.CreateBy,
                                      CreateDate = pro.CreateDate,
                                      UpdateBy = pro.UpdateBy,
                                      UpdateDate = pro.UpdateDate
                                  }).Take(0).AsNoTracking();

                    return profilList;
                }


                profilList = (from pro in _context.Profil
                              where pro.Name == param.Name
                              select new ProfilResponse
                              {
                                  Id = pro.Id,
                                  Name = pro.Name,
                                  Description = pro.Description,
                                  Active = pro.Active,
                                  CreateBy = pro.CreateBy,
                                  CreateDate = pro.CreateDate,
                                  UpdateBy = pro.UpdateBy,
                                  UpdateDate = pro.UpdateDate
                              }).Take(1).AsNoTracking();


                return profilList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateProfil";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return profilList;
            }

        }
    }
}
