using EventifyCommon.Models.ViewModels;
using EventifyMVC.Enums;
using EventifyMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventifyMVC.Controllers
{
    [Authorize(Roles = $"{Roles.Role_Admin}, {Roles.Role_SuperAdmin}")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index(string? term, string orderBy, int pageNumber = 1)
        {
            if (pageNumber <= 0)
            {
                return RedirectToAction("Index", new { term, orderBy, pageNumber = 1 });
            }

            var viewModel = new UserIndexVM();

            var users = await _userRepository.GetAll(term, orderBy, pageNumber);

            viewModel.Users = users.Users;
            viewModel.TotalResults = users.TotalResults;
            viewModel.TotalPages = users.TotalPages;
            viewModel.EmailSortOrder = orderBy == "email_asc" ? "email_dsc" : "email_asc";
            viewModel.FirstNameSortOrder = orderBy == "name_asc" ? "name_dsc" : "name_asc";
            viewModel.LastNameSortOrder = orderBy == "lastname_asc" ? "lastname_dsc" : "lastname_asc";
            viewModel.EventsSortOrder = orderBy == "events_asc" ? "events_dsc" : "events_asc";
            viewModel.FollowersSortOrder = orderBy == "followers_asc" ? "followers_dsc" : "followers_asc";
            viewModel.ReportsSortOrder = orderBy == "reports_asc" ? "reports_dsc" : "reports_asc";

            return View(viewModel);
        }

        public async Task<IActionResult> UserEvents(string userId, string title, string? category, string? status, string? online, string? orderBy = "date_soonest_first", string? pastEvents = "false", int pageNumber = 1)
        {
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = "date_soonest_first";
            }

            var user = await _userRepository.FindUser(userId);

            var events = await _userRepository.GetEvents(userId, title, category, orderBy, status, pastEvents, online, pageNumber);

            var viewModel = new UserEventsVM
            {
                User = user,
                Events = events.Events,
                TotalPages = events.TotalPages,
                TotalResults = events.TotalResults,
            };

            return View(viewModel);
        }
    }
}
