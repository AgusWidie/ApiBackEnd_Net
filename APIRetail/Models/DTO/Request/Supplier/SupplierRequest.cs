namespace APIRetail.Models.DTO.Request
{
    public class SupplierRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? CompanyId { get; set; }

        public long? BranchId { get; set; }

        public string? SupplierName { get; set; }

    }
}
