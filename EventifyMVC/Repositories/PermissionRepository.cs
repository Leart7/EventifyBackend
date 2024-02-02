using EventifyCommon.Context;
using EventifyCommon.Models;
using EventifyMVC.Enums;
using Microsoft.EntityFrameworkCore;

namespace EventifyMVC.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public PermissionRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<bool> CheckPermission(string userId, string controller, string action)
        {
            var permission = await _db.Permissions
            .Where(p => p.UserId == userId && p.Controller == controller && p.Action == action)
            .FirstOrDefaultAsync();

            if (permission == null)
            {
                return false;
            }
            
            return permission.Allowed;
        }

        public async Task CreateDefaultPermissions(string userId)
        {
            await CreatePermission(userId, EventifyControllers.Category_Controller, Actions.Create_Action, true);
            await CreatePermission(userId, EventifyControllers.Category_Controller, Actions.Update_Action, false);
            await CreatePermission(userId, EventifyControllers.Category_Controller, Actions.Delete_Action, false);

            await CreatePermission(userId, EventifyControllers.Language_Controller, Actions.Create_Action, true);
            await CreatePermission(userId, EventifyControllers.Language_Controller, Actions.Update_Action, false);
            await CreatePermission(userId, EventifyControllers.Language_Controller, Actions.Delete_Action, false);

            await CreatePermission(userId, EventifyControllers.Format_Controller, Actions.Create_Action, true);
            await CreatePermission(userId, EventifyControllers.Format_Controller, Actions.Update_Action, false);
            await CreatePermission(userId, EventifyControllers.Format_Controller, Actions.Delete_Action, false);

            await CreatePermission(userId, EventifyControllers.Currency_Controller, Actions.Create_Action, true);
            await CreatePermission(userId, EventifyControllers.Currency_Controller, Actions.Update_Action, false);
            await CreatePermission(userId, EventifyControllers.Currency_Controller, Actions.Delete_Action, false);

            await CreatePermission(userId, EventifyControllers.Type_Controller, Actions.Create_Action, true);
            await CreatePermission(userId, EventifyControllers.Type_Controller, Actions.Update_Action, false);
            await CreatePermission(userId, EventifyControllers.Type_Controller, Actions.Delete_Action, false);

            //await CreatePermission(userId, EventifyControllers.Report_Controller, Actions.Create_Action, false);
            await CreatePermission(userId, EventifyControllers.Report_Controller, Actions.Update_Action, true);
            //await CreatePermission(userId, EventifyControllers.Report_Controller, Actions.Delete_Action, false);


            //await CreatePermission(userId, EventifyControllers.Event_Controller, Actions.Create_Action, false);
            await CreatePermission(userId, EventifyControllers.Event_Controller, Actions.Update_Action, true);
            //await CreatePermission(userId, EventifyControllers.Event_Controller, Actions.Delete_Action, false);

        }

        public async Task CreatePermission(string userId, string controller, string action, bool allowed)
        {
            _db.Permissions.Add(new Permission
            {
                UserId = userId,
                Controller = controller,
                Action = action,
                Allowed = allowed
            });

            await _db.SaveChangesAsync();
        }

        public async Task<List<Permission>> GetPermissions(string userId)
        {
            return await _db.Permissions.Where(p => p.UserId == userId).ToListAsync();
        }

        public async Task UpdatePermissions(List<int> permissionIds, List<bool> newPermissions)
        {
            if (permissionIds.Count != newPermissions.Count)
            {
                return;
            }

            for (int i = 0; i < permissionIds.Count; i++)
            {
                int id = permissionIds[i];
                bool newPermission = newPermissions[i];

                var permission = await _db.Permissions
                    .Where(p => p.Id == id)
                    .FirstOrDefaultAsync();

                if (permission != null)
                {
                    permission.Allowed = newPermission;
                }
            }

            await _db.SaveChangesAsync(); // Move it outside the loop
        }


    }
}
