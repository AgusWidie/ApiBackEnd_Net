namespace APIClinic.CacheList.ProcessDataList.Interface
{
    public interface ITransactionDataList
    {
        void GetAbsenDoctor();
        void GetExaminationDoctor();
        void GetExaminationLab();
        void GetPatientRegistrationLab();
        void GetPatientRegistration();
        void GetTransactionHeaderPatientRegistration();
        void GetTransactionDetailPatientRegistration();
        void GetTransactionHeaderPatientLab();
    }
}
