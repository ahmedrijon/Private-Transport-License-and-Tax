using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult NewCarFees()
        {
            ViewBag.registrationFees = _context.RegistrationFee?.ToList() ?? new List<RegistrationFee>();
            return View();
        }
        [HttpPost]
        public IActionResult NewCarFees(RegistrationFee model)
        {
            if (ModelState.IsValid)
            {
                _context.RegistrationFee.Add(model);
                _context.SaveChanges();

            }
            ViewBag.registrationFees = _context.RegistrationFee?.ToList() ?? new List<RegistrationFee>();
            return View();
        }
        [HttpPost]
        public IActionResult EditNewCarFees(RegistrationFee model)
        {
            if (ModelState.IsValid)
            {
                var existingFee = _context.RegistrationFee.FirstOrDefault(f => f.ID == model.ID);

                if (existingFee != null)
                {
                    existingFee.EnginePowerS = model.EnginePowerS;
                    existingFee.EnginePowerE = model.EnginePowerE;
                    existingFee.Amount = model.Amount;

                    _context.SaveChanges();

                    return RedirectToAction("NewCarFees");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Registration fee not found.");
                }
            }

            return View(model);
        }

        public IActionResult Taxes()
        {
            ViewBag.Taxes = _context.TaxFee?.ToList() ?? new List<TaxFee>();
            return View();
        }

        [HttpPost]
        public IActionResult Taxes(TaxFee model)
        {

            if (ModelState.IsValid)
            {
                _context.TaxFee.Add(model);
                _context.SaveChanges();

            }
            ViewBag.Taxes = _context.TaxFee?.ToList() ?? new List<TaxFee>();
            return View();
        }

        [HttpPost]
        public IActionResult EditTaxes(TaxFee model)
        {
            if (ModelState.IsValid)
            {
                var existingFee = _context.TaxFee.FirstOrDefault(f => f.ID == model.ID);

                if (existingFee != null)
                {
                    existingFee.EnginePowerS = model.EnginePowerS;
                    existingFee.EnginePowerE = model.EnginePowerE;
                    existingFee.Amount = model.Amount;

                    _context.SaveChanges();

                    return RedirectToAction("Taxes");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Taxes fee not found.");
                }
            }

            return View(model);
        }


        public IActionResult PaymentReport()
        {
            var PaymentReport = _context.Payments.OrderByDescending(x=>x.PaymentsID).ToList();
            return View(PaymentReport);
        }

        public async Task<IActionResult> PaymentReportPDF()
        {
            var PaymentReport = _context.Payments.OrderByDescending(x => x.PaymentsID).ToList();
            return View(PaymentReport);

        }
        public IActionResult TakePayment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetAmount(string paymentCode, string paymentType)
        {

            if (paymentType == "NEWREG" && paymentCode.Length == 8)
            {
                var cars = _context.Vehicles.Where(x => x.RegSerial == paymentCode && x.Status == "Waiting for payment").FirstOrDefault();
                if (cars == null) return Json("0");

                var Amount = _context.RegistrationFee.Where(x => x.EnginePowerS <= cars.EngineCC && x.EnginePowerE >= cars.EngineCC).Select(x => x.Amount).FirstOrDefault();
                if (Amount != null) return Json(Amount);
                else return Json("0");
            }
            else
            {
                return Json("0");
            }


        }
        
        [HttpPost]
        public IActionResult GetTaxAmount(string carNumber, string paymentType)
        {

            if (paymentType == "TAX" && carNumber.Length > 10)
            {
                var cars = _context.Vehicles.Where(x => x.VehiclesNumber == carNumber).FirstOrDefault();
                if (cars == null) return Json("0");

                var Amount = _context.TaxFee.Where(x => x.EnginePowerS <= cars.EngineCC && x.EnginePowerE >= cars.EngineCC).Select(x => x.Amount).FirstOrDefault();
                if (Amount != null) return Json(Amount);
                else return Json("0");
            }
            else
            {
                return Json("0");
            }


        }

        [HttpPost]
        public IActionResult TakePayment(Payments model,int taxyear)
        {
            if (model.PaymentCode != null && model.Type == "NEWREG" && model.CarNumber == null)
            {
                var cars = _context.Vehicles.FirstOrDefault(x => x.RegSerial == model.PaymentCode && x.Status == "Waiting for payment");
                if (cars == null)
                {
                    ViewBag.Message = "Invalid payment amount!";
                    return View();
                }
                var amount = _context.RegistrationFee.FirstOrDefault(x => x.EnginePowerS <= cars.EngineCC && x.EnginePowerE >= cars.EngineCC)?.Amount;
                if (amount == null)
                {
                    ViewBag.Message = "Invalid payment amount!";
                    return View();
                }

                if (amount == model.Amount)
                {
                    _context.Add(model);
                    _context.SaveChanges();

                    cars.Status = "Payment";
                    _context.SaveChanges();
                    ViewBag.Message = "Payment successfully processed";
                }
                else
                {
                    ViewBag.Message = "Invalid payment amount!";
                }
            }
            else if (model.PaymentCode == null && model.Type == "TAX" && model.CarNumber != null)
            {

                var cars = _context.Vehicles.FirstOrDefault(x => x.VehiclesNumber == model.CarNumber);
                if (cars == null)
                {
                    ViewBag.Message = "Invalid payment Information!";
                    return View();
                }
                var amount = _context.TaxFee.FirstOrDefault(x => x.EnginePowerS <= cars.EngineCC && x.EnginePowerE >= cars.EngineCC)?.Amount;
                if (amount == null)
                {
                    ViewBag.Message = "Invalid payment amount!";
                    return View();
                }

                if (model.Amount == (amount*taxyear))
                {
                    _context.Add(model);
                    _context.SaveChanges();

                    var checkTaxes = _context.Taxes
    .Where(x => x.VehiclesNumber == model.CarNumber)
    .OrderByDescending(x => x.taxID)
    .FirstOrDefault();

                    Taxes taxes = new Taxes();
                    taxes.PaymentsID = model.PaymentsID;
                    taxes.VehiclesNumber = model.CarNumber;
                    taxes.RegisterNID = cars.RegisterNID;

                    if (checkTaxes != null)
                    {
                        taxes.Expired_Date = checkTaxes.Expired_Date.AddYears(taxyear);
                    }
                    else
                    {
                        taxes.Expired_Date = DateTime.Now.AddYears(taxyear);
                    }



                    _context.Add(taxes);
                    _context.SaveChanges();
                    ViewBag.Message = "TAX Payment successfully processed";
                }
                else
                {
                    ViewBag.Message = "Invalid payment amount!";
                }
            }
            else
            {
                ViewBag.Message = "Invalid payment information!";
            }

            return View();
        }

    }
}
