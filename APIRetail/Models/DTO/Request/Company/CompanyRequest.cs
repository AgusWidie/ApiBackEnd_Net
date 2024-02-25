namespace APIRetail.Models.DTO.Request
{
    public class CompanyRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public string Name { get; set; }
    }
}
