namespace APIClinic.Models.DTOs.Request
{
    public class CheckUserMenuRequest
    {
        public long? UserId { get; set; }

        public long? ProfilId { get; set; }

        public string? ControllerName { get; set; }
    }
}
