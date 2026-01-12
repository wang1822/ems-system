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
    public class EventsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public EventsController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventResponse>>> GetEvents(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100,
            [FromQuery] string? eventType = null,
            [FromQuery] int? deviceId = null,
            [FromQuery] int? powerStationId = null,
            [FromQuery] bool? resolved = null)
        {
            var query = _context.Events.AsQueryable();

            if (!string.IsNullOrEmpty(eventType))
            {
                query = query.Where(e => e.EventType == eventType);
            }

            if (deviceId.HasValue)
            {
                query = query.Where(e => e.DeviceId == deviceId.Value);
            }

            if (powerStationId.HasValue)
            {
                query = query.Where(e => e.PowerStationId == powerStationId.Value);
            }

            if (resolved.HasValue)
            {
                query = query.Where(e => e.Resolved == resolved.Value);
            }

            var events = await query
                .OrderByDescending(e => e.CreatedAt)
                .Skip(skip)
                .Take(limit)
                .Select(e => new EventResponse
                {
                    Id = e.Id,
                    DeviceId = e.DeviceId,
                    PowerStationId = e.PowerStationId,
                    EventType = e.EventType,
                    Severity = e.Severity,
                    Title = e.Title,
                    Description = e.Description,
                    Resolved = e.Resolved,
                    ResolvedAt = e.ResolvedAt,
                    ResolvedBy = e.ResolvedBy,
                    CreatedAt = e.CreatedAt
                })
                .ToListAsync();

            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventResponse>> GetEvent(int id)
        {
            var evt = await _context.Events.FindAsync(id);

            if (evt == null)
            {
                return NotFound();
            }

            return Ok(new EventResponse
            {
                Id = evt.Id,
                DeviceId = evt.DeviceId,
                PowerStationId = evt.PowerStationId,
                EventType = evt.EventType,
                Severity = evt.Severity,
                Title = evt.Title,
                Description = evt.Description,
                Resolved = evt.Resolved,
                ResolvedAt = evt.ResolvedAt,
                ResolvedBy = evt.ResolvedBy,
                CreatedAt = evt.CreatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<EventResponse>> CreateEvent(EventRequest request)
        {
            var evt = new Event
            {
                DeviceId = request.DeviceId,
                PowerStationId = request.PowerStationId,
                EventType = request.EventType,
                Severity = request.Severity,
                Title = request.Title,
                Description = request.Description
            };

            _context.Events.Add(evt);
            await _context.SaveChangesAsync();

            var response = new EventResponse
            {
                Id = evt.Id,
                DeviceId = evt.DeviceId,
                PowerStationId = evt.PowerStationId,
                EventType = evt.EventType,
                Severity = evt.Severity,
                Title = evt.Title,
                Description = evt.Description,
                Resolved = evt.Resolved,
                ResolvedAt = evt.ResolvedAt,
                ResolvedBy = evt.ResolvedBy,
                CreatedAt = evt.CreatedAt
            };

            return CreatedAtAction(nameof(GetEvent), new { id = evt.Id }, response);
        }

        [HttpPut("{id}/resolve")]
        public async Task<ActionResult<EventResponse>> ResolveEvent(int id, [FromBody] string? resolvedBy)
        {
            var evt = await _context.Events.FindAsync(id);

            if (evt == null)
            {
                return NotFound();
            }

            evt.Resolved = true;
            evt.ResolvedAt = DateTime.UtcNow;
            evt.ResolvedBy = resolvedBy;

            await _context.SaveChangesAsync();

            return Ok(new EventResponse
            {
                Id = evt.Id,
                DeviceId = evt.DeviceId,
                PowerStationId = evt.PowerStationId,
                EventType = evt.EventType,
                Severity = evt.Severity,
                Title = evt.Title,
                Description = evt.Description,
                Resolved = evt.Resolved,
                ResolvedAt = evt.ResolvedAt,
                ResolvedBy = evt.ResolvedBy,
                CreatedAt = evt.CreatedAt
            });
        }
    }
}
