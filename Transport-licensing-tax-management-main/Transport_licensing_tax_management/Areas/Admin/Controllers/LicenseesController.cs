using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class LicenseesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LicenseesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return _context.Vehicles != null ?
                       View(_context.Vehicles.Where(x => x.Status == "Payment" && x.VehiclesNumber == "").ToList()) :
                       Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");

        }
        public IActionResult NewNumberPlateDetails(int id)
        {
            return _context.Vehicles != null ?
                       View(_context.Vehicles.Where(x => x.VehiclesId == id).FirstOrDefault()) :
                       Problem("Entity set 'ApplicationDbContext.Vehicles'  is null.");

        }
        [HttpPost]
        public IActionResult VehiclesNumberSubmit(string vehiclesNumber, int appointmentDate, long vehiclesId)
        {
            bool CheckDoubleNumberPLate = _context.Vehicles.Where(x=>x.VehiclesNumber == vehiclesNumber).Any();
            if(!CheckDoubleNumberPLate)
            {
                
                _context.Vehicles.Where(v => v.VehiclesId == vehiclesId && string.IsNullOrEmpty(v.VehiclesNumber))
                .ToList().ForEach(v =>
                {
                    v.Status = "Completed";
                    v.VehiclesNumber = vehiclesNumber;
                    v.AppointmentDate = DateTime.Today.AddDays(appointmentDate);
                    v.Update_At = DateTime.Now;
                });

                _context.SaveChanges();
                return Json("OK");
            }

            

            return Json("Number Plate Alredy Used!");
        }



    }
}

