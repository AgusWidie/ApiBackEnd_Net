namespace APIClinic.Models.DTOs.Response
{
    public class ProfilMenuResponse
    {
        public long Id { get; set; }

        public long? ProfilId { get; set; }

        public string? ProfilName { get; set; }

        public long? ParentMenuId { get; set; }

        public string? ParentMenuName { get; set; }

        public long? MenuId { get; set; }

        public string? MenuName { get; set; }

        public string CreateBy { get; set; } = "";

        public DateTime? CreateDate { get; set; }

        public string UpdateBy { get; set; } = "";

        public DateTime? UpdateDate { get; set; }
    }
}
