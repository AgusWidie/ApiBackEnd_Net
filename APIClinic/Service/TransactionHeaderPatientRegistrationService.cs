using APIClinic.CacheList;
using APIClinic.Helper;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;
using APIClinic.Service.Interface;

namespace APIClinic.Service
{
    public class TransactionHeaderPatientRegistrationService : ITransactionHeaderPatientService
    {
        public readonly IConfiguration _configuration;
        public readonly clinic_systemContext _context;
        public readonly ILogError _errorService;
        public readonly ITransactionHeaderPatient _transHeaderPatientRepo;
        public readonly ITransactionDetailPatient _transDetailPatientRepo;

        public TransactionHeaderPatientRegistrationService(IConfiguration Configuration, clinic_systemContext context, ILogError errorService, ITransactionHeaderPatient transHeaderPatientRepo, ITransactionDetailPatient transDetailPatientRepo)
        {
            _configuration = Configuration;
            _context = context;
            _errorService = errorService;
            _transHeaderPatientRepo = transHeaderPatientRepo;
            _transDetailPatientRepo = transDetailPatientRepo;
        }

        public async Task<List<TransactionHeaderPatientResponse>> GetTrPatientRegistration(TransactionHeaderPatientSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listTransactionHeaderPatient.Count() > 0)
                {
                    if (param.PaymentType == "" || param.PaymentType == null)
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listTransactionHeaderPatient.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.TransactionDate >= param.TransactionDateFrom && x.TransactionDate <= param.TransactionDateTo);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }
                    else
                    {
                        long Page = param.Page - 1;
                        var resultList = GeneralList._listTransactionHeaderPatient.Where(x => x.ClinicId == param.ClinicId && x.BranchId == param.BranchId && x.TransactionDate >= param.TransactionDateFrom && x.TransactionDate <= param.TransactionDateTo && x.PaymentType == param.PaymentType);
                        var TotalPageSize = Math.Ceiling((decimal)resultList.Count() / (int)param.PageSize);
                        param.TotalPageSize = (long)TotalPageSize;
                        return resultList.Skip((int)Page * (int)param.PageSize).Take((int)param.PageSize).ToList();
                    }

                }
                else
                {
                    var resultList = await _transHeaderPatientRepo.GetTrPatientRegistration(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<TransactionHeaderPatientResponse>> CreateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _transHeaderPatientRepo.CreateTrHeaderPatient(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    GeneralList._listTransactionHeaderPatient.Add(resultData.First());
                    var transNo = new TransactionDetailPatientSearchRequest()
                    {
                        TransactionNo = param.TransactionNo
                    };
                    var resulDetail = await _transDetailPatientRepo.GetTrDetailPatientRegistration(transNo, cancellationToken);
                    GeneralList._listTransactionDetailPatient.AddRange(resulDetail);
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

        public async Task<List<TransactionHeaderPatientResponse>> UpdateTrHeaderPatient(TransactionHeaderPatientRequest param, CancellationToken cancellationToken)
        {
            try
            {
                var resultData = await _transHeaderPatientRepo.UpdateTrHeaderPatient(param, cancellationToken);
                if (resultData.Count() > 0)
                {
                    var checkData = GeneralList._listTransactionHeaderPatient.Where(x => x.Id == param.Id).FirstOrDefault();
                    if (checkData != null)
                    {
                        checkData.TransactionDate = resultData.First().TransactionDate;
                        checkData.TransactionNo = resultData.First().TransactionNo;
                        checkData.ExaminationDoctorId = resultData.First().ExaminationDoctorId;
                        checkData.PaymentType = resultData.First().PaymentType;
                        checkData.Bpjsno = resultData.First().Bpjsno;
                        checkData.InsuranceName = resultData.First().InsuranceName;
                        checkData.InsuranceNo = resultData.First().InsuranceNo;
                        checkData.Total = resultData.First().Total;
                        checkData.UpdateBy = resultData.First().UpdateBy;
                        checkData.UpdateDate = DateTime.Now;

                        var transNo = new TransactionDetailPatientSearchRequest()
                        {
                            TransactionNo = param.TransactionNo
                        };
                        var resulDetail = await _transDetailPatientRepo.GetTrDetailPatientRegistration(transNo, cancellationToken);
                        if (resulDetail != null)
                        {
                            var checkDataDetail = GeneralList._listTransactionDetailPatient.Where(x => x.Id == param.Id).FirstOrDefault();
                            if (checkDataDetail != null)
                            {
                                var drug = GeneralList._listDrug.Where(x => x.Id == resulDetail.First().DrugId).FirstOrDefault();
                                checkDataDetail.TransactionNo = resulDetail.First().TransactionNo;
                                checkDataDetail.DrugId = resulDetail.First().DrugId;
                                checkDataDetail.DrugName = drug.DrugName;
                                checkDataDetail.UnitType = resulDetail.First().UnitType;
                                checkDataDetail.Price = resulDetail.First().Price;
                                checkDataDetail.Qty = resulDetail.First().Qty;
                                checkDataDetail.Subtotal = resulDetail.First().Subtotal;
                                checkDataDetail.UpdateBy = resulDetail.First().CreateBy;
                                checkDataDetail.UpdateDate = DateTime.Now;
                            }
                        }

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

        public async Task<List<TransactionDetailPatientResponse>> GetTrDetailPatientRegistration(TransactionDetailPatientSearchRequest param, CancellationToken cancellationToken)
        {

            try
            {
                GarbageCollector.GarbageCollection();
                if (GeneralList._listTransactionDetailPatient.Count() > 0)
                {
                    var resultList = GeneralList._listTransactionDetailPatient.Where(x => x.TransactionNo == param.TransactionNo);
                    return resultList.ToList();

                }
                else
                {
                    var resultList = await _transDetailPatientRepo.GetTrDetailPatientRegistration(param, cancellationToken);
                    return resultList.ToList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
