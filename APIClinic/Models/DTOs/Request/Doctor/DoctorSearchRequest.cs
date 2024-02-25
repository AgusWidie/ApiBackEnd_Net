namespace APIClinic.Models.DTOs.Request
{
    public class DoctorSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string? DoctorName { get; set; } = "";

    }
}
