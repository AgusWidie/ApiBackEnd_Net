using APIClinic.Models.DTOs.Request;
using APIClinic.Models.DTOs.Response;

namespace APIClinic.Service.Interface
{
    public interface IBranchService
    {
        Task<List<BranchResponse>> GetBranch(BranchSearchRequest param, CancellationToken cancellationToken);
        Task<List<BranchResponse>> CreateBranch(BranchRequest param, CancellationToken cancellationToken);
        Task<List<BranchResponse>> UpdateBranch(BranchRequest param, CancellationToken cancellationToken);
    }
}
