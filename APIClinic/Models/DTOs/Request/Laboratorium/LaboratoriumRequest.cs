namespace APIClinic.Models.DTOs.Request
{
    public class LaboratoriumRequest
    {
        public long? Id { get; set; }

        public long ClinicId { get; set; }

        public long BranchId { get; set; }

        public string LaboratoriumName { get; set; } = "";

        public long? Price { get; set; }

        public string? Description { get; set; } = "";

        public ulong? Active { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
