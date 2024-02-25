namespace APIRetail.Models.DTO.Request
{
    public class ProfilRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public string Name { get; set; }
    }
}
