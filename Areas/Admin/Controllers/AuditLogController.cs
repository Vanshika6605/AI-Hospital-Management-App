using AIHospitalManagementSys.Data;
using AIHospitalManagementSys.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AIHospitalManagementSys.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;

            var logs = _context.AuditLogs
                .Include(a => a.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                logs = logs.Where(s => s.Action.Contains(searchString)
                                       || s.EntityName.Contains(searchString)
                                       || (s.User != null && s.User.FullName.Contains(searchString)));
            }

            // Simple take 50 for now, could implement full pagination later
            var recentLogs = await logs.OrderByDescending(l => l.CreatedAt).Take(50).ToListAsync();

            return View(recentLogs);
        }
    }
}
