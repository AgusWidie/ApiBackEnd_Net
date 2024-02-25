using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class TransactionHeaderPatientLabService : ITransactionHeaderPatientLabService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ITransactionLab _transHeaderPatientLabRepo;

        public TransactionHeaderPatientLabService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ITransactionLab transHeaderPatientLabRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _transHeaderPatientLabRepo = transHeaderPatientLabRepo;
        }

        public async Task<List<TransactionHeaderPatientLabResponse>> GetTrPatientLabRegistration(TransactionHeaderPatientLabSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listTransactionHeaderPatientLab.Count() > 0)
                {
                    if (param.PaymentType == "" || param.PaymentType == null)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listTransactionHeaderPatientLab.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.TransactionDate >= param.TransactionDateFrom && x.TransactionDate <= param.TransactionDateTo);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listTransactionHeaderPatientLab.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.TransactionDate >= param.TransactionDateFrom && x.TransactionDate <= param.TransactionDateTo && x.PaymentType == param.PaymentType);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _transHeaderPatientLabRepo.GetTrPatientLab(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<TransactionHeaderPatientLabResponse>> CreateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _transHeaderPatientLabRepo.CreateTrHeaderPatientLab(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listTransactionHeaderPatientLab.Add(resultData.First());
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

        public async Task<List<TransactionHeaderPatientLabResponse>> UpdateTrHeaderPatientLab(TransactionHeaderPatientLabRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _transHeaderPatientLabRepo.UpdateTrHeaderPatientLab(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listTransactionHeaderPatientLab.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.TransactionDate = resultData.First().TransactionDate;
                        checkData.TransactionNo = resultData.First().TransactionNo;
                        checkData.ExaminationLabId = resultData.First().ExaminationLabId;
                        checkData.PaymentType = resultData.First().PaymentType;
                        checkData.Bpjsno = resultData.First().Bpjsno;
                        checkData.InsuranceName = resultData.First().InsuranceName;
                        checkData.InsuranceNo = resultData.First().InsuranceNo;
                        checkData.Total = resultData.First().Total;
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
                return null;
            }


        }
    }
}
