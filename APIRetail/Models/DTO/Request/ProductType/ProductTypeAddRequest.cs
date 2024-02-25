namespace APIRetail.Models.DTO.Request
{
    public class ProductTypeAddRequest
    {
        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public string? ProductTypeName { get; set; }

        public string? Description { get; set; }

        public ulong? Active { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
