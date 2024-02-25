namespace APIClinic.Models.DTOs.Response
{
    public class UserMenuResponse
    {
        public long? ClinicId { get; set; }

        public long? BranchId { get; set; }

        public long? UserId { get; set; }

        public string? UserName { get; set; }

        public long? ProfilId { get; set; }

        public string? ProfilName { get; set; }

        public long? ParentMenuId { get; set; }

        public string? ParentMenuName { get; set; }

        public long? MenuId { get; set; }

        public string? MenuName { get; set; }

        public int? Sort { get; set; }
    }
}
