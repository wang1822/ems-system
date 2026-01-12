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
    public class MonitoringController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public MonitoringController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceMonitoringResponse>>> GetMonitoringData(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100,
            [FromQuery] int? deviceId = null)
        {
            var query = _context.DeviceMonitoring.AsQueryable();

            if (deviceId.HasValue)
            {
                query = query.Where(dm => dm.DeviceId == deviceId.Value);
            }

            var data = await query
                .OrderByDescending(dm => dm.RecordedAt)
                .Skip(skip)
                .Take(limit)
                .Select(dm => new DeviceMonitoringResponse
                {
                    Id = dm.Id,
                    DeviceId = dm.DeviceId,
                    BatteryLevel = dm.BatteryLevel,
                    Temperature = dm.Temperature,
                    Voltage = dm.Voltage,
                    Current = dm.Current,
                    PowerOutput = dm.PowerOutput,
                    EnergyGenerated = dm.EnergyGenerated,
                    EnergyConsumed = dm.EnergyConsumed,
                    Status = dm.Status,
                    ErrorCode = dm.ErrorCode,
                    RecordedAt = dm.RecordedAt
                })
                .ToListAsync();

            return Ok(data);
        }

        [HttpGet("device/{deviceId}/latest")]
        public async Task<ActionResult<DeviceMonitoringResponse>> GetLatestMonitoringData(int deviceId)
        {
            var data = await _context.DeviceMonitoring
                .Where(dm => dm.DeviceId == deviceId)
                .OrderByDescending(dm => dm.RecordedAt)
                .FirstOrDefaultAsync();

            if (data == null)
            {
                return NotFound();
            }

            return Ok(new DeviceMonitoringResponse
            {
                Id = data.Id,
                DeviceId = data.DeviceId,
                BatteryLevel = data.BatteryLevel,
                Temperature = data.Temperature,
                Voltage = data.Voltage,
                Current = data.Current,
                PowerOutput = data.PowerOutput,
                EnergyGenerated = data.EnergyGenerated,
                EnergyConsumed = data.EnergyConsumed,
                Status = data.Status,
                ErrorCode = data.ErrorCode,
                RecordedAt = data.RecordedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<DeviceMonitoringResponse>> CreateMonitoringData(DeviceMonitoringRequest request)
        {
            var data = new DeviceMonitoring
            {
                DeviceId = request.DeviceId,
                BatteryLevel = request.BatteryLevel,
                Temperature = request.Temperature,
                Voltage = request.Voltage,
                Current = request.Current,
                PowerOutput = request.PowerOutput,
                EnergyGenerated = request.EnergyGenerated,
                EnergyConsumed = request.EnergyConsumed,
                Status = request.Status,
                ErrorCode = request.ErrorCode
            };

            _context.DeviceMonitoring.Add(data);
            await _context.SaveChangesAsync();

            var response = new DeviceMonitoringResponse
            {
                Id = data.Id,
                DeviceId = data.DeviceId,
                BatteryLevel = data.BatteryLevel,
                Temperature = data.Temperature,
                Voltage = data.Voltage,
                Current = data.Current,
                PowerOutput = data.PowerOutput,
                EnergyGenerated = data.EnergyGenerated,
                EnergyConsumed = data.EnergyConsumed,
                Status = data.Status,
                ErrorCode = data.ErrorCode,
                RecordedAt = data.RecordedAt
            };

            return Created($"/api/monitoring/{data.Id}", response);
        }
    }
}
