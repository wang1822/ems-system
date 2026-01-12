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
    public class DevicesController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public DevicesController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetDevices(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100,
            [FromQuery] int? powerStationId = null)
        {
            var query = _context.Devices.AsQueryable();

            if (powerStationId.HasValue)
            {
                query = query.Where(d => d.PowerStationId == powerStationId.Value);
            }

            var devices = await query
                .Skip(skip)
                .Take(limit)
                .Select(d => new DeviceResponse
                {
                    Id = d.Id,
                    PowerStationId = d.PowerStationId,
                    Name = d.Name,
                    DeviceType = d.DeviceType,
                    Manufacturer = d.Manufacturer,
                    Model = d.Model,
                    SerialNumber = d.SerialNumber,
                    Specifications = d.Specifications,
                    InstallationDate = d.InstallationDate,
                    WarrantyExpiry = d.WarrantyExpiry,
                    Status = d.Status,
                    CreatedAt = d.CreatedAt,
                    UpdatedAt = d.UpdatedAt
                })
                .ToListAsync();

            return Ok(devices);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DeviceResponse>> GetDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            return Ok(new DeviceResponse
            {
                Id = device.Id,
                PowerStationId = device.PowerStationId,
                Name = device.Name,
                DeviceType = device.DeviceType,
                Manufacturer = device.Manufacturer,
                Model = device.Model,
                SerialNumber = device.SerialNumber,
                Specifications = device.Specifications,
                InstallationDate = device.InstallationDate,
                WarrantyExpiry = device.WarrantyExpiry,
                Status = device.Status,
                CreatedAt = device.CreatedAt,
                UpdatedAt = device.UpdatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<DeviceResponse>> CreateDevice(DeviceRequest request)
        {
            var device = new Device
            {
                PowerStationId = request.PowerStationId,
                Name = request.Name,
                DeviceType = request.DeviceType,
                Manufacturer = request.Manufacturer,
                Model = request.Model,
                SerialNumber = request.SerialNumber,
                Specifications = request.Specifications,
                InstallationDate = request.InstallationDate,
                WarrantyExpiry = request.WarrantyExpiry
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            var response = new DeviceResponse
            {
                Id = device.Id,
                PowerStationId = device.PowerStationId,
                Name = device.Name,
                DeviceType = device.DeviceType,
                Manufacturer = device.Manufacturer,
                Model = device.Model,
                SerialNumber = device.SerialNumber,
                Specifications = device.Specifications,
                InstallationDate = device.InstallationDate,
                WarrantyExpiry = device.WarrantyExpiry,
                Status = device.Status,
                CreatedAt = device.CreatedAt,
                UpdatedAt = device.UpdatedAt
            };

            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<DeviceResponse>> UpdateDevice(int id, DeviceRequest request)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            device.Name = request.Name;
            device.DeviceType = request.DeviceType;
            device.Manufacturer = request.Manufacturer;
            device.Model = request.Model;
            device.Specifications = request.Specifications;
            device.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new DeviceResponse
            {
                Id = device.Id,
                PowerStationId = device.PowerStationId,
                Name = device.Name,
                DeviceType = device.DeviceType,
                Manufacturer = device.Manufacturer,
                Model = device.Model,
                SerialNumber = device.SerialNumber,
                Specifications = device.Specifications,
                InstallationDate = device.InstallationDate,
                WarrantyExpiry = device.WarrantyExpiry,
                Status = device.Status,
                CreatedAt = device.CreatedAt,
                UpdatedAt = device.UpdatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var device = await _context.Devices.FindAsync(id);

            if (device == null)
            {
                return NotFound();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
