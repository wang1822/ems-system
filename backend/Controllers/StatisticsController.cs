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
    public class StatisticsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public StatisticsController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatisticsResponse>>> GetStatistics(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100,
            [FromQuery] int? powerStationId = null,
            [FromQuery] string? statisticType = null,
            [FromQuery] string? period = null)
        {
            var query = _context.Statistics.AsQueryable();

            if (powerStationId.HasValue)
            {
                query = query.Where(s => s.PowerStationId == powerStationId.Value);
            }

            if (!string.IsNullOrEmpty(statisticType))
            {
                query = query.Where(s => s.StatisticType == statisticType);
            }

            if (!string.IsNullOrEmpty(period))
            {
                query = query.Where(s => s.Period == period);
            }

            var statistics = await query
                .OrderByDescending(s => s.CreatedAt)
                .Skip(skip)
                .Take(limit)
                .Select(s => new StatisticsResponse
                {
                    Id = s.Id,
                    PowerStationId = s.PowerStationId,
                    StatisticType = s.StatisticType,
                    Period = s.Period,
                    PeriodStart = s.PeriodStart,
                    PeriodEnd = s.PeriodEnd,
                    Value = s.Value,
                    Unit = s.Unit,
                    Data = s.Data,
                    CreatedAt = s.CreatedAt
                })
                .ToListAsync();

            return Ok(statistics);
        }

        [HttpPost]
        public async Task<ActionResult<StatisticsResponse>> CreateStatistics(StatisticsRequest request)
        {
            var stat = new Statistics
            {
                PowerStationId = request.PowerStationId,
                StatisticType = request.StatisticType,
                Period = request.Period,
                PeriodStart = request.PeriodStart,
                PeriodEnd = request.PeriodEnd,
                Value = request.Value,
                Unit = request.Unit,
                Data = request.Data
            };

            _context.Statistics.Add(stat);
            await _context.SaveChangesAsync();

            var response = new StatisticsResponse
            {
                Id = stat.Id,
                PowerStationId = stat.PowerStationId,
                StatisticType = stat.StatisticType,
                Period = stat.Period,
                PeriodStart = stat.PeriodStart,
                PeriodEnd = stat.PeriodEnd,
                Value = stat.Value,
                Unit = stat.Unit,
                Data = stat.Data,
                CreatedAt = stat.CreatedAt
            };

            return Created($"/api/statistics/{stat.Id}", response);
        }

        [HttpGet("summary")]
        public async Task<ActionResult<StatisticsSummaryResponse>> GetSummary([FromQuery] int? powerStationId = null)
        {
            var energyQuery = _context.DeviceMonitoring.AsQueryable();

            if (powerStationId.HasValue)
            {
                energyQuery = energyQuery.Where(dm => dm.Device!.PowerStationId == powerStationId.Value);
            }

            var totalGenerated = await energyQuery.SumAsync(dm => dm.EnergyGenerated ?? 0);
            var totalConsumed = await energyQuery.SumAsync(dm => dm.EnergyConsumed ?? 0);

            var eventQuery = _context.Events.AsQueryable();

            if (powerStationId.HasValue)
            {
                eventQuery = eventQuery.Where(e => e.PowerStationId == powerStationId.Value);
            }

            var totalEvents = await eventQuery.CountAsync();
            var unresolvedEvents = await eventQuery.Where(e => !e.Resolved).CountAsync();

            return Ok(new StatisticsSummaryResponse
            {
                EnergyGeneratedTotal = totalGenerated,
                EnergyConsumedTotal = totalConsumed,
                TotalEvents = totalEvents,
                UnresolvedEvents = unresolvedEvents,
                Period = "all_time"
            });
        }
    }
}
