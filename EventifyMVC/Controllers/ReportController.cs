using EventifyCommon.Models.ViewModels;
using EventifyMVC.Attributes;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Criterion;
using System.Xml.Linq;

namespace EventifyMVC.Controllers
{
    [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_SuperAdmin}")]
    public class ReportController : Controller
    {
        private readonly IReportRepository _reportRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<ReportController> _logger;
        private readonly PermissionUtility _permissionUtility;

        public ReportController(IReportRepository reportRepository, IPermissionRepository permissionRepository, ILogger<ReportController> logger)
        {
            _reportRepository = reportRepository;
            _permissionRepository = permissionRepository;
            _logger = logger;
            _permissionUtility = new PermissionUtility(permissionRepository);
        }
        public async Task<IActionResult> Index(string? term = null, string orderBy = "date_dsc", string reviewed = "false", int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return RedirectToAction("Index", new { term, orderBy, reviewed, pageNumber = 1 });
            }

            var reports = await _reportRepository.GetAll(term, orderBy, reviewed, pageNumber);

            var viewModel = new ReportIndexVM
            {
                Reports = reports.Reports,
                TotalPages = reports.TotalPages,
                TotalResults = reports.TotalResults,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateReportStatus(int id, string term, string orderBy, string reviewed, int pageNumber)
        {
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Report_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            await _reportRepository.UpdateReportStatus(id);

            TempData["success"] = "Report reviewed successfully";
            _logger.LogInformation(message: $"Report reviewed by {User.Identity.Name}.");
            return RedirectToAction("Index", new { term, orderBy, reviewed, pageNumber });
        }

    }
}
