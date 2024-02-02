using EventifyCommon.Models;
using EventifyCommon.Models.ViewModels;
using EventifyMVC.Attributes;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog.Filters;

namespace EventifyMVC.Controllers
{
    [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_SuperAdmin}")]
    public class FormatController : Controller
    {
        private readonly IFilterRepository<Format> _filterRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<FormatController> _logger;
        private readonly PermissionUtility _permissionUtility;

        public FormatController(IFilterRepository<Format> filterRepository, IPermissionRepository permissionRepository, ILogger<FormatController> logger)
        {
            _filterRepository = filterRepository;
            _permissionRepository = permissionRepository;
            _logger = logger;
            _permissionUtility = new PermissionUtility(permissionRepository);
        }

        public async Task<IActionResult> Index(string? name, string orderBy,int pageNumber=1)
        {
            if(pageNumber<= 0)
            {
                return RedirectToAction("Index" , new {name,orderBy ,pageNumber=1});
            }

            var filters = await _filterRepository.GetAll(name , orderBy, pageNumber);

            var trendingFilter = await _filterRepository.GetMostTrendingFilter();
            var unusedFilter = await _filterRepository.GetMostUnusedFilter();

            var viewModel = new FormatIndexVM
            {
                Formats = filters.Filters,
                TotalPages = filters.TotalPages,
                TotalResults = filters.TotalResults,
                TrendingFilter = new TrendingFormat
                {
                    MostTrendingFilter = trendingFilter.MostTrendingFilter,
                    NumberOfEvents = trendingFilter.NumberOfEvents
                },
                UnusedFilter = new TrendingFormat
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
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Format_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Format obj)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Format_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                await _filterRepository.Create(obj);
                TempData["success"] = "Format created successfully!";
                _logger.LogInformation(message: $"Format created by {User.Identity.Name}. Format Name: {obj.Name}");
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Format_Controller, Actions.Update_Action);

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
        public async Task<IActionResult> Edit(int id, Format format)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Format_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                var updatedFormat = await _filterRepository.Update(id, format);
                TempData["success"] = "Format updated successfully!";
                _logger.LogInformation(message: $"Format updated by {User.Identity.Name}. Format Name: {updatedFormat.Name}");
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string name, string orderBy, int pageNumber)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Format_Controller, Actions.Delete_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            var filter = await _filterRepository.Delete(id);
            if (filter == null)
            {
                return NotFound();
            }

            TempData["success"] = "Format deleted successfully!";
            _logger.LogInformation(message: $"Format deleted by {User.Identity.Name}. Format Name: {filter.Name}");
            return RedirectToAction("Index", new { name, orderBy, pageNumber });
        }

    }
}

