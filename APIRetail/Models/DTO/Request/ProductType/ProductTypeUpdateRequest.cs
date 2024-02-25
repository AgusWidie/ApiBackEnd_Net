namespace APIRetail.Models.DTO.Request
{
    public class ProductTypeUpdateRequest
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public string ProductTypeName { get; set; }

        public string Description { get; set; }

        public ulong? Active { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
