using EventifyCommon.Models;

namespace EventifyMVC.Repositories
{
    public interface IPermissionRepository
    {
        Task CreateDefaultPermissions(string userId);
        Task CreatePermission(string userId, string controller, string action, bool allowed);
        Task UpdatePermissions(List<int> permissionIds, List<bool> newPermissions);
        Task<bool> CheckPermission(string userId, string controller, string action);
        Task<List<Permission>> GetPermissions(string userId);
    }
}
