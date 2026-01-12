using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EMSBackend.Data;
using EMSBackend.DTOs;
using EMSBackend.Models;

namespace EMSBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PowerStationsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public PowerStationsController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PowerStationResponse>>> GetPowerStations(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100)
        {
            var stations = await _context.PowerStations
                .Skip(skip)
                .Take(limit)
                .Select(s => new PowerStationResponse
                {
                    Id = s.Id,
                    Name = s.Name,
                    Location = s.Location,
                    Latitude = s.Latitude,
                    Longitude = s.Longitude,
                    Capacity = s.Capacity,
                    Status = s.Status,
                    Description = s.Description,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync();

            return Ok(stations);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PowerStationResponse>> GetPowerStation(int id)
        {
            var station = await _context.PowerStations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }

            return Ok(new PowerStationResponse
            {
                Id = station.Id,
                Name = station.Name,
                Location = station.Location,
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Capacity = station.Capacity,
                Status = station.Status,
                Description = station.Description,
                CreatedAt = station.CreatedAt,
                UpdatedAt = station.UpdatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<PowerStationResponse>> CreatePowerStation(PowerStationRequest request)
        {
            var station = new PowerStation
            {
                Name = request.Name,
                Location = request.Location,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Capacity = request.Capacity,
                Status = request.Status ?? "active",
                Description = request.Description
            };

            _context.PowerStations.Add(station);
            await _context.SaveChangesAsync();

            var response = new PowerStationResponse
            {
                Id = station.Id,
                Name = station.Name,
                Location = station.Location,
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Capacity = station.Capacity,
                Status = station.Status,
                Description = station.Description,
                CreatedAt = station.CreatedAt,
                UpdatedAt = station.UpdatedAt
            };

            return CreatedAtAction(nameof(GetPowerStation), new { id = station.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PowerStationResponse>> UpdatePowerStation(int id, PowerStationRequest request)
        {
            var station = await _context.PowerStations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }

            station.Name = request.Name;
            station.Location = request.Location;
            station.Latitude = request.Latitude;
            station.Longitude = request.Longitude;
            station.Capacity = request.Capacity;
            if (request.Status != null)
                station.Status = request.Status;
            station.Description = request.Description;
            station.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new PowerStationResponse
            {
                Id = station.Id,
                Name = station.Name,
                Location = station.Location,
                Latitude = station.Latitude,
                Longitude = station.Longitude,
                Capacity = station.Capacity,
                Status = station.Status,
                Description = station.Description,
                CreatedAt = station.CreatedAt,
                UpdatedAt = station.UpdatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePowerStation(int id)
        {
            var station = await _context.PowerStations.FindAsync(id);

            if (station == null)
            {
                return NotFound();
            }

            _context.PowerStations.Remove(station);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
