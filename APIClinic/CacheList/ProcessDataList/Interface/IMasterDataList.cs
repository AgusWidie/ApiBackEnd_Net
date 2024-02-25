namespace APIClinic.CacheList.ProcessDataList.Interface
{
    public interface IMasterDataList
    {
        void GetMasterClinicList();
        void GetMasterBranchList();
        void GetMasterDrugList();
        void GetMasterDoctorList();
        void GetMasterSpecialList();
        void GetMasterSpecialListDoctor();
        void GetMasterProfil();
        void GetMasterMenu();
        void GetMasterProfilMenu();
        void GetMasterProfilUser();
        void GetMasterLaboratorium();
        void GetMasterScheduleDoctor();
    }
}
