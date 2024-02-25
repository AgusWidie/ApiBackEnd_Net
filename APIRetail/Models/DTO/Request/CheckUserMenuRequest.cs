namespace APIRetail.Models.DTO.Request
{
    public class CheckUserMenuRequest
    {
        public long? UserId { get; set; }

        public long? ProfilId { get; set; }

        public string? ControllerName { get; set; }
    }
}
