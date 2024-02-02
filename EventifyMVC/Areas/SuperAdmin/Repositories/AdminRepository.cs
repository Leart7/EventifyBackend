using EventifyCommon.Context;
using EventifyCommon.Models;
using EventifyMVC.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static EventifyMVC.Enums.Enums;

namespace EventifyMVC.Areas.SuperAdmin.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IdentityUser?> FindUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<(List<IdentityUser> Users, int TotalPages, int TotalResults)> GetAll(string? term = null, string? orderBy = null, int pageNumber = 1)
        {
            var usersQueryable = _userManager.Users
                .OfType<ApplicationUser>()
                .AsQueryable();

            var allUsers = await usersQueryable.ToListAsync(); 

            var adminUserIds = await _userManager.GetUsersInRoleAsync(Roles.Role_Admin);

            var adminUsers = allUsers.Where(u => adminUserIds.Any(adminUser => adminUser.Id == u.Id));

            if (!string.IsNullOrEmpty(term))
            {
                adminUsers = adminUsers.Where(u =>
                    u.Email.Contains(term) ||
                    u.FirstName.Contains(term) ||
                    u.LastName.Contains(term) ||
                    u.Id == term);
            }

            if (!string.IsNullOrEmpty(orderBy))
            {
                switch (orderBy.ToLower())
                {
                    case "email_dsc":
                        adminUsers = adminUsers.OrderByDescending(u => u.UserName);
                        break;
                    case "email_asc":
                        adminUsers = adminUsers.OrderBy(u => u.UserName);
                        break;
                    case "name_dsc":
                        adminUsers = adminUsers.OrderByDescending(u => u.FirstName);
                        break;
                    case "name_asc":
                        adminUsers = adminUsers.OrderBy(u => u.FirstName);
                        break;
                    case "lastname_dsc":
                        adminUsers = adminUsers.OrderByDescending(u => u.LastName);
                        break;
                    case "lastname_asc":
                        adminUsers = adminUsers.OrderBy(u => u.LastName);
                        break;
                    case "date_dsc":
                        adminUsers = adminUsers.OrderByDescending(u => u.Created_at);
                        break;
                    case "date_asc":
                        adminUsers = adminUsers.OrderBy(u => u.Created_at);
                        break;
                }
            }

            var totalResults = adminUsers.Count();

            var pageSize = PageSize.Default;
            var totalPages = (int)Math.Ceiling(totalResults / (double)pageSize);

            var skipResults = (pageNumber - 1) * (int)PageSize.Default;
            var resultUsers = adminUsers.Skip(skipResults).Take((int)pageSize).ToList();

            var identityUsers = resultUsers.OfType<IdentityUser>().ToList();

            return (identityUsers, totalPages, totalResults);
        }

        public async Task<IdentityUser> RemoveUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
            {
                return null;
            }

            await _userManager.DeleteAsync(user);
            return user;
        }

        public async Task UpdateRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var existingRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, existingRoles);

            await _userManager.AddToRoleAsync(user, newRole);
        }
    }
}
