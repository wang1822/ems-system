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
    public class FactoryTestsController : ControllerBase
    {
        private readonly EMSDbContext _context;

        public FactoryTestsController(EMSDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FactoryTestResponse>>> GetFactoryTests(
            [FromQuery] int skip = 0,
            [FromQuery] int limit = 100,
            [FromQuery] int? deviceId = null,
            [FromQuery] string? status = null)
        {
            var query = _context.FactoryTests.AsQueryable();

            if (deviceId.HasValue)
            {
                query = query.Where(ft => ft.DeviceId == deviceId.Value);
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(ft => ft.Status == status);
            }

            var tests = await query
                .OrderByDescending(ft => ft.CreatedAt)
                .Skip(skip)
                .Take(limit)
                .Select(ft => new FactoryTestResponse
                {
                    Id = ft.Id,
                    DeviceId = ft.DeviceId,
                    TestName = ft.TestName,
                    TestType = ft.TestType,
                    Status = ft.Status,
                    Result = ft.Result,
                    TestedBy = ft.TestedBy,
                    TestedAt = ft.TestedAt,
                    Notes = ft.Notes,
                    CreatedAt = ft.CreatedAt,
                    UpdatedAt = ft.UpdatedAt
                })
                .ToListAsync();

            return Ok(tests);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FactoryTestResponse>> GetFactoryTest(int id)
        {
            var test = await _context.FactoryTests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return Ok(new FactoryTestResponse
            {
                Id = test.Id,
                DeviceId = test.DeviceId,
                TestName = test.TestName,
                TestType = test.TestType,
                Status = test.Status,
                Result = test.Result,
                TestedBy = test.TestedBy,
                TestedAt = test.TestedAt,
                Notes = test.Notes,
                CreatedAt = test.CreatedAt,
                UpdatedAt = test.UpdatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<FactoryTestResponse>> CreateFactoryTest(FactoryTestRequest request)
        {
            var test = new FactoryTest
            {
                DeviceId = request.DeviceId,
                TestName = request.TestName,
                TestType = request.TestType,
                Status = request.Status ?? "pending",
                Result = request.Result,
                TestedBy = request.TestedBy,
                Notes = request.Notes
            };

            _context.FactoryTests.Add(test);
            await _context.SaveChangesAsync();

            var response = new FactoryTestResponse
            {
                Id = test.Id,
                DeviceId = test.DeviceId,
                TestName = test.TestName,
                TestType = test.TestType,
                Status = test.Status,
                Result = test.Result,
                TestedBy = test.TestedBy,
                TestedAt = test.TestedAt,
                Notes = test.Notes,
                CreatedAt = test.CreatedAt,
                UpdatedAt = test.UpdatedAt
            };

            return CreatedAtAction(nameof(GetFactoryTest), new { id = test.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FactoryTestResponse>> UpdateFactoryTest(int id, FactoryTestRequest request)
        {
            var test = await _context.FactoryTests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            if (request.Status != null)
                test.Status = request.Status;
            test.Result = request.Result;
            test.TestedBy = request.TestedBy;
            test.Notes = request.Notes;
            test.UpdatedAt = DateTime.UtcNow;

            if (request.Status == "passed" || request.Status == "failed")
            {
                test.TestedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            return Ok(new FactoryTestResponse
            {
                Id = test.Id,
                DeviceId = test.DeviceId,
                TestName = test.TestName,
                TestType = test.TestType,
                Status = test.Status,
                Result = test.Result,
                TestedBy = test.TestedBy,
                TestedAt = test.TestedAt,
                Notes = test.Notes,
                CreatedAt = test.CreatedAt,
                UpdatedAt = test.UpdatedAt
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactoryTest(int id)
        {
            var test = await _context.FactoryTests.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            _context.FactoryTests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
