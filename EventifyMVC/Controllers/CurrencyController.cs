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
    public class CurrencyController : Controller
    {
        private readonly IFilterRepository<Currency> _filterRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<CurrencyController> _logger;
        private readonly PermissionUtility _permissionUtility;


        public CurrencyController(IFilterRepository<Currency> filterRepository, IPermissionRepository permissionRepository, ILogger<CurrencyController> logger)
        {
            _filterRepository = filterRepository;
            _permissionRepository = permissionRepository;
            _logger = logger;
            _permissionUtility = new PermissionUtility(permissionRepository);
        }

        public async Task<IActionResult> Index (string? name, string orderBy, int pageNumber = 1)
        {
            if (pageNumber < 0)
            {
                return RedirectToAction("Index", new { name, orderBy, pageNumber = 1 });
            }


            var filters = await _filterRepository.GetAll(name, orderBy, pageNumber);

            var trendingFilter = await _filterRepository.GetMostTrendingFilter();
            var unusedFilter = await _filterRepository.GetMostUnusedFilter();

            var viewModel = new CurrencyIndexVM
            {
                Currencies = filters.Filters,
                TotalPages = filters.TotalPages,
                TotalResults = filters.TotalResults,
                TrendingFilter = new TrendingCurrency
                {
                    MostTrendingFilter = trendingFilter.MostTrendingFilter,
                    NumberOfEvents = trendingFilter.NumberOfEvents
                },
                UnusedFilter = new TrendingCurrency
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
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Currency_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Currency obj)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Currency_Controller, Actions.Create_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                await _filterRepository.Create(obj);
                TempData["success"] = "Currency created successfully!";
                _logger.LogInformation($"Currency created by {User.Identity.Name}. Currency Name: {obj.Name}");
                return RedirectToAction("Index");
            }

            return View();
        }


        public async Task<IActionResult> Edit(int id)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Currency_Controller, Actions.Update_Action);

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
        public async Task<IActionResult> Edit(int id, Currency currency)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Currency_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            if (ModelState.IsValid)
            {
                var updatedCurrency = await _filterRepository.Update(id, currency);
                TempData["success"] = "Currency updated successfully!";
                _logger.LogInformation($"Currency updated by {User.Identity.Name}. Currency Name: {updatedCurrency.Name}");
                return RedirectToAction("Index");
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string name, string orderBy, int pageNumber)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Currency_Controller, Actions.Delete_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            var filter = await _filterRepository.Delete(id);
            if (filter == null)
            {
                return NotFound();
            }

            TempData["success"] = "Currency deleted successfully!";
            _logger.LogInformation(message: $"Currency deleted by {User.Identity.Name}. Currency Name: {filter.Name}");
            return RedirectToAction("Index", new { name, orderBy, pageNumber });
        }
    }
}
