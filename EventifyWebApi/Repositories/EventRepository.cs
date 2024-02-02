using EventifyCommon.Context;
using EventifyCommon.Models;
using EventifyWebApi.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EventifyWebApi.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EventRepository(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Event?> DeleteEvent(int id)
        {
            var existingEvent = await _db.Events.Include(e => e.Images).FirstOrDefaultAsync(e => e.Id == id);
            if (existingEvent == null)
            {
                return null;
            }

            foreach (var image in existingEvent.Images)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", Path.GetFileName(image.ImageUrl));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            _db.Events.Remove(existingEvent);
            await _db.SaveChangesAsync();
            return existingEvent;
        }

        public async Task<(List<Event> Events, int TotalPages)> GetAllEvents(string? name = null, string? category = null, string? language = null, string? currency = null, string? format = null, string? city = null, double? bottomLeftLatitude = null, double? bottomLeftLongitude = null, double? topRightLatitude = null, double? topRightLongitude = null, bool? online = false, bool? free = false, bool? paid = false, string? dateFilter = null, int pageNumber = 1)
        {
            var events = _db.Events.Include(e => e.Category).Include(e => e.Language).Include(e => e.Format).Include(e => e.Currency).Include(e => e.Images).Include(e => e.Tags).Include(e => e.EventAgends).Include(e => e.User).Where(e => e.StartTime > DateTime.Now).AsQueryable();
            if (string.IsNullOrWhiteSpace(name) == false)
            {
                events = events.Where(e => e.Title.Contains(name));
            }
            if(string.IsNullOrWhiteSpace(city) == false)
            {
                events = events.Where(e => e.City == city);
            }
            if(online == true)
            {
                events = events.Where(e => e.Latitude == null);
            }
            if (free == true)
            {
                events = events.Where(e => e.Price == null);
            }
            if (paid == true)
            {
                events = events.Where(e => e.Price != null);
            }
            if (string.IsNullOrWhiteSpace(category) == false)
            {
                events = events.Where(e => e.Category.Name == category);
            }

            if (string.IsNullOrWhiteSpace(language) == false)
            {
                events = events.Where(e => e.Language.Name == language);
            }
            if (string.IsNullOrWhiteSpace(currency) == false)
            {
                events = events.Where(e => e.Currency.Name == currency);
            }
            if (string.IsNullOrWhiteSpace(format) == false)
            {
                events = events.Where(e => e.Format.Name == format);
            }

            if (bottomLeftLatitude.HasValue && bottomLeftLongitude.HasValue && topRightLatitude.HasValue && topRightLongitude.HasValue)
            {
                events = events.Where(e =>
                    e.Latitude.HasValue &&
                    e.Longitude.HasValue &&
                    e.Latitude >= bottomLeftLatitude &&
                    e.Latitude <= topRightLatitude &&
                    e.Longitude >= bottomLeftLongitude &&
                    e.Longitude <= topRightLongitude
                );
            }

            if (!string.IsNullOrWhiteSpace(dateFilter))
            {
                DateTime currentDate = DateTime.Now.Date;

                switch (dateFilter.ToLower())
                {
                    case "today":
                        events = events.Where(e => e.StartTime.Date == currentDate);
                        break;
                    case "tomorrow":
                        events = events.Where(e => e.StartTime.Date == currentDate.AddDays(1));
                        break;
                    case "this-week":
                        events = events.Where(e => e.StartTime.Date >= currentDate && e.StartTime.Date < currentDate.AddDays(7));
                        break;
                    case "this-weekend":
                        DateTime thisSaturday = currentDate.AddDays(DayOfWeek.Saturday - currentDate.DayOfWeek);
                        DateTime thisSunday = thisSaturday.AddDays(1);
                        events = events.Where(e => e.StartTime.Date >= thisSaturday && e.StartTime.Date <= thisSunday);
                        break;
                    case "next-week":
                        events = events.Where(e => e.StartTime.Date >= currentDate.AddDays(7) && e.StartTime.Date < currentDate.AddDays(14));
                        break;
                    case "this-month":
                        events = events.Where(e => e.StartTime.Year == currentDate.Year && e.StartTime.Month == currentDate.Month);
                        break;
                    case "next-month":
                        events = events.Where(e => e.StartTime.Year == currentDate.AddMonths(1).Year && e.StartTime.Month == currentDate.AddMonths(1).Month);
                        break;
                    default:
                        break;
                }
            }

            var totalEvents = await events.CountAsync();
            var pageSize = 2;

            var totalPages = (int)Math.Ceiling((double)totalEvents / pageSize);
            if(totalPages == 0)
            {
                totalPages = 1;
            }


            var skipResults = (pageNumber - 1) * pageSize;

            var resultEvents = await events.Skip(skipResults).Take(pageSize).ToListAsync();

            var returnedObj = new
            {
                Events = resultEvents,
                TotalPages = totalPages
            };

            return (resultEvents, totalPages);
        }

        public async Task<Event> CreateEvent(Event Event, ICollection<IFormFile> images, List<string> tags, List<EventAgend> eventAgends, string userId)
        {
            if (images != null && images.Any())
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                foreach (var image in images)
                {
                    string uniqueFileName = $"{Guid.NewGuid().ToString()}_{image.FileName}";
                    string filePath = Path.Combine(folderPath, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                    var eventImage = new Image
                    {
                        ImageUrl = $"{baseUrl}/Images/{uniqueFileName}"
                    };

                    Event.Images ??= new List<EventifyCommon.Models.Image>();
                    Event.Images.Add(eventImage);
                }
            }

            if (tags != null && tags.Any())
            {
                foreach (var tagName in tags)
                {
                    if (Event.Tags == null || !Event.Tags.Any(t => t.Name == tagName))
                    {
                        var tag = new Tag
                        {
                            Name = tagName
                        };

                        Event.Tags ??= new List<Tag>();
                        Event.Tags.Add(tag);
                    }
                }
            }

            Event.UserId = userId;

            await _db.Events.AddAsync(Event);
            await _db.SaveChangesAsync();

            foreach (var eventAgend in eventAgends)
            {
                if (Event.EventAgends == null || !Event.EventAgends.Any(ea => ea.Title == eventAgend.Title)) 
                {
                    eventAgend.EventId = Event.Id;
                    await _db.EventAgends.AddAsync(eventAgend);
                }
            }

            await _db.SaveChangesAsync();

            await _db.Entry(Event).Reference(e => e.Category).LoadAsync();
            await _db.Entry(Event).Reference(e => e.Language).LoadAsync();
            await _db.Entry(Event).Reference(e => e.Format).LoadAsync();
            await _db.Entry(Event).Reference(e => e.Currency).LoadAsync();

            return Event;
        }

        public async Task<Event?> GetEvent(int id)
        {
            return await _db.Events.Include(e => e.Category).Include(e => e.Language).Include(e => e.Format).Include(e => e.Currency).Include(e => e.Images).Include(e => e.Tags).Include(e => e.EventAgends).Include(e => e.User).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Event?> UpdateEvent(int id, UpdateEventRequestDto eventDto)
        {
            var existingEvent = await _db.Events.Include(e => e.Category).Include(e => e.Language).Include(e => e.Format).Include(e => e.Currency).Include(e => e.Images).Include(e => e.Tags).Include(e => e.EventAgends).FirstOrDefaultAsync(x => x.Id == id);

            if (existingEvent == null)
            {
                return null;
            }

            existingEvent.Title = eventDto.Title ?? existingEvent.Title;
            existingEvent.Description = eventDto.Description ?? existingEvent.Description;
            existingEvent.StartTime = eventDto.StartTime ?? existingEvent.StartTime;
            existingEvent.EndTime = eventDto.EndTime ?? existingEvent.EndTime;
            existingEvent.Latitude = eventDto.Latitude ?? existingEvent.Latitude;
            existingEvent.Longitude = eventDto.Longitude ?? existingEvent.Longitude;
            existingEvent.City = eventDto.City ?? existingEvent.City;
            existingEvent.About = eventDto.About ?? existingEvent.About;
            existingEvent.Price = eventDto.Price ?? existingEvent.Price;
            existingEvent.Capacity = eventDto.Capacity ?? existingEvent.Capacity;
            existingEvent.EndSales = eventDto.EndSales ?? existingEvent.EndSales;
            existingEvent.CategoryId = eventDto.CategoryId ?? existingEvent.CategoryId;
            existingEvent.CurrencyId = eventDto.CurrencyId ?? existingEvent.CurrencyId;
            existingEvent.FormatId = eventDto.FormatId ?? existingEvent.FormatId;
            existingEvent.LanguageId = eventDto.LanguageId ?? existingEvent.LanguageId;
            existingEvent.TypeId = eventDto.TypeId ?? existingEvent.TypeId;
            existingEvent.Organizer = eventDto.Organizer ?? existingEvent.Organizer;

            if(eventDto.Images != null && eventDto.Images.Any()) { 
            foreach (var image in existingEvent.Images)
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", Path.GetFileName(image.ImageUrl));
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            existingEvent.Images = new List<Image>(); 

            foreach (var imageDto in eventDto.Images)
            {
                string uniqueFileName = $"{Guid.NewGuid().ToString()}_{imageDto.FileName}";
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Images", uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageDto.CopyToAsync(fileStream);
                }

                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";

                var eventImage = new Image
                {
                    ImageUrl = $"{baseUrl}/Images/{uniqueFileName}"
                };

                existingEvent.Images.Add(eventImage);
            }
        }

            if (eventDto.EventAgends != null)
            {
                foreach (var eventAgendDto in eventDto.EventAgends)
                {
                    var existingEventAgend = existingEvent.EventAgends.FirstOrDefault(ea => ea.Id == eventAgendDto.Id);

                    if (existingEventAgend != null)
                    {
                        existingEventAgend.Title = eventAgendDto.Title;
                        existingEventAgend.Description = eventAgendDto.Description;
                        existingEventAgend.Speaker = eventAgendDto.Speaker;
                        existingEventAgend.StartTime = eventAgendDto.StartTime;
                        existingEventAgend.EndTime = eventAgendDto.EndTime;

                    }
                }
            }

            if (eventDto.Tags != null)
            {
                existingEvent.Tags = eventDto.Tags.Select(tagName => new Tag { Name = tagName }).ToList();
            }

            existingEvent.Updated_at = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return existingEvent;
        }

        public async Task<(List<Event> Events, int TotalPages)> GetEventsFromFollowings(string userId, int pageNumber = 1)
        {
            var eventsQuery = _db.Follows
                .Where(f => f.FollowerId == userId)
                .SelectMany(f => f.FollowedUser.Events)
                .Include(e => e.Images)
                .Where(e => e.StartTime > DateTime.Today);

            var totalEvents = await eventsQuery.CountAsync();
            var pageSize = 20;

            var totalPages = (int)Math.Ceiling((double)totalEvents / pageSize);
            if (totalPages == 0)
            {
                totalPages = 1;
            }

            var skipResults = (pageNumber - 1) * pageSize;

            var resultEvents = await eventsQuery.Skip(skipResults).Take(pageSize).ToListAsync();

            var returnedObj = new
            {
                Events = resultEvents,
                TotalPages = totalPages
            };

            return (resultEvents, totalPages);
        }

        public async Task<List<Event>> SuggestEvents(string userId, int eventId)
        {
            return await _db.Events.Include(e => e.Category).Include(e => e.Language).Include(e => e.Format).Include(e => e.Currency).Include(e => e.Images).Include(e => e.User).ThenInclude(u => u.FollowerFollows).Where(e => e.UserId == userId).Where(e => e.Id != eventId).Where(e => e.StartTime > DateTime.Now).Take(3).ToListAsync();
        }

        public async Task<(List<Event> Events, int TotalEvents)> GetUpcomingEventsFromUser(string userId)
        {
            var eventsQuery = _db.Events
                .Include(e => e.Category)
                .Include(e => e.Language)
                .Include(e => e.Format)
                .Include(e => e.Currency)
                .Include(e => e.Images)
                .Include(e => e.User)
                .Where(e => e.UserId == userId && e.StartTime > DateTime.Now);

            var upcomingEvents = await eventsQuery.ToListAsync();

            var numberOfEvents = await eventsQuery.CountAsync();

            return (upcomingEvents, numberOfEvents);
        }

        public async Task<(List<Event> Events, int TotalEvents)> GetPastEventsFromUser(string userId)
        {
            var eventsQuery = _db.Events
                .Include(e => e.Category)
                .Include(e => e.Language)
                .Include(e => e.Format)
                .Include(e => e.Currency)
                .Include(e => e.Images)
                .Include(e => e.User)
                .Where(e => e.UserId == userId && e.StartTime < DateTime.Now);

            var upcomingEvents = await eventsQuery.ToListAsync();

            var numberOfEvents = await eventsQuery.CountAsync();

            return (upcomingEvents, numberOfEvents);
        }

        public async Task<int> GetTotalNumberOfEvents(string userId)
        {
            var numberOfEvents = await _db.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Events.Count)
                .FirstOrDefaultAsync();

            return numberOfEvents;
        }
    }

}
