using AuthApi.DTOs;
using AuthApi.Repositories;
using EventifyCommon.Context;
using EventifyCommon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDeleteFollowings _deleteFollowings;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ITokenRepository tokenRepository, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IDeleteFollowings deleteFollowings)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenRepository = tokenRepository;
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _deleteFollowings = deleteFollowings;
        }

        [HttpGet]
        [Route("user/exists/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return NoContent();
            }

            return Conflict();
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            if (!await _roleManager.RoleExistsAsync("user"))
            {
                var userRole = new IdentityRole("user");
                await _roleManager.CreateAsync(userRole);
            }
            var newUser = new ApplicationUser
            {
                Email = registerRequestDto.Email,
                UserName = registerRequestDto.Email,
                FirstName = registerRequestDto.FirstName,
                LastName = registerRequestDto.LastName,
            };

            var identityResult = await _userManager.CreateAsync(newUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, "user");

                return Ok(newUser);
            }

            return BadRequest($"Registration failed: {string.Join(", ", identityResult.Errors.Select(e => e.Description))}");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if(user != null)
            {
                var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

                if (checkPasswordResult)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                        var jwtToken = _tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };

                        return Ok(response);
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCurrentUser()
        {

            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var user = await _userManager.FindByIdAsync(authenticatedUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var response = new
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = (user as ApplicationUser)?.FirstName,
                LastName = (user as ApplicationUser)?.LastName,
                Organization = (user as ApplicationUser)?.Organization,
                Website = (user as ApplicationUser)?.Website,
                Address = (user as ApplicationUser)?.Address,
                City = (user as ApplicationUser)?.City,
                Zip = (user as ApplicationUser)?.Zip,
                OrganizationAddress = (user as ApplicationUser)?.OrganizationAddress,
                OrganizationCity = (user as ApplicationUser)?.OrganizationCity,
                OrganizationZip = (user as ApplicationUser)?.OrganizationZip,
                PhoneNumber = user.PhoneNumber,
                Image_Url = (user as ApplicationUser)?.ImageUrl,


            };

            return Ok(response);
        }

        [HttpGet]
        [Route("user/public/{userId}")]
        public async Task<IActionResult> GetPublicUser([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var response = new PublicUserInformationDto
            {
                FirstName = (user as ApplicationUser)?.FirstName,
                LastName = (user as ApplicationUser)?.LastName,
                ImageUrl = (user as ApplicationUser)?.ImageUrl,
                Bio = (user as ApplicationUser)?.Bio,
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("update/password")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto updateUserPasswordDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var user = await _userManager.FindByIdAsync(authenticatedUserId);

            if (user == null)
            {
                return NotFound();
            }

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, updateUserPasswordDto.OldPassword);

            if (!checkPasswordResult)
            {
                return BadRequest("Password not correct");
            }

            if(updateUserPasswordDto.OldPassword == updateUserPasswordDto.NewPassword)
            {
                return Conflict();
            }

            await _userManager.ChangePasswordAsync(user, updateUserPasswordDto.OldPassword, updateUserPasswordDto.NewPassword);

            var updateResult = await _userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("update/email")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateUserEmail([FromBody] UpdateUserEmailDto updateUserEmailDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var user = await _userManager.FindByIdAsync(authenticatedUserId);

            if (user == null)
            {
                return NotFound();
            }

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, updateUserEmailDto.Password);

            if (!checkPasswordResult)
            {
                return Unauthorized("Password incorrect");
            }

            if(user.Email == updateUserEmailDto.Email)
            {
                return BadRequest("Your new email matches the previous one");
            }

            user.Email = updateUserEmailDto.Email;
            user.UserName = updateUserEmailDto.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if(updateResult.Succeeded)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserRequestDto updateUserRequestDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var user = await _userManager.FindByIdAsync(authenticatedUserId);

            if(user == null)
            {
                return NotFound();
            }

            user.PhoneNumber = updateUserRequestDto.PhoneNumber;

            if (user is ApplicationUser applicationUser)
            {
                applicationUser.FirstName = updateUserRequestDto.FirstName;
                applicationUser.LastName = updateUserRequestDto.LastName;
                applicationUser.Organization = updateUserRequestDto.Organization;
                applicationUser.Website = updateUserRequestDto.Website;
                applicationUser.Address = updateUserRequestDto.Address;
                applicationUser.City = updateUserRequestDto.City;
                applicationUser.Zip = updateUserRequestDto.Zip;
                applicationUser.OrganizationCity = updateUserRequestDto.OrganizationCity;
                applicationUser.OrganizationAddress = updateUserRequestDto.OrganizationAddress;
                applicationUser.OrganizationZip = updateUserRequestDto.OrganizationZip;
            }

            if (updateUserRequestDto.Image != null && user is ApplicationUser applicationUser1)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "UserImages");
                string uniqueFileName = $"{Guid.NewGuid().ToString()}_{updateUserRequestDto.Image.FileName}";

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = Path.Combine(folderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    updateUserRequestDto.Image.CopyTo(fileStream);
                }

                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}";
                applicationUser1.ImageUrl = $"{baseUrl}/UserImages/{uniqueFileName}";
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (updateResult.Succeeded)
            {
                var updatedUserResponse = new UpdatedUserResponseDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = (user as ApplicationUser)?.FirstName,
                    LastName = (user as ApplicationUser)?.LastName,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl = (user as ApplicationUser)?.ImageUrl,
                };

                return Ok(updatedUserResponse);
            }

            return BadRequest();
        }

        [HttpDelete("delete-account")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> DeleteOwnAccount([FromBody] DeleteRequestDto deleteRequestDto)
        {
            var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(authenticatedUserId))
            {
                return BadRequest("Invalid request. Unable to determine the authenticated user.");
            }

            var user = await _userManager.FindByIdAsync(authenticatedUserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, deleteRequestDto.Password);

            if (!checkPasswordResult)
            {
                return BadRequest();
            }

            await _deleteFollowings.RemoveFollowings(authenticatedUserId);
            

            await _userManager.DeleteAsync(user);

            return NoContent();
        }

    }
}
