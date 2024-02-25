namespace APIClinic.Models.DTOs.Response
{
    public class LogErrorResponse
    {
        public long Id { get; set; }

        public string ServiceName { get; set; } = "";

        public string ErrorDeskripsi { get; set; } = "";

        public DateTime? ErrorDate { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }
    }
}
