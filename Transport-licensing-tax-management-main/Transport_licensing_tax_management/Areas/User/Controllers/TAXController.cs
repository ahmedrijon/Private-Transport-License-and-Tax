using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.ViewModel;

namespace Transport_licensing_tax_management.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class TAXController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TAXController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult MyTax()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MyTax([FromQuery] string CarNumber)
        {
            var vehicle = _context.Vehicles.FirstOrDefault(v => v.VehiclesNumber == CarNumber);

            if (vehicle == null)
            {
                ViewBag.msg = "Car Not Found!";
                return View();
            }
            var taxHistory = _context.Taxes
            .Join(_context.Payments,
                t => t.PaymentsID,
                p => p.PaymentsID,
                (t, p) => new TaxesVM
                {
                    taxID = t.taxID,
                    VehiclesNumber = t.VehiclesNumber,
                    Fees = p.Amount,
                    Issu_Date = t.Issu_Date,
                    Expired_Date = t.Expired_Date,
                    RegisterNID = vehicle.RegisterNID
                })
            .Where(t => t.VehiclesNumber == CarNumber)
            .ToList();

            ViewBag.TaxesHistory = taxHistory;
           
            return View(taxHistory);
        }
    }
}
