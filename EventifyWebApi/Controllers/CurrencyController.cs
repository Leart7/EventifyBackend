using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EventifyWebApi.Repositories;
using EventifyWebApi.DTOs;
using EventifyCommon.Models;

namespace EventifyWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {

        private readonly IFilterRepository<Currency> _repository;
        private readonly IMapper _mapper;

        public CurrencyController(IFilterRepository<Currency> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurrencies()
        {
            var currencies = await _repository.GetAll();
            return Ok(_mapper.Map<List<CurrencyDto>>(currencies));
        }
    }
}