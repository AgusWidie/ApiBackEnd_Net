namespace APIRetail.Models.DTO.Request
{
    public class BranchRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public long? CompanyId { get; set; }

        public string Name { get; set; } = "";
    }
}
