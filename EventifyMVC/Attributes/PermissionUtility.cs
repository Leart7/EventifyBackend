using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventifyMVC.Attributes
{
    public class PermissionUtility
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionUtility(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IActionResult> CheckPermission(ClaimsPrincipal user, string controller, string action)
        {
            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (user.IsInRole(Roles.Role_Admin) && await _permissionRepository.CheckPermission(userId, controller, action) == false)
            {
                return new ViewResult { ViewName = "_AccessDenied" };
            }

            return null;
        }
    }

}
