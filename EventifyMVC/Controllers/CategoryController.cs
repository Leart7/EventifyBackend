using EventifyCommon.Models;
using EventifyCommon.Models.ViewModels;
using EventifyMVC.Attributes;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventifyMVC.Controllers
{
    [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_SuperAdmin}")]
    public class CategoryController : Controller
    {
        private readonly IFilterRepository<Category> _filterRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly PermissionUtility _permissionUtility;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IFilterRepository<Category> filterRepository, IPermissionRepository permissionRepository, ILogger<CategoryController> logger)
        {
            _filterRepository = filterRepository;
            _permissionRepository = permissionRepository;
            _permissionUtility = new PermissionUtility(permissionRepository);
            _logger = logger;
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

            var viewModel = new CategoryIndexVM
            {
                Categories = filters.Filters,
                TotalPages = filters.TotalPages,
                TotalResults = filters.TotalResults,
                TrendingFilter = new TrendingCategory
                {
                    MostTrendingFilter = trendingFilter.MostTrendingFilter,
                    NumberOfEvents = trendingFilter.NumberOfEvents
                },
                UnusedFilter = new TrendingCategory
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
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Category_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category obj)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Category_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                await _filterRepository.Create(obj);
                TempData["success"] = "Category created successfully!";
                _logger.LogInformation($"Category created by {User.Identity.Name}. Category Name: {obj.Name}");
                return RedirectToAction("Index");
            }

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Category_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            var filter = await _filterRepository.FindFilter(id);
            if(filter == null)
            {
                return View("_PageNotFound");
            }
            return View(filter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Category_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                var updatedCategory = await _filterRepository.Update(id, category);
                TempData["success"] = "Category updated successfully!";
                _logger.LogInformation($"Category updated by {User.Identity.Name}. Category Name: {updatedCategory.Name}");
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string name, string orderBy, int pageNumber)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Category_Controller, Actions.Delete_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            var filter = await _filterRepository.Delete(id);
            if (filter == null)
            {
                return NotFound();
            }

            TempData["success"] = "Category deleted successfully!";
            _logger.LogInformation($"Category deleted by {User.Identity.Name}. Category Name: {filter.Name}");
            return RedirectToAction("Index", new { name, orderBy, pageNumber });
        }

    }
}
