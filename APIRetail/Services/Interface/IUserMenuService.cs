using APIRetail.Models.DTO.Request;
using APIRetail.Models.DTO.Response;

namespace APIRetail.Services.Interface
{
    public interface IUserMenuService
    {
        Task<List<UserMenuParentResponse>> GetUserMenuParent(UserMenuParentRequest param, CancellationToken cancellationToken);
        Task<List<UserMenuResponse>> GetUserMenu(UserMenuRequest param, CancellationToken cancellationToken);
        Task<List<CheckUserMenuResponse>> GetCheckUserMenu(CheckUserMenuRequest param, CancellationToken cancellationToken);
    }
}
