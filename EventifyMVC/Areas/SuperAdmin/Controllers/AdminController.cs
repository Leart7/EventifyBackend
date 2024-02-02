using EventifyCommon.Models;
using EventifyCommon.Models.ViewModels;
using EventifyMVC.Areas.SuperAdmin.Repositories;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Criterion;
using System.Xml.Linq;

namespace EventifyMVC.Areas.SuperAdmin.Controllers
{
    [Area("SuperAdmin")]
    [Authorize(Roles = Roles.Role_SuperAdmin)]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminRepository adminRepository, IPermissionRepository permissionRepository, ILogger<AdminController> logger)
        {
            _adminRepository = adminRepository;
            _permissionRepository = permissionRepository;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string? term, string orderBy, int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return RedirectToAction("Index", new { term, orderBy, pageNumber = 1 });
            }

            var viewModel = new AdminIndexVM();

            var users = await _adminRepository.GetAll(term, orderBy, pageNumber);

            viewModel.Users = users.Users;
            viewModel.TotalResults = users.TotalResults;
            viewModel.TotalPages = users.TotalPages;
            viewModel.EmailSortOrder = orderBy == "email_asc" ? "email_dsc" : "email_asc";
            viewModel.FirstNameSortOrder = orderBy == "name_asc" ? "name_dsc" : "name_asc";
            viewModel.LastNameSortOrder = orderBy == "lastname_asc" ? "lastname_dsc" : "lastname_asc";
            viewModel.DateSortOrder = orderBy == "date_asc" ? "date_dsc" : "date_asc";

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string userId, string term, string orderBy, int pageNumber)
        {
            if(String.IsNullOrEmpty(userId))
            {
                return View("_PageNotFound");
            }

            await _adminRepository.RemoveUser(userId);
            TempData["success"] = "Admin removed successfully";
            _logger.LogInformation(message: $"Admin removed by {User.Identity.Name}. Admin Removed: {userId}");
            return RedirectToAction("Index", new { term, orderBy, pageNumber });
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _adminRepository.FindUser(userId);

            if (user == null)
            {
                return View("_PageNotFound");
            }

            var permissions = await _permissionRepository.GetPermissions(userId);

            var viewModel = new EditAdminVM();

            viewModel.User = user;
            viewModel.Permissions = permissions;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(List<Permission> permissions, string userId, string term, string orderBy, int pageNumber)
        {
            var permissionIds = permissions.Select(p => p.Id).ToList();
            var permissionAllowed = permissions.Select(p => p.Allowed).ToList();

            await _permissionRepository.UpdatePermissions(permissionIds, permissionAllowed);
            TempData["success"] = "Admin updated successfully";
            _logger.LogInformation(message: $"Admin updated by {User.Identity.Name}. Admin Updated: {userId}");
            return RedirectToAction("Index", new { term, orderBy, pageNumber });
        }





    }
}
