namespace APIRetail.Models.DTO.Request
{
    public class ProfilMenuAddRequest
    {
        public long? ProfilId { get; set; }

        public long? ParentMenuId { get; set; }

        public long? MenuId { get; set; }

        public string CreateBy { get; set; }

        public DateTime? CreateDate { get; set; }

    }
}
