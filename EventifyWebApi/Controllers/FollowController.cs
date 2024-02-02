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
    [Authorize(Roles = "User")]
    public class FollowController : ControllerBase
    {
        private readonly IFollowRepository _followRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public FollowController(IFollowRepository followRepository, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _followRepository = followRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetFollowings()
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var followings = await _followRepository.GetFollows(authenticatedUserId);
            return Ok(_mapper.Map<List<FollowDto>>(followings));
        }

        [HttpGet]
        [Route("suggest/organizers")]
        [AllowAnonymous]
        public async Task<IActionResult> SuggestOrganizers()
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var rawFollowData = await _followRepository.SuggestOrganizers(authenticatedUserId);

            var mappedData = rawFollowData.Select(data =>
            {
                var followDto = _mapper.Map<FollowDto>(data.Item1);
                followDto.FollowersCount = data.Item2;
                return followDto;
            }).ToList();

            return Ok(mappedData);
        }

        [HttpGet]
        [Route("followers/total/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTotalNumberOfFollowers(string userId)
        {
            int number = await _followRepository.GetTotalNumberOfFollowers(userId);
            return Ok(number);
        }


        [HttpPost]
        public async Task<IActionResult> CreateFollow([FromBody] CreateFollowRequestDto createFollowRequestDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var organizer = await _userManager.FindByIdAsync(createFollowRequestDto.FollowedUserId);

            if (organizer == null)
            {
                return NotFound("Organizer does not exist");
            }

            var followDomainModel = _mapper.Map<Follow>(createFollowRequestDto);
            followDomainModel = await _followRepository.CreateFollow(followDomainModel, authenticatedUserId);

            return Ok(_mapper.Map<FollowDto>(followDomainModel));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFollow(int id)
        {
            var follow = await _followRepository.GetFollow(id);

            if(follow == null)
            {
                return NotFound();
            }

            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            if(authenticatedUserId != follow.FollowerId)
            {
                return Unauthorized();
            }

            var unfollowedUser = await _followRepository.DeleteFollow(id);

            return Ok(_mapper.Map<FollowDto>(unfollowedUser));
        }
    }
}
