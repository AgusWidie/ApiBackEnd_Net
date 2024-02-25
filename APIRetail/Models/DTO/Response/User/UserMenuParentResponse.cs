namespace APIRetail.Models.DTO.Response
{
    public class UserMenuParentResponse
    {
        public long? CompanyId { get; set; }

        public string? CompanyName { get; set; }

        public long? BranchId { get; set; }

        public string? BranchName { get; set; }

        public long? UserId { get; set; }

        public string? UserName { get; set; }

        public long? ProfilId { get; set; }

        public string? ProfilName { get; set; }

        public long? ParentMenuId { get; set; }

        public string? ParentMenuName { get; set; }

        public int? Sort { get; set; }
    }
}
