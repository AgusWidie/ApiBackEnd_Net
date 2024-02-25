namespace APIClinic.Models.DTOs.Response
{
    public class ProfilUserResponse
    {
        public long Id { get; set; }

        public long? ProfilId { get; set; }

        public string ProfilName { get; set; } = "";

        public long? UserId { get; set; }

        public string UserName { get; set; } = "";

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
