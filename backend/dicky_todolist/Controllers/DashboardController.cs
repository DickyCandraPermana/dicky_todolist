using dicky_todolist.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dicky_todolist.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseApiController
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetStats()
        {
            var userId = GetUserId();
            var today = DateTime.UtcNow.Date;

            var totalTasks = await _context.Todos
                .Where(t => t.UserId == userId && t.DeletedAt == null)
                .CountAsync();

            var completedTasks = await _context.Todos
                .Where(t => t.UserId == userId && t.IsCompleted && t.DeletedAt == null)
                .CountAsync();

            var completedToday = await _context.Todos
                .Where(t => t.UserId == userId && t.IsCompleted && t.UpdatedAt >= today && t.DeletedAt == null)
                .CountAsync();

            var pendingTasks = totalTasks - completedTasks;

            return Ok(new
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                CompletedToday = completedToday,
                PendingTasks = pendingTasks
            });
        }

        [HttpGet("productivity")]
        public async Task<IActionResult> GetProductivity()
        {
            var userId = GetUserId();
            var sevenDaysAgo = DateTime.UtcNow.Date.AddDays(-6);

            // Fetch from DB first (without grouping/ToString that can't be translated)
            var completedTodos = await _context.Todos
                .Where(t => t.UserId == userId && t.IsCompleted && t.UpdatedAt >= sevenDaysAgo)
                .Select(t => t.UpdatedAt)
                .ToListAsync();

            // Group in memory
            var grouped = completedTodos
                .GroupBy(d => d.Date)
                .ToDictionary(g => g.Key.ToString("yyyy-MM-dd"), g => g.Count());

            // Fill in all 7 days (including days with 0 completions)
            var result = Enumerable.Range(0, 7)
                .Select(offset =>
                {
                    var date = sevenDaysAgo.AddDays(offset).ToString("yyyy-MM-dd");
                    return new
                    {
                        Date = date,
                        Count = grouped.TryGetValue(date, out var count) ? count : 0
                    };
                })
                .ToList();

            return Ok(result);
        }
    }
}
