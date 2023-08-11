using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class VehiclesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VehiclesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> NewVehiclesRequestList()
        {

            return _context.Vehicles != null ?
                        View(await _context.Vehicles.Where(x => x.Status == "Pending").ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
        }
        public async Task<IActionResult> RegisteredVehicles()
        {

            return _context.Vehicles != null ?
                        View(await _context.Vehicles.Where(x => x.Status == "Completed").ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
        }

        public async Task<IActionResult> PendingVehiclesRequestList()
        {
            return _context.Vehicles != null ?
                        View(await _context.Vehicles.Where(x => x.Status == "Waiting for payment").ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Vehicles == null)
            {
                return NotFound();
            }

            var vehicles = await _context.Vehicles
                .FirstOrDefaultAsync(m => m.VehiclesId == id);
            if (vehicles == null)
            {
                return NotFound();
            }

            return View(vehicles);
        }

      
        
        public IActionResult Appointment(long vehiclesId, DateTime date)
        {
            try
            {
                var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehiclesId == vehiclesId);
                if (vehicle != null)
                {
                    vehicle.Status = "Waiting for payment";
                    vehicle.AppointmentDate = date;
                    _context.SaveChanges();
                }
                return Json("OK");
            }
            catch (Exception ex)
            {
                return Json("Error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Reject(long vehiclesId)
        {
            try
            {
                var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehiclesId == vehiclesId);
                if (vehicle != null)
                {

                    
                    vehicle.Status = "Reject";
                    vehicle.Update_By = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
                    vehicle.Update_At = DateTime.Now;
                    _context.SaveChanges();
                    return Ok("OK");
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, ex.Message);
            }
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
                return RedirectToAction(nameof(Index));
            }


            return View(model);
        }


        private bool VehiclesExists(long id)
        {
            return (_context.Vehicles?.Any(e => e.VehiclesId == id)).GetValueOrDefault();
        }
    }
}
