using AutoMapper;
using EventifyCommon.Models;
using EventifyWebApi.DTOs;
using EventifyWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventifyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _repository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public EventController(IEventRepository repository, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents([FromQuery] string? name, [FromQuery] string? category, [FromQuery] string? language, [FromQuery] string? currency, [FromQuery] string? format, [FromQuery] string? city,[FromQuery] double? bottomLeftLatitude, [FromQuery] double? bottomLeftLongitude, [FromQuery] double? topRightLatitude, [FromQuery] double? topRightLongitude, [FromQuery] bool? online, [FromQuery] bool? free, [FromQuery] bool? paid, [FromQuery] string? dateFilter, [FromQuery] int pageNumber)
        {
            var result = await _repository.GetAllEvents(name, category, language, currency, format, city, bottomLeftLatitude, bottomLeftLongitude, topRightLatitude, topRightLongitude, online, free, paid, dateFilter, pageNumber);

            var events = result.Events;
            var totalPages = result.TotalPages;

            return Ok(new { Events = _mapper.Map<List<EventDto>>(events), totalPages });
        }

        [HttpGet]
        [Route("events/followings")]
        public async Task<IActionResult> GetEventsFromFollowings([FromQuery] int pageNumber)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var result = await _repository.GetEventsFromFollowings(authenticatedUserId, pageNumber);

            var events = result.Events;
            var totalPages = result.TotalPages;

            return Ok(new { Events = _mapper.Map<List<EventDto>>(events), totalPages });
        }

        [HttpGet]
        [Route("events/suggest/{userId}/{eventId}")]
        public async Task<IActionResult> SuggestEvents([FromRoute] string userId, [FromRoute] int eventId)
        {
            var events = await _repository.SuggestEvents(userId, eventId);
            return Ok(_mapper.Map<List<EventDto>>(events));
        }

        [HttpGet]
        [Route("events/past/{userId}")]
        public async Task<IActionResult> GetPastEvents([FromRoute] string userId)
        {
            var (events, totalEvents) = await _repository.GetPastEventsFromUser(userId);

            var responseDto = new
            {
                Events = _mapper.Map<List<EventDto>>(events),
                TotalEvents = totalEvents
            };

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("events/upcoming/{userId}")]
        public async Task<IActionResult> GetUpcomingEvents([FromRoute] string userId)
        {
            var (events, totalEvents) = await _repository.GetUpcomingEventsFromUser(userId);

            var responseDto = new
            {
                Events = _mapper.Map<List<EventDto>>(events),
                TotalEvents = totalEvents
            };

            return Ok(responseDto);
        }


        [HttpGet]
        [Route("events/total/{userId}")]
        public async Task<IActionResult> GetTotalEvents([FromRoute] string userId)
        {
            int number = await _repository.GetTotalNumberOfEvents(userId);
            return Ok(number);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> DeleteEvent([FromRoute] int id)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var existingEvent = await _repository.DeleteEvent(id);

            if (existingEvent == null)
            {
                return NotFound();
            }

            if(!isAdmin && authenticatedUserId != existingEvent.UserId)
            {
                return Unauthorized();
            }
            
            var deletedEvent = _mapper.Map<EventDto>(existingEvent);
            return Ok(deletedEvent);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEventById([FromRoute] int id)
        {
            var eventDomain = await _repository.GetEvent(id);

            if (eventDomain == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EventDto>(eventDomain));
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventRequestDto eventDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var eventDomainModel = _mapper.Map<Event>(eventDto);
            var eventAgendsDomainModel = _mapper.Map<List<EventAgend>>(eventDto.EventAgends);
          

            eventDomainModel = await _repository.CreateEvent(eventDomainModel, eventDto.Images, eventDto.Tags, eventAgendsDomainModel, authenticatedUserId);

            var returnedNewEvent = _mapper.Map<EventDto>(eventDomainModel);

            return CreatedAtAction(nameof(GetEventById), new { id = eventDomainModel.Id }, returnedNewEvent);  
        }

        [HttpPut]
        [Route("{id}")]
        public async Task <IActionResult> UpdateEvent([FromRoute]int id, [FromForm] UpdateEventRequestDto eventDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var eventDomainModel = _mapper.Map<Event>(eventDto);

            eventDomainModel =  await _repository.UpdateEvent(id, eventDto);

            if (eventDomainModel == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EventDto>(eventDomainModel));
        }
    }
}
