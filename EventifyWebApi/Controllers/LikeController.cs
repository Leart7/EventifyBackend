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
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IEventRepository _eventRepository;

        public LikeController(ILikeRepository likeRepository, UserManager<IdentityUser> userManager, IMapper mapper, IEventRepository eventRepository)
        {
            _likeRepository = likeRepository;
            _userManager = userManager;
            _mapper = mapper;
            _eventRepository = eventRepository;
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetLikes()
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var likes = await _likeRepository.GetAll(authenticatedUserId);
            return Ok(_mapper.Map<List<LikeDto>>(likes));
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateLike([FromBody] CreateLikeRequestDto createLikeRequestDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var eventt = await _eventRepository.GetEvent(createLikeRequestDto.EventId);
            
            if(eventt == null)
            {
                return NotFound("Event does not exist");
            }

            var likeDomainModel = _mapper.Map<Like>(createLikeRequestDto);
            likeDomainModel = await _likeRepository.Create(likeDomainModel, authenticatedUserId);

            return Ok(_mapper.Map<LikeDto>(likeDomainModel));
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteLike([FromRoute] int id)
        {
            var likedEvent = await _likeRepository.GetLike(id);

            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            if(likedEvent == null)
            {
                return NotFound();
            }

            if (authenticatedUserId != likedEvent.UserId)
            {
                return Unauthorized();
            }

            await _likeRepository.Delete(id);

            return NoContent();
        }
    }
}
