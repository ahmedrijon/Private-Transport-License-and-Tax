using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Areas.User.Controllers
{
  
    [Area("User")]
    [Authorize(Roles = "Admin,User")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult NewVehiclesRegistration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewVehiclesRegistration(Vehicles model)
        {
            if (!_context.Vehicles.Any(x => x.EngineNumber == model.EngineNumber || x.chassisNumber == model.chassisNumber))
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                model.Create_By = userIdClaim ?? "System";

                _context.Add(model);
                await _context.SaveChangesAsync();

                return Json(new
                {
                    icon = "success",
                    message = "Registration completed",
                    title = "Success"
                });
            }
            else
            {
                return Json(new
                {
                    icon = "error",
                    message = "Duplicate engine number or chassis number found",
                    title = "Error"
                });
            }
        }

        public IActionResult Profile()
        {
            return View();
        }
        public async Task<IActionResult> Notice()
        {
            return _context.Notices != null ?
                        View(await _context.Notices.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Notices'  is null.");
        }

    }
}
