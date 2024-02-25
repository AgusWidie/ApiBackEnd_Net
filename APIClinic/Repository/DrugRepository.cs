using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class DrugRepository : IDrug
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public DrugRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<DrugResponse>> GetDrug(DrugSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DrugResponse>? drugList = null;
            try
            {
                long Page = param.Page - 1;
                if (param.DrugName == null || param.DrugName == "")
                {
                    drugList = (from branch in _context.Branch
                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                join drug in _context.Drug on branch.Id equals drug.BranchId
                                where drug.ClinicId == param.ClinicId && drug.BranchId == param.BranchId
                                select new DrugResponse
                                {
                                    Id = drug.Id,
                                    ClinicId = clinic.Id,
                                    ClinicName = clinic.Name,
                                    BranchId = branch.Id,
                                    BranchName = branch.Name,
                                    DrugName = drug.DrugName,
                                    UnitType = drug.UnitType,
                                    Price = drug.Price,
                                    Stock = drug.Stock,
                                    Active = drug.Active,
                                    CreateBy = drug.CreateBy,
                                    CreateDate = drug.CreateDate,
                                    UpdateBy = drug.UpdateBy,
                                    UpdateDate = drug.UpdateDate
                                }).OrderBy(x => x.DrugName).AsNoTracking().ToList();
                }

                if (param.DrugName != null || param.DrugName != "")
                {
                    drugList = (from branch in _context.Branch
                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                join drug in _context.Drug on branch.Id equals drug.BranchId
                                where drug.ClinicId == param.ClinicId && drug.BranchId == param.BranchId && param.DrugName.Contains(param.DrugName)
                                select new DrugResponse
                                {
                                    Id = drug.Id,
                                    ClinicId = clinic.Id,
                                    ClinicName = clinic.Name,
                                    BranchId = branch.Id,
                                    BranchName = branch.Name,
                                    DrugName = drug.DrugName,
                                    UnitType = drug.UnitType,
                                    Price = drug.Price,
                                    Stock = drug.Stock,
                                    Active = drug.Active,
                                    CreateBy = drug.CreateBy,
                                    CreateDate = drug.CreateDate,
                                    UpdateBy = drug.UpdateBy,
                                    UpdateDate = drug.UpdateDate
                                }).OrderBy(x => x.DrugName).AsNoTracking().ToList();
                }

                var TotalPageSize = Math.Ceiling((decimal)drugList.Count() / (int)param.PageSize);
                param.TotalPageSize = (long)TotalPageSize;
                var result = drugList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return drugList;
            }
        }

        public async Task<IEnumerable<DrugResponse>> CreateDrug(DrugRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DrugResponse>? drugList = null;
            Drug drugAdd = new Drug();
            try
            {
                drugAdd.ClinicId = param.ClinicId;
                drugAdd.BranchId = param.BranchId;
                drugAdd.DrugName = param.DrugName;
                drugAdd.UnitType = param.UnitType;
                drugAdd.Price = param.Price;
                drugAdd.Stock = param.Stock;
                drugAdd.Active = param.Active;
                drugAdd.CreateBy = param.CreateBy;
                drugAdd.CreateDate = DateTime.Now;
                _context.Drug.Add(drugAdd);
                await _context.SaveChangesAsync();

                drugList = (from branch in _context.Branch
                            join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                            join drug in _context.Drug on branch.Id equals drug.BranchId
                            where drug.ClinicId == param.ClinicId && drug.BranchId == param.BranchId && drug.Id == param.Id
                            select new DrugResponse
                            {
                                Id = drug.Id,
                                ClinicId = clinic.Id,
                                ClinicName = clinic.Name,
                                BranchId = branch.Id,
                                BranchName = branch.Name,
                                DrugName = drug.DrugName,
                                UnitType = drug.UnitType,
                                Price = drug.Price,
                                Stock = drug.Stock,
                                Active = drug.Active,
                                CreateBy = drug.CreateBy,
                                CreateDate = drug.CreateDate,
                                UpdateBy = drug.UpdateBy,
                                UpdateDate = drug.UpdateDate
                            }).Take(1).AsNoTracking();


                return drugList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateDrug";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return drugList;
            }

        }

        public async Task<IEnumerable<DrugResponse>> UpdateDrug(DrugRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<DrugResponse>? drugList = null;
            try
            {
                var drugUpdate = await _context.Drug.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (drugUpdate != null)
                {
                    DrugLog drugLog = new DrugLog();
                    drugLog.ClinicId = drugUpdate.ClinicId;
                    drugLog.BranchId = drugUpdate.BranchId;
                    drugLog.DrugName = drugUpdate.DrugName;
                    drugLog.UnitType = drugUpdate.UnitType;
                    drugLog.Price = drugUpdate.Price;
                    drugLog.Stock = drugUpdate.Stock;
                    drugLog.Active = drugUpdate.Active;
                    drugLog.Description = "Before Update Data Drug";
                    drugLog.CreateBy = drugUpdate.CreateBy;
                    drugLog.CreateDate = drugUpdate.CreateDate;
                    _context.DrugLog.Add(drugLog);
                    await _context.SaveChangesAsync();

                    drugUpdate.ClinicId = param.ClinicId;
                    drugUpdate.BranchId = param.BranchId;
                    drugUpdate.DrugName = param.DrugName;
                    drugUpdate.UnitType = param.UnitType;
                    drugUpdate.Price = param.Price;
                    drugUpdate.Stock = param.Stock;
                    drugUpdate.Active = param.Active;
                    drugUpdate.UpdateBy = param.UpdateBy;
                    drugUpdate.UpdateDate = DateTime.Now;
                    _context.Drug.Update(drugUpdate);
                    await _context.SaveChangesAsync();


                }
                else
                {
                    drugList = (from branch in _context.Branch
                                join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                join drug in _context.Drug on branch.Id equals drug.BranchId
                                where drug.ClinicId == param.ClinicId && drug.BranchId == param.BranchId && drug.Id == param.Id
                                select new DrugResponse
                                {
                                    Id = drug.Id,
                                    ClinicId = clinic.Id,
                                    ClinicName = clinic.Name,
                                    BranchId = branch.Id,
                                    BranchName = branch.Name,
                                    DrugName = drug.DrugName,
                                    UnitType = drug.UnitType,
                                    Price = drug.Price,
                                    Stock = drug.Stock,
                                    Active = drug.Active,
                                    CreateBy = drug.CreateBy,
                                    CreateDate = drug.CreateDate,
                                    UpdateBy = drug.UpdateBy,
                                    UpdateDate = drug.UpdateDate
                                }).Take(0).AsNoTracking();

                    return drugList;
                }


                drugList = (from branch in _context.Branch
                            join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                            join drug in _context.Drug on branch.Id equals drug.BranchId
                            where drug.ClinicId == param.ClinicId && drug.BranchId == param.BranchId && drug.Id == param.Id
                            select new DrugResponse
                            {
                                Id = drug.Id,
                                ClinicId = clinic.Id,
                                ClinicName = clinic.Name,
                                BranchId = branch.Id,
                                BranchName = branch.Name,
                                DrugName = drug.DrugName,
                                UnitType = drug.UnitType,
                                Price = drug.Price,
                                Stock = drug.Stock,
                                Active = drug.Active,
                                CreateBy = drug.CreateBy,
                                CreateDate = drug.CreateDate,
                                UpdateBy = drug.UpdateBy,
                                UpdateDate = drug.UpdateDate
                            }).Take(1).AsNoTracking();

                return drugList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateDrug";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return drugList;
            }

        }
    }
}
