using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.EntityFrameworkCore;
using static EventifyMVC.Enums.Enums;

namespace EventifyMVC.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db;

        public EventRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Event?> FindEvent(int id)
        {
            var eventt = await _db.Events
            .Include(e => e.Category)
            .Include(e => e.Language)
            .Include(e => e.Format)
            .Include(e => e.Currency)
            .Include(e => e.Type)
            .Include(e => e.Images)
            .Include(e => e.Likes)
            .Include(e => e.Reports)
            .Include(e => e.User)
                .ThenInclude(u => u.Events)
            .Include(e => e.User)
                .ThenInclude(u => u.FollowedUserFollows)
            .FirstOrDefaultAsync(e => e.Id == id);

            if (eventt == null)
            {
                return null;
            }

            return eventt;
        }

        public async Task<(List<Event> Events, int TotalPages, int TotalResults)> GetAll(string? title = null, string? category = null, string? language = null, string? format = null, string? currency = null, string? type = null, string? orderBy = "date_soonest_first", string? status = null, string? pastEvents = "false", string? online = null, int pageNumber = 1)
        {
            var events = _db.Events.Include(e => e.Category).Include(e => e.Language).Include(e => e.Format).Include(e => e.Currency).Include(e => e.Type).Include(e => e.Images).Include(e => e.Likes).Include(e => e.Reports).Include(e => e.User).AsQueryable();

            if (!String.IsNullOrEmpty(title))
            {
                events = events.Where(e => e.Title.ToLower().Contains(title.ToLower()));
            }

            if (!String.IsNullOrEmpty(category))
            {
                events = events.Where(e => e.Category.Name == category);
            }

            if (!String.IsNullOrEmpty(format))
            {
                events = events.Where(e => e.Format.Name == format);
            }

            if (!String.IsNullOrEmpty(language))
            {
                events = events.Where(e => e.Language.Name == language);
            }

            if (!String.IsNullOrEmpty(currency))
            {
                events = events.Where(e => e.Currency.Name == currency);
            }

            if (!String.IsNullOrEmpty(type))
            {
                events = events.Where(e => e.Type.Name == type);
            }


            if (!String.IsNullOrEmpty(status))
            {
                events = events.Where(e => e.Status == status);
            }

            if (online == "true")
            {
                events = events.Where(e => e.Latitude == null);
            }

            if (pastEvents == "true")
            {
                events = events.Where(e => e.StartTime < DateTime.UtcNow);
            }
            else if (pastEvents == "false")
            {
                events = events.Where(e => e.StartTime > DateTime.UtcNow);
            }


            switch (orderBy)
            {
                case "date_dsc":
                    events = events.OrderByDescending(e => e.Created_at);
                    break;
                case "date_asc":
                    events = events.OrderBy(e => e.Created_at);
                    break;
                case "date_soonest_first":
                    events = events.OrderBy(e => e.StartTime);
                    break;
                case "likes_dsc":
                    events = events.OrderByDescending(e => e.Likes.Count);
                    break;
                case "reports_dsc":
                    events = events.OrderByDescending(e => e.Reports.Count);
                    break;
            }


            int totalRecords = await events.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / (double)PageSize.Default);
            if (totalPages == 0)
            {
                totalPages = 1;
            }


            var skipResults = (pageNumber - 1) * (int)PageSize.Default;
            var resultEvents = await events.Skip(skipResults).Take((int)PageSize.Default).ToListAsync();


            var returnedObj = new
            {
                Events = resultEvents,
                TotalPages = totalPages
            };

            return (resultEvents, totalPages, totalRecords);
        }

        public async Task UpdateEventStatus(int id, string newStatus)
        {
            var oldEvent = await _db.Events.FirstOrDefaultAsync(e => e.Id == id);

            oldEvent.Status = newStatus;
            await _db.SaveChangesAsync();
        }

    }
}
