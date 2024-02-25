namespace APIRetail.Models.DTO.Request
{
    public class ProfilAddRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ulong? Active { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }


    }
}
