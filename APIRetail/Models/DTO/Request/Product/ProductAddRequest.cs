namespace APIRetail.Models.DTO.Request
{
    public class ProductAddRequest
    {
        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? ProductTypeId { get; set; }

        public string? ProductNo { get; set; }

        public string? ProductName { get; set; }

        public long? BuyPrice { get; set; }

        public long? SellPrice { get; set; }

        public string? Description { get; set; }

        public ulong? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
