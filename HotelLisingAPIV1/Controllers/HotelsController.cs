using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelLisingAPIV1.Models;
using HotelLisingAPIV1.DTOs.Hotels;
using AutoMapper;
using HotelLisingAPIV1.Interfaces;
using Microsoft.AspNetCore.Authorization;
using HotelListingAPI.Models;
using Microsoft.AspNetCore.OData.Query;

namespace HotelLisingAPIV1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IHotelRepository _hotelRepository;

        public HotelsController(IMapper mapper, IHotelRepository hotelRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        // GET: api/Hotels
        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<HotelDto>>> GetHotels(QueryParameters queryParameters)
        {

            var hotels = await _hotelRepository.GetAllAsync(queryParameters);
            
            var hotelsDto = _mapper.Map<List<HotelDto>>(hotels);

            return hotelsDto;
        }

        // GET: api/Hotels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HotelDto>> GetHotel(int id)
        {
            var hotel = await _hotelRepository.GetAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            var hotelDto = _mapper.Map<HotelDto>(hotel);

            return hotelDto;
        }

        // PUT: api/Hotels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutHotel(int id, HotelDto hotelDto)
        {
            if (id != hotelDto.Id)
            {
                return BadRequest();
            }

            if(!await HotelExists(id))
            {
                return NotFound();
            }

            var hotel = await _hotelRepository.GetAsync(id);

            _mapper.Map(hotelDto, hotel);

            await _hotelRepository.UpdateAsync(hotel);

            return NoContent();
        }

        // POST: api/Hotels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<HotelDto>> PostHotel(BaseHotelDto baseHotelDto)
        {
            var hotel = _mapper.Map<Hotel>(baseHotelDto);

            await _hotelRepository.AddAsync(hotel);

            var hotelDto = _mapper.Map<HotelDto>(hotel);

            return CreatedAtAction("GetHotel", new { id = hotelDto.Id }, hotelDto);
        }

        // DELETE: api/Hotels/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (!await HotelExists(id))
            {
                return NotFound();
            }

            await _hotelRepository.DeleteAsync(id);

            return NoContent();
        }

        private async Task<bool> HotelExists(int id)
        {
            return await _hotelRepository.Exists(id);
        }
    }
}
