using APIClinic.CacheList.ProcessDataList;
using APIClinic.Models.Database;
using APIClinic.Models.DTOs.Response;
using APIClinic.Repository.IRepository;

namespace APIClinic.CacheList
{

    public interface IMasterJobList
    {
        void ThreadJobMasterStart();
        void ThreadJobMasterStop();
    }

    public class MasterJobList : MasterDataList, IMasterJobList
    {
        public Thread threadMaster;
        public Thread threadTransaction;
        public bool ExecuteThreadMaster = false;

        public MasterJobList(IConfiguration Configuration, clinic_systemContext context, ILogError errorService) : base(Configuration, context, errorService)
        {

        }

        public void ThreadJobMasterStart()
        {
            threadMaster = new Thread(new ThreadStart(ExecuteThreadJobMaster));
            threadMaster.IsBackground = true;
            if (threadMaster.IsAlive == false)
            {
                threadMaster.Start();
            }

        }

        public void ThreadJobMasterStop()
        {
            if (threadMaster.IsAlive == true)
            {
                threadMaster.Abort();

            }

        }


        public void ExecuteThreadJobMaster()
        {
            try
            {
                while (threadMaster.IsAlive)
                {
                    if (ExecuteThreadMaster == false)
                    {
                        ExecuteThreadMaster = true;

                        if (GeneralList._listThreadJobMaster.Count() <= 0)
                        {
                            ThreadJobMasterResponse addData = new ThreadJobMasterResponse();
                            addData.JobName = "JobMasterDataList";
                            addData.StartTime = DateTime.Now;
                            addData.EndTime = null;
                            addData.Description = "Process ExecuteThreadJobMaster.";
                            GeneralList._listThreadJobMaster.Add(addData);
                        }
                        else
                        {
                            var checkData = GeneralList._listThreadJobMaster.Where(x => x.JobName == "JobMasterDataList").FirstOrDefault();
                            if (checkData != null)
                            {
                                checkData.JobName = "JobMasterDataList";
                                checkData.StartTime = DateTime.Now;
                                checkData.EndTime = null;
                                checkData.Description = "Process ExecuteThreadJobMaster.";
                            }
                        }

                        GetMasterClinicList();
                        GetMasterBranchList();
                        GetMasterDrugList();
                        GetMasterDoctorList();
                        GetMasterSpecialList();
                        GetMasterSpecialListDoctor();
                        GetMasterProfil();
                        GetMasterMenu();
                        GetMasterProfilMenu();
                        GetMasterProfilUser();
                        GetMasterLaboratorium();
                        GetMasterScheduleDoctor();

                        var checkDataUpdate = GeneralList._listThreadJobMaster.Where(x => x.JobName == "JobMasterDataList").FirstOrDefault();
                        if (checkDataUpdate != null)
                        {
                            checkDataUpdate.JobName = "JobMasterDataList";
                            checkDataUpdate.EndTime = DateTime.Now;
                            checkDataUpdate.Description = "Successfully ExecuteThreadJobMaster.";
                        }

                        ExecuteThreadMaster = false;
                    }

                    Thread.Sleep(15000);
                }

            }
            catch (Exception ex)
            {
                var checkDataUpdate = GeneralList._listThreadJobMaster.Where(x => x.JobName == "JobMasterDataList").FirstOrDefault();
                if (checkDataUpdate != null)
                {
                    checkDataUpdate.Description = "Error ExecuteThreadJobMaster : " + ex.Message;
                }

                ExecuteThreadMaster = false;
                Thread.Sleep(100);
            }
        }
    }
}
