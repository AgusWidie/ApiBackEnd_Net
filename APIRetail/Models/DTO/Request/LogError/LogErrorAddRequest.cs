namespace APIRetail.Models.DTO.Request.LogError
{
    public class LogErrorAddRequest
    {
        public string? ServiceName { get; set; }

        public string? ErrorDeskripsi { get; set; }

        public DateTime? ErrorDate { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
