namespace APIClinic.Models.DTOs.Request
{
    public class PatientRegistrationSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string QueueNo { get; set; } = "";

        public DateTime RegistrationDate { get; set; }

        public long? DoctorId { get; set; }

    }
}
