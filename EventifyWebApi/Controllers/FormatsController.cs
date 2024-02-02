using AutoMapper;
using EventifyCommon.Models;
using EventifyWebApi.DTOs;
using EventifyWebApi.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventifyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FormatsController : ControllerBase
    {
        private readonly IFilterRepository<Format> _repository;
        private readonly IMapper _mapper;

        public FormatsController(IFilterRepository<Format> filterRepository, IMapper mapper) {
            _repository = filterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetFormats()
        {
            var formats = await _repository.GetAll();
            return Ok(_mapper.Map<List<FormatDto>>(formats));
        }

    }
}
