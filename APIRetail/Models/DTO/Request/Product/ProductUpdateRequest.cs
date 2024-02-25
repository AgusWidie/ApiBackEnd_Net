namespace APIRetail.Models.DTO.Request
{
    public class ProductUpdateRequest
    {
        public long Id { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public long? ProductTypeId { get; set; }

        public string ProductNo { get; set; }

        public string ProductName { get; set; }

        public long? BuyPrice { get; set; }

        public long? SellPrice { get; set; }

        public string Description { get; set; }

        public ulong? Active { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
