namespace APIRetail.Models.DTO.Response
{
    public class CheckUserMenuResponse
    {
        public long? UserId { get; set; }

        public long? ProfilId { get; set; }

        public string? ControllerName { get; set; }

        public long? ParentMenuId { get; set; }

        public string? ParentMenuName { get; set; }

        public long? MenuId { get; set; }

        public string? MenuName { get; set; }
    }
}
