using EventifyCommon.Models;
using EventifyCommon.Models.ViewModels;
using EventifyMVC.Attributes;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventifyMVC.Controllers
{
    [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_SuperAdmin}")]
    public class TypeController : Controller
    {
        private readonly IFilterRepository<EventifyCommon.Models.Type> _filterRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly PermissionUtility _permissionUtility;

        public TypeController(IFilterRepository<EventifyCommon.Models.Type> filterRepository, IPermissionRepository permissionRepository)
        {
            _filterRepository = filterRepository;
            _permissionRepository = permissionRepository;
            _permissionUtility = new PermissionUtility(permissionRepository);
        }

        public async Task<IActionResult> Index(string? name, string orderBy, int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return RedirectToAction("Index", new { name, orderBy, pageNumber = 1 });
            }

            var filters = await _filterRepository.GetAll(name, orderBy, pageNumber);

            var trendingFilter = await _filterRepository.GetMostTrendingFilter();
            var unusedFilter = await _filterRepository.GetMostUnusedFilter();

            var viewModel = new TypeIndexVM
            {
                Types = filters.Filters,
                TotalPages = filters.TotalPages,
                TotalResults = filters.TotalResults,
                TrendingFilter = new TrendingType
                {
                    MostTrendingFilter = trendingFilter.MostTrendingFilter,
                    NumberOfEvents = trendingFilter.NumberOfEvents
                },
                UnusedFilter = new TrendingType
                {
                    MostTrendingFilter = unusedFilter.MostTrendingFilter,
                    NumberOfEvents = unusedFilter.NumberOfEvents
                }
            };
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var filter = await _filterRepository.FindFilter(id);
            if (filter == null)
            {
                return View("_PageNotFound");
            }

            return View(filter);
        }

        public async Task<IActionResult> Create()
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Type_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventifyCommon.Models.Type obj)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Type_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                await _filterRepository.Create(obj);
                TempData["success"] = "Type created successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Type_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            var filter = await _filterRepository.FindFilter(id);
            if (filter == null)
            {
                return View("_PageNotFound");
            }
            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventifyCommon.Models.Type Type)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Type_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                var updatedType = await _filterRepository.Update(id, Type);
                TempData["success"] = "Type updated successfully!";
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string name, string orderBy, int pageNumber)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Type_Controller, Actions.Delete_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            var filter = await _filterRepository.Delete(id);
            if (filter == null)
            {
                return NotFound();
            }

            TempData["success"] = "Type deleted successfully!";
            return RedirectToAction("Index", new { name, orderBy, pageNumber });
        }

    }
}

