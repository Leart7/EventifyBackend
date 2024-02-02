using AutoMapper;
using EventifyCommon.Models;
using EventifyWebApi.DTOs;
using EventifyWebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventifyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClosedAccountController : ControllerBase
    {
        private readonly IClosedAccount _closedAccount;
        private readonly IMapper _mapper;

        public ClosedAccountController(IClosedAccount closedAccount, IMapper mapper)
        {
            _closedAccount = closedAccount;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetClosedAccountReasons()
        {
            var closedAccountReasons = await _closedAccount.GetClosedAccountReasons();
            return Ok(_mapper.Map<List<ClosedAccountReasonDto>>(closedAccountReasons));
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateClosedAccount(CreateClosedAccountRequestDto createClosedAccountRequestDto)
        {
            var closedAccountReason = await _closedAccount.GetClosedAccountReason(createClosedAccountRequestDto.ClosedAccountReasonId);

            if(closedAccountReason == null)
            {
                return NotFound();
            }

            if(closedAccountReason.Name == "Other" && createClosedAccountRequestDto.Description == null)
            {
                return BadRequest();
            }

            var closedAccountDomainModel = _mapper.Map<ClosedAccount>(createClosedAccountRequestDto);

            closedAccountDomainModel = await _closedAccount.Create(closedAccountDomainModel);

            return Ok(_mapper.Map<ClosedAccountDto>(closedAccountDomainModel));
        }
    }
}
