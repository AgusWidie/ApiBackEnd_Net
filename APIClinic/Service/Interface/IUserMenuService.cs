using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IUserMenuService
    {
        Task<List<UserMenuParentResponse>> GetUserMenuParent(UserMenuParentRequest param, CancellationToken cancellationToken);
        Task<List<UserMenuResponse>> GetUserMenu(UserMenuRequest param, CancellationToken cancellationToken);
        Task<List<CheckUserMenuResponse>> GetCheckUserMenu(CheckUserMenuRequest param, CancellationToken cancellationToken);
    }
}
