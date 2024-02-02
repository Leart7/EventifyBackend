using Microsoft.AspNetCore.Identity;

namespace EventifyMVC.Areas.SuperAdmin.Repositories
{
    public interface IAdminRepository
    {
        Task<(List<IdentityUser> Users, int TotalPages, int TotalResults)> GetAll(string? term = null, string? orderBy = null, int pageNumber = 1);
        Task<IdentityUser?> FindUser(string userId);
        Task UpdateRole(string userId, string newRole);
        Task<IdentityUser> RemoveUser(string userId);
    }
}
