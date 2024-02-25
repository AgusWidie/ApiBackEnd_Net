namespace APIClinic.Models.DTOs.Request
{
    public class BranchSearchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? ClinicId { get; set; }

        public string Name { get; set; } = "";
    }
}
