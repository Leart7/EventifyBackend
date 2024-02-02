using EventifyCommon.Models.ViewModels;
using EventifyMVC.Attributes;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Criterion;
using NLog.Filters;

namespace EventifyMVC.Controllers
{
    [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_SuperAdmin}")]
    public class EventController : Controller
    {
        private readonly IEventRepository _eventRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger<EventController> _logger;
        private readonly PermissionUtility _permissionUtility;


        public EventController(IEventRepository eventRepository, IPermissionRepository permissionRepository, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _permissionRepository = permissionRepository;
            _logger = logger;
            _permissionUtility = new PermissionUtility(permissionRepository);
        }

        public async Task<IActionResult> Index(string title, string? category, string? language, string? format, string? currency, string? type, string? status, string? online, string? orderBy = "date_soonest_first", string? pastEvents = "false", int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = "date_soonest_first";
            }

            var events = await _eventRepository.GetAll(title, category, language, format, currency, type, orderBy, status, pastEvents, online, pageNumber);

            var viewModel = new EventIndexVM
            {
                Events = events.Events,
                TotalPages = events.TotalPages,
                TotalResults = events.TotalResults,
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var eventt = await _eventRepository.FindEvent(id);
            if(eventt == null)
            {
                return View("_PageNotFound");
            }

            return View(eventt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string newStatus)
        {
            var eventt = await _eventRepository.FindEvent(id);
            var permissionResult = await _permissionUtility.CheckPermission(User, EventifyControllers.Event_Controller, Actions.Update_Action);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            await _eventRepository.UpdateEventStatus(id, newStatus);
            TempData["success"] = "Event status updated successfully!";
            _logger.LogInformation(message: $"Event status updated by {User.Identity.Name}. Event: {eventt.Id} - {eventt.Title} | Status: {newStatus}");
            return RedirectToAction(actionName: "Details", new { id });
        }
    }
}
