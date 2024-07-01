using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelLisingAPIV1.Models;
using HotelLisingAPIV1.Models.Configrations;
using AutoMapper;
using HotelLisingAPIV1.DTOs.Countries;
using HotelLisingAPIV1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.Models;
using Microsoft.AspNetCore.OData.Query;

namespace HotelLisingAPIV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {

        private readonly IMapper _mapper;
        private readonly ICountryRepository _countryRepository;

        public CountriesController(IMapper mapper, ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CountryDto>>> GetCountries([FromQuery] QueryParameters queryParameters)
        {
            var countries =  await _countryRepository.GetAllAsync(queryParameters);

            var countriesDto = _mapper.Map<List<CountryDto>>(countries);

            return countriesDto;
        }

        // GET: api/Countries/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DetailedCountryDto>> GetCountry(int id)
        {
            var country = await _countryRepository.GetAsync(id);

            if (country == null)
            {
                return NotFound();
            }

            var countryDto = _mapper.Map<DetailedCountryDto>(country);

            return countryDto;
        }

        // PUT: api/Countries/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutCountry(int id, CountryDto countryDto)
        {
            if (id != countryDto.Id)
            {
                return BadRequest();
            }
            if(! await CountryExists(id))
            {
                return NotFound(id);
            }

            var country = await _countryRepository.GetAsync(id);

            _mapper.Map(countryDto, country);

            await _countryRepository.UpdateAsync(country);

            return NoContent();
        }

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Country>> PostCountry(BaseCountryDto baseCountryDto)
        {
            Country country = _mapper.Map<Country>(baseCountryDto);

            await _countryRepository.AddAsync(country);

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }

        // DELETE: api/Countries/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (! await CountryExists(id))
            {
                return NotFound();
            }

            await _countryRepository.DeleteAsync(id);
            
            return NoContent();
        }

        private async Task<bool> CountryExists(int id)
        {
            return await _countryRepository.Exists(id);
        }
    }
}
