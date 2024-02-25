namespace APIRetail.Models.DTO.Request
{
    public class ProfilUserUpdateRequest
    {
        public long Id { get; set; }

        public long? ProfilId { get; set; }

        public long? UserId { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
