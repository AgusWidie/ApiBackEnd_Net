namespace APIRetail.Models.DTO.Request
{
    public class ProfilUserAddRequest
    {
        public long? ProfilId { get; set; }

        public long? UserId { get; set; }

        public string? CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

    }
}
