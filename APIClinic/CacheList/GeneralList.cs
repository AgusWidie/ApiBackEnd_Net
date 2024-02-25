using APIClinic.Models.DTOs.Response;
using APIClinic.Models.DTOs.Response.ThreadJob;

namespace APIClinic.CacheList
{
    public class GeneralList
    {
        public static List<AbsenDoctorResponse> _listAbsenDoctor = new List<AbsenDoctorResponse>();

        public static List<BranchResponse> _listBranch = new List<BranchResponse>();

        public static List<ClinicResponse> _listClinic = new List<ClinicResponse>();

        public static List<DoctorResponse> _listDoctor = new List<DoctorResponse>();

        public static List<DrugResponse> _listDrug = new List<DrugResponse>();

        public static List<ExaminationDoctorResponse> _listExaminationDoctor = new List<ExaminationDoctorResponse>();

        public static List<ExaminationLabResponse> _listExaminationLab = new List<ExaminationLabResponse>();

        public static List<LaboratoriumResponse> _listLaboratorium = new List<LaboratoriumResponse>();

        public static List<LogErrorResponse> _listError = new List<LogErrorResponse>();

        public static List<PatientRegistrationResponse> _listPatientRegistration = new List<PatientRegistrationResponse>();

        public static List<PatientRegistrationLabResponse> _listPatientRegistrationLab = new List<PatientRegistrationLabResponse>();

        public static List<ProfilMenuResponse> _listProfilMenu = new List<ProfilMenuResponse>();

        public static List<ProfilResponse> _listProfil = new List<ProfilResponse>();

        public static List<ProfilUserResponse> _listProfilUser = new List<ProfilUserResponse>();

        public static List<ScheduleDoctorResponse> _listScheduleDoctor = new List<ScheduleDoctorResponse>();

        public static List<SpecialistDoctorResponse> _listSpecialDoctor = new List<SpecialistDoctorResponse>();

        public static List<SpecialistResponse> _listSpeciallist = new List<SpecialistResponse>();

        public static List<UserMenuParentResponse> _listUserMenuParent = new List<UserMenuParentResponse>();

        public static List<UserMenuResponse> _listUserMenu = new List<UserMenuResponse>();

        public static List<UsersResponse> _listUser = new List<UsersResponse>();

        public static List<CheckUserMenuResponse> _listCheckUserMenu = new List<CheckUserMenuResponse>();

        public static List<MenuResponse> _listMenu = new List<MenuResponse>();

        public static List<TransactionHeaderPatientResponse> _listTransactionHeaderPatient = new List<TransactionHeaderPatientResponse>();

        public static List<TransactionDetailPatientResponse> _listTransactionDetailPatient = new List<TransactionDetailPatientResponse>();

        public static List<TransactionHeaderPatientLabResponse> _listTransactionHeaderPatientLab = new List<TransactionHeaderPatientLabResponse>();

        public static List<ThreadJobMasterResponse> _listThreadJobMaster = new List<ThreadJobMasterResponse>();

        public static List<ThreadJobMenuResponse> _listThreadJobMenu = new List<ThreadJobMenuResponse>();

        public static List<ThreadJobTransactionResponse> _listThreadJobTransaction = new List<ThreadJobTransactionResponse>();
    }
}
