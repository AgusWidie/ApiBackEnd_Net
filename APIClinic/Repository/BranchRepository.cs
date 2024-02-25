using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class BranchRepository : IBranch
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public BranchRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<BranchResponse>> GetBranch(BranchSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<BranchResponse>? branchList = null;
            long Page = param.Page - 1;
            try
            {
                if (param.ClinicId != null && (param.Name == null || param.Name == ""))
                {
                    branchList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  where branch.ClinicId == param.ClinicId
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.ClinicId != null && (param.Name != null || param.Name != ""))
                {
                    branchList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  where branch.ClinicId == param.ClinicId && branch.Name.Contains(param.Name)
                                  orderby branch.Name
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).OrderBy(x => x.Name).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return branchList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetBranch";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return branchList;
            }
        }

        public async Task<IEnumerable<BranchResponse>> CreateBranch(BranchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<BranchResponse>? branchList = null;
            Branch branchAdd = new Branch();
            try
            {
                branchAdd.ClinicId = param.ClinicId;
                branchAdd.Name = param.Name;
                branchAdd.Address = param.Address;
                branchAdd.Telp = param.Telp;
                branchAdd.Fax = param.Fax;
                branchAdd.CreateBy = param.CreateBy;
                branchAdd.CreateDate = DateTime.Now;
                _context.Branch.Add(branchAdd);
                await _context.SaveChangesAsync();

                branchList = (from branch in _context.Branch
                              join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                              where branch.ClinicId == param.ClinicId && branch.Name == param.Name
                              select new BranchResponse
                              {
                                  Id = branch.Id,
                                  Name = branch.Name,
                                  ClinicId = clinic.Id,
                                  ClinicName = clinic.Name,
                                  Address = branch.Address,
                                  Telp = branch.Telp,
                                  Fax = branch.Fax,
                                  CreateBy = branch.CreateBy,
                                  CreateDate = branch.CreateDate,
                                  UpdateBy = branch.UpdateBy,
                                  UpdateDate = branch.UpdateDate
                              }).Take(1).AsNoTracking();


                return branchList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateBranch";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return branchList;
            }

        }

        public async Task<IEnumerable<BranchResponse>> UpdateBranch(BranchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<BranchResponse>? branchList = null;
            try
            {
                var branchUpdate = await _context.Branch.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (branchUpdate != null)
                {
                    branchUpdate.ClinicId = param.ClinicId;
                    branchUpdate.Name = param.Name;
                    branchUpdate.Address = param.Address;
                    branchUpdate.Telp = param.Telp;
                    branchUpdate.Fax = param.Fax;
                    branchUpdate.UpdateBy = param.UpdateBy;
                    branchUpdate.UpdateDate = DateTime.Now;
                    _context.Branch.Update(branchUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    branchList = (from branch in _context.Branch
                                  join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                  where branch.Id == param.Id
                                  select new BranchResponse
                                  {
                                      Id = branch.Id,
                                      Name = branch.Name,
                                      ClinicId = clinic.Id,
                                      ClinicName = clinic.Name,
                                      Address = branch.Address,
                                      Telp = branch.Telp,
                                      Fax = branch.Fax,
                                      CreateBy = branch.CreateBy,
                                      CreateDate = branch.CreateDate,
                                      UpdateBy = branch.UpdateBy,
                                      UpdateDate = branch.UpdateDate
                                  }).Take(0).AsNoTracking();

                    return branchList;
                }


                branchList = (from branch in _context.Branch
                              join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                              where branch.Id == param.Id
                              select new BranchResponse
                              {
                                  Id = branch.Id,
                                  Name = branch.Name,
                                  ClinicId = clinic.Id,
                                  ClinicName = clinic.Name,
                                  Address = branch.Address,
                                  Telp = branch.Telp,
                                  Fax = branch.Fax,
                                  CreateBy = branch.CreateBy,
                                  CreateDate = branch.CreateDate,
                                  UpdateBy = branch.UpdateBy,
                                  UpdateDate = branch.UpdateDate
                              }).Take(1).AsNoTracking();

                return branchList;

            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateBranch";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return branchList;
            }

        }
    }
}
