namespace APIClinic.Models.DTOs.Request
{
    public class ProfilUserSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? ClinicId { get; set; }

        public long? BranchId { get; set; }

        public long? ProfilId { get; set; }
    }
}
