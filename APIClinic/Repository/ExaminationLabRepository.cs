using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace APIClinic.Repository
{
    public class ExaminationLabRepository : IExaminationLab
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;

        public ExaminationLabRepository(IConfiguration Configuration, clinic_systemContext context, ILogError errorService)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
        }

        public async Task<IEnumerable<ExaminationLabResponse>> GetExaminationLab(ExaminationLabSearchRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ExaminationLabResponse>? examinationLabList = null;
            long Page = param.Page - 1;
            try
            {
                if (param.QueueNo != null && param.QueueNo != "")
                {
                    examinationLabList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                          join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                          where exa.ClinicId == param.ClinicId && exa.BranchId == param.BranchId &&
                                                exa.ExaminationDate >= param.ExaminationDateFrom && exa.ExaminationDate <= param.ExaminationDateTo
                                          select new ExaminationLabResponse
                                          {
                                              Id = exa.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              QueueNo = exa.QueueNo,
                                              Ktpno = pat.Ktpno,
                                              FamilyCardNo = pat.FamilyCardNo,
                                              Name = pat.Name,
                                              Description = exa.Description,
                                              CreateBy = exa.CreateBy,
                                              CreateDate = exa.CreateDate,
                                              UpdateBy = exa.UpdateBy,
                                              UpdateDate = exa.UpdateDate
                                          }).OrderBy(x => x.QueueNo).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                if (param.QueueNo == null || param.QueueNo == "")
                {
                    examinationLabList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                          join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                          where exa.ClinicId == param.ClinicId && exa.BranchId == param.BranchId &&
                                                exa.ExaminationDate >= param.ExaminationDateFrom && exa.ExaminationDate <= param.ExaminationDateTo && param.QueueNo.Contains(pat.QueueNo)
                                          select new ExaminationLabResponse
                                          {
                                              Id = exa.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              QueueNo = exa.QueueNo,
                                              Ktpno = pat.Ktpno,
                                              FamilyCardNo = pat.FamilyCardNo,
                                              Name = pat.Name,
                                              Description = exa.Description,
                                              CreateBy = exa.CreateBy,
                                              CreateDate = exa.CreateDate,
                                              UpdateBy = exa.UpdateBy,
                                              UpdateDate = exa.UpdateDate
                                          }).OrderBy(x => x.QueueNo).AsNoTracking().Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize);
                }

                return examinationLabList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "GetExaminationLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return examinationLabList;
            }
        }

        public async Task<IEnumerable<ExaminationLabResponse>> CreateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ExaminationLabResponse>? examinationLabList = null;
            ExaminationLab examinationLabAdd = new ExaminationLab();
            try
            {
                examinationLabAdd.ClinicId = param.ClinicId;
                examinationLabAdd.BranchId = param.BranchId;
                examinationLabAdd.QueueNo = param.QueueNo;
                examinationLabAdd.ExaminationDate = param.ExaminationDate;
                examinationLabAdd.Description = param.Description;
                examinationLabAdd.CreateBy = param.CreateBy;
                examinationLabAdd.CreateDate = DateTime.Now;
                _context.ExaminationLab.Add(examinationLabAdd);
                await _context.SaveChangesAsync();

                examinationLabList = (from branch in _context.Branch
                                      join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                      join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                      join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                      where exa.ClinicId == param.ClinicId && exa.BranchId == param.BranchId &&
                                            exa.QueueNo == param.QueueNo
                                      select new ExaminationLabResponse
                                      {
                                          Id = exa.Id,
                                          ClinicId = clinic.Id,
                                          ClinicName = clinic.Name,
                                          BranchId = branch.Id,
                                          BranchName = branch.Name,
                                          QueueNo = exa.QueueNo,
                                          Ktpno = pat.Ktpno,
                                          FamilyCardNo = pat.FamilyCardNo,
                                          Name = pat.Name,
                                          Description = exa.Description,
                                          CreateBy = exa.CreateBy,
                                          CreateDate = exa.CreateDate,
                                          UpdateBy = exa.UpdateBy,
                                          UpdateDate = exa.UpdateDate
                                      }).Take(1).AsNoTracking();


                return examinationLabList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "CreateExaminationLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return examinationLabList;
            }

        }

        public async Task<IEnumerable<ExaminationLabResponse>> UpdateExaminationLab(ExaminationLabRequest param, CancellationToken cancellationToken)
        {
            IEnumerable<ExaminationLabResponse>? examinationLabList = null;
            try
            {
                var examinationLabUpdate = await _context.ExaminationLab.Where(x => x.Id == param.Id).AsNoTracking().FirstOrDefaultAsync();
                if (examinationLabUpdate != null)
                {
                    examinationLabUpdate.ExaminationDate = param.ExaminationDate;
                    examinationLabUpdate.QueueNo = param.QueueNo;
                    examinationLabUpdate.Description = param.Description;
                    examinationLabUpdate.UpdateBy = param.UpdateBy;
                    examinationLabUpdate.UpdateDate = DateTime.Now;
                    _context.ExaminationLab.Update(examinationLabUpdate);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    examinationLabList = (from branch in _context.Branch
                                          join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                          join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                          join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                          where exa.Id == param.Id
                                          select new ExaminationLabResponse
                                          {
                                              Id = exa.Id,
                                              ClinicId = clinic.Id,
                                              ClinicName = clinic.Name,
                                              BranchId = branch.Id,
                                              BranchName = branch.Name,
                                              QueueNo = exa.QueueNo,
                                              Ktpno = pat.Ktpno,
                                              FamilyCardNo = pat.FamilyCardNo,
                                              Name = pat.Name,
                                              Description = exa.Description,
                                              CreateBy = exa.CreateBy,
                                              CreateDate = exa.CreateDate,
                                              UpdateBy = exa.UpdateBy,
                                              UpdateDate = exa.UpdateDate
                                          }).Take(1).AsNoTracking();

                    return examinationLabList;
                }


                examinationLabList = (from branch in _context.Branch
                                      join clinic in _context.Clinic on branch.ClinicId equals clinic.Id
                                      join exa in _context.ExaminationLab on branch.Id equals exa.BranchId
                                      join pat in _context.PatientRegistration on exa.QueueNo equals pat.QueueNo
                                      where exa.Id == param.Id
                                      select new ExaminationLabResponse
                                      {
                                          Id = exa.Id,
                                          ClinicId = clinic.Id,
                                          ClinicName = clinic.Name,
                                          BranchId = branch.Id,
                                          BranchName = branch.Name,
                                          QueueNo = exa.QueueNo,
                                          Ktpno = pat.Ktpno,
                                          FamilyCardNo = pat.FamilyCardNo,
                                          Name = pat.Name,
                                          Description = exa.Description,
                                          CreateBy = exa.CreateBy,
                                          CreateDate = exa.CreateDate,
                                          UpdateBy = exa.UpdateBy,
                                          UpdateDate = exa.UpdateDate
                                      }).Take(1).AsNoTracking();

                return examinationLabList;
            }
            catch (Exception ex)
            {
                LogErrorRequest logDataError = new LogErrorRequest();
                logDataError.ServiceName = "UpdateExaminationLab";
                logDataError.ErrorDeskripsi = "Error : " + ex.Message + ", Source : " + ex.StackTrace;
                logDataError.ErrorDate = DateTime.Now;
                logDataError.CreateDate = DateTime.Now;
                logDataError.CreateBy = "System";

                var resultError = _errorService.CreateLogError(logDataError, cancellationToken);

                return examinationLabList;
            }

        }
    }
}
