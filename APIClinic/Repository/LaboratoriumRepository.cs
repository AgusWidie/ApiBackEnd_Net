using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APIClinic.Repository
{
    public class LaboratoriumRepository : ILaboratorium
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public LaboratoriumRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<LaboratoriumResponse>> GetLaboratorium(LaboratoriumSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LaboratoriumResponse>? laboratoriumList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.LaboratoriumName != null && param.LaboratoriumName != "")
                {
                    laboratoriumList = (from branch in _context.Branch
                                        join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                        join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                        where lab.ClinicId == param.ClinicId && lab.BranchId == param.BranchId && param.LaboratoriumName.Contains(lab.LaboratoriumName)
                                        select new LaboratoriumResponse
                                        {
                                            Id = lab.Id,
                                            ClinicId = clinic.Id,
                                            ClinicName = clinic.Name,
                                            BranchId = branch.Id,
                                            BranchName = branch.Name,
                                            LaboratoriumName = lab.LaboratoriumName,
                                            Description = lab.Description,
                                            Price = lab.Price,
                                            Active = lab.Active,
                                            CreateBy = lab.CreateBy,
                                            CreateDate = lab.CreateDate,
                                            UpdateBy = lab.UpdateBy,
                                            UpdateDate = lab.UpdateDate
                                        }).OrderBy(x => x.LaboratoriumName).AsNoTracking().ToList();
                }
                else
                {
                    laboratoriumList = (from branch in _context.Branch
                                        join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                        join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                        where lab.ClinicId == param.ClinicId && lab.BranchId == param.BranchId
                                        select new LaboratoriumResponse
                                        {
                                            Id = lab.Id,
                                            ClinicId = clinic.Id,
                                            ClinicName = clinic.Name,
                                            BranchId = branch.Id,
                                            BranchName = branch.Name,
                                            LaboratoriumName = lab.LaboratoriumName,
                                            Description = lab.Description,
                                            Price = lab.Price,
                                            CreateBy = lab.CreateBy,
                                            CreateDate = lab.CreateDate,
                                            UpdateBy = lab.UpdateBy,
                                            UpdateDate = lab.UpdateDate
                                        }).OrderBy(x => x.LaboratoriumName).AsNoTracking().ToList();
                }


                var TotalPageSize = Math.Ceiling((decimal)laboratoriumList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = laboratoriumList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetLaboratorium";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return laboratoriumList;
            }
        }

        public async Task<IEnumerable<LaboratoriumResponse>> CreateLaboratium(LaboratoriumRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LaboratoriumResponse>? laboratoriumList = null;
            Laboratorium laboratoriumAdd = new Laboratorium();
            try
            {
                laboratoriumAdd.ClinicId = param.ClinicId;
                laboratoriumAdd.BranchId = param.BranchId;
                laboratoriumAdd.LaboratoriumName = param.LaboratoriumName;
                laboratoriumAdd.Price = param.Price;
                laboratoriumAdd.Description = param.Description;
                laboratoriumAdd.Active = param.Active;
                laboratoriumAdd.CreateBy = param.CreateBy;
                laboratoriumAdd.CreateDate = DateTime.Now;
                _context.Laboratorium.Add(laboratoriumAdd);
                await _context.SaveChangesAsync();

                laboratoriumList = (from branch in _context.Branch
                                    join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                    join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                    where lab.Id == param.Id
                                    select new LaboratoriumResponse
                                    {
                                        Id = lab.Id,
                                        ClinicId = clinic.Id,
                                        ClinicName = clinic.Name,
                                        BranchId = branch.Id,
                                        BranchName = branch.Name,
                                        LaboratoriumName = lab.LaboratoriumName,
                                        Description = lab.Description,
                                        Price = lab.Price,
                                        CreateBy = lab.CreateBy,
                                        CreateDate = lab.CreateDate,
                                        UpdateBy = lab.UpdateBy,
                                        UpdateDate = lab.UpdateDate
                                    }).Take(1).AsNoTracking();


                return laboratoriumList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateLaboratium";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return laboratoriumList;
            }

        }

        public async Task<IEnumerable<LaboratoriumResponse>> UpdateLaboratorium(LaboratoriumRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<LaboratoriumResponse>? laboratoriumList = null;
            try
            {
                var laboratoriumUpdate = await _context.Laboratorium.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (laboratoriumUpdate != null)
                {
                    laboratoriumUpdate.LaboratoriumName = param.LaboratoriumName;
                    laboratoriumUpdate.Price = param.Price;
                    laboratoriumUpdate.Description = param.Description;
                    laboratoriumUpdate.Active = param.Active;
                    laboratoriumUpdate.UpdateBy = param.UpdateBy;
                    laboratoriumUpdate.UpdateDate = DateTime.Now;
                    _context.Laboratorium.Update(laboratoriumUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    laboratoriumList = (from branch in _context.Branch
                                        join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                        join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                        where lab.Id == param.Id
                                        select new LaboratoriumResponse
                                        {
                                            Id = lab.Id,
                                            ClinicId = clinic.Id,
                                            ClinicName = clinic.Name,
                                            BranchId = branch.Id,
                                            BranchName = branch.Name,
                                            LaboratoriumName = lab.LaboratoriumName,
                                            Description = lab.Description,
                                            Price = lab.Price,
                                            CreateBy = lab.CreateBy,
                                            CreateDate = lab.CreateDate,
                                            UpdateBy = lab.UpdateBy,
                                            UpdateDate = lab.UpdateDate
                                        }).Take(0).AsNoTracking();

                    return laboratoriumList;
                }


                laboratoriumList = (from branch in _context.Branch
                                    join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                    join lab in _context.Laboratorium on branch.Id equals lab.BranchId
                                    where lab.Id == param.Id
                                    select new LaboratoriumResponse
                                    {
                                        Id = lab.Id,
                                        ClinicId = clinic.Id,
                                        ClinicName = clinic.Name,
                                        BranchId = branch.Id,
                                        BranchName = branch.Name,
                                        LaboratoriumName = lab.LaboratoriumName,
                                        Description = lab.Description,
                                        Price = lab.Price,
                                        CreateBy = lab.CreateBy,
                                        CreateDate = lab.CreateDate,
                                        UpdateBy = lab.UpdateBy,
                                        UpdateDate = lab.UpdateDate
                                    }).Take(1).AsNoTracking();

                return laboratoriumList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateLaboratorium";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return laboratoriumList;
            }

        }
    }
}
