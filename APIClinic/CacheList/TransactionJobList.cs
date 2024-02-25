using APIClinic.CacheList.ProcessDataList;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;

namespace APIClinic.CacheList
{
    public interface ITransactionJobList
    {
        void ThreadJobTransactionStart();
        void ThreadJobTransactionStop();
    }

    public class TransactionJobList : TransactionDataList, ITransactionJobList
    {
        public Thread threadTransaction;
        public bool ExecuteThreadTransaction = false;
        public TransactionJobList(IConfiguration Configuration, clinic_systemContext context, ILogError errorService) : base(Configuration, context, errorService)
        {

        }

        public void ThreadJobTransactionStart()
        {
            threadTransaction = new Thread(new ThreadStart(ExecuteThreadJobTransaction));
            threadTransaction.IsBackground = true;
            if (threadTransaction.IsAlive == false)
            {
                threadTransaction.Start();
            }

        }

        public void ThreadJobTransactionStop()
        {
            if (threadTransaction.IsAlive == true)
            {
                threadTransaction.Abort();
            }

        }

        public void ExecuteThreadJobTransaction()
        {
            try
            {
                while (threadTransaction.IsAlive)
                {
                    if (ExecuteThreadTransaction == false)
                    {
                        ExecuteThreadTransaction = true;

                        if (GeneralList._listThreadJobTransaction.Count() <= 0)
                        {
                            ThreadJobTransactionResponse addData = new ThreadJobTransactionResponse();
                            addData.JobName = "JobTransactionDataList";
                            addData.StartTime = DateTime.Now;
                            addData.EndTime = null;
                            addData.Description = "Process ExecuteThreadJobMaster.";
                            GeneralList._listThreadJobTransaction.Add(addData);
                        }
                        else
                        {
                            var checkData = GeneralList._listThreadJobTransaction.Where(x => x.JobName == "JobTransactionDataList").FirstOrDefault();
                            if (checkData != null)
                            {
                                checkData.JobName = "JobTransactionDataList";
                                checkData.StartTime = DateTime.Now;
                                checkData.EndTime = null;
                                checkData.Description = "Process ExecuteThreadJobTransaction.";
                            }
                        }

                        GetAbsenDoctor();
                        GetExaminationDoctor();
                        GetExaminationLab();
                        GetPatientRegistrationLab();
                        GetPatientRegistration();
                        GetTransactionHeaderPatientRegistration();
                        GetTransactionDetailPatientRegistration();
                        GetTransactionHeaderPatientLab();

                        var checkDataUpdate = GeneralList._listThreadJobTransaction.Where(x => x.JobName == "JobTransactionDataList").FirstOrDefault();
                        if (checkDataUpdate != null)
                        {
                            checkDataUpdate.JobName = "JobTransactionDataList";
                            checkDataUpdate.EndTime = DateTime.Now;
                            checkDataUpdate.Description = "Successfully ExecuteThreadJobTransaction.";
                        }

                        ExecuteThreadTransaction = false;
                    }

                    Thread.Sleep(15000);
                }

            }
            catch (Exception ex)
            {
                Thread.Sleep(100);
            }
        }
    }
}
