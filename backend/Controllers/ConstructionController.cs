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
    public class ConstructionController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public ConstructionController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConstructionProcessResponse>>> GetConstructionProcesses(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100,
            [FromQuery] int? powerStationId = null,
            [FromQuery] string? status = null)
        {
            var query = _context.ConstructionProcesses.AsQueryable();

            if (powerStationId.HasValue)
            {
                query = query.Where(cp => cp.PowerStationId == powerStationId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(cp => cp.Status == status);
            }

            var processes = await query
                .OrderByDescending(cp => cp.CreatedAt)
                .Skip(skip)
                .Take(limit)
                .Select(cp => new ConstructionProcessResponse
                {
                    Id = cp.Id,
                    PowerStationId = cp.PowerStationId,
                    Phase = cp.Phase,
                    Status = cp.Status,
                    StartDate = cp.StartDate,
                    EndDate = cp.EndDate,
                    ExpectedCompletion = cp.ExpectedCompletion,
                    ResponsiblePerson = cp.ResponsiblePerson,
                    Description = cp.Description,
                    Documents = cp.Documents,
                    CreatedAt = cp.CreatedAt,
                    UpdatedAt = cp.UpdatedAt
                })
                .ToListAsync();

            return Ok(processes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConstructionProcessResponse>> GetConstructionProcess(int id)
        {
            var process = await _context.ConstructionProcesses.FindAsync(id);

            if (process == null)
            {
                return NotFound();
            }

            return Ok(new ConstructionProcessResponse
            {
                Id = process.Id,
                PowerStationId = process.PowerStationId,
                Phase = process.Phase,
                Status = process.Status,
                StartDate = process.StartDate,
                EndDate = process.EndDate,
                ExpectedCompletion = process.ExpectedCompletion,
                ResponsiblePerson = process.ResponsiblePerson,
                Description = process.Description,
                Documents = process.Documents,
                CreatedAt = process.CreatedAt,
                UpdatedAt = process.UpdatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<ConstructionProcessResponse>> CreateConstructionProcess(ConstructionProcessRequest request)
        {
            var process = new ConstructionProcess
            {
                PowerStationId = request.PowerStationId,
                Phase = request.Phase,
                Status = request.Status ?? "planning",
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ExpectedCompletion = request.ExpectedCompletion,
                ResponsiblePerson = request.ResponsiblePerson,
                Description = request.Description,
                Documents = request.Documents
            };

            _context.ConstructionProcesses.Add(process);
            await _context.SaveChangesAsync();

            var response = new ConstructionProcessResponse
            {
                Id = process.Id,
                PowerStationId = process.PowerStationId,
                Phase = process.Phase,
                Status = process.Status,
                StartDate = process.StartDate,
                EndDate = process.EndDate,
                ExpectedCompletion = process.ExpectedCompletion,
                ResponsiblePerson = process.ResponsiblePerson,
                Description = process.Description,
                Documents = process.Documents,
                CreatedAt = process.CreatedAt,
                UpdatedAt = process.UpdatedAt
            };

            return CreatedAtAction(nameof(GetConstructionProcess), new { id = process.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ConstructionProcessResponse>> UpdateConstructionProcess(int id, ConstructionProcessRequest request)
        {
            var process = await _context.ConstructionProcesses.FindAsync(id);

            if (process == null)
            {
                return NotFound();
            }

            process.Phase = request.Phase;
            if (request.Status != null)
                process.Status = request.Status;
            process.StartDate = request.StartDate;
            process.EndDate = request.EndDate;
            process.ExpectedCompletion = request.ExpectedCompletion;
            process.ResponsiblePerson = request.ResponsiblePerson;
            process.Description = request.Description;
            process.Documents = request.Documents;
            process.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Ok(new ConstructionProcessResponse
            {
                Id = process.Id,
                PowerStationId = process.PowerStationId,
                Phase = process.Phase,
                Status = process.Status,
                StartDate = process.StartDate,
                EndDate = process.EndDate,
                ExpectedCompletion = process.ExpectedCompletion,
                ResponsiblePerson = process.ResponsiblePerson,
                Description = process.Description,
                Documents = process.Documents,
                CreatedAt = process.CreatedAt,
                UpdatedAt = process.UpdatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConstructionProcess(int id)
        {
            var process = await _context.ConstructionProcesses.FindAsync(id);

            if (process == null)
            {
                return NotFound();
            }

            _context.ConstructionProcesses.Remove(process);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
