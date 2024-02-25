namespace APIRetail.Models.DTO.Request.LogError
{
    public class LogErrorRequest
    {
        public long Page { get; set; }

        public long PageSize { get; set; }

        public long TotalPageSize { get; set; }

        public string ServiceName { get; set; }

        public string ErrorDeskripsi { get; set; }
    }
}
