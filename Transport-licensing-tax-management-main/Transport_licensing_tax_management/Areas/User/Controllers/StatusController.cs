using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Transport_licensing_tax_management.Data;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class StatusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatusController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult MyCarList()
        {
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var CarList = _context.Vehicles.Where(x=>x.Create_By == userID && x.Status!="Done").ToList();
            return View(CarList);
        }
        public IActionResult MyCarStatus(long VehiclesId)
        {
            var vehicleStatus = _context.Vehicles
            .Where(x => x.VehiclesId == VehiclesId && x.Status != "done")
            .Select(x => new ViewModel.StatusVM
            {
                VehiclesId = x.VehiclesId,
                Status = x.Status,
                RegSerial = x.RegSerial,
                Appointment = x.AppointmentDate,
                FeesAmount = _context.RegistrationFee
                    .Where(fee => fee.EnginePowerS <= x.EngineCC && fee.EnginePowerE >= x.EngineCC)
                    .Select(fee => fee.Amount)
                    .FirstOrDefault()
            })
            .FirstOrDefault();


            return View(vehicleStatus);
        }
        public IActionResult UpdateInfo(long VehiclesId)
        {
            // Retrieve the vehicle from the database based on the VehiclesId
            var vehicle = _context.Vehicles.FirstOrDefault(x => x.VehiclesId == VehiclesId);

            if (vehicle == null)
            {
                // Vehicle not found, return an error or redirect to an error page
                return NotFound();
            }

            // Pass the vehicle to the view for editing
            return View(vehicle);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInfo(Vehicles model)
        {
            var data = _context.Vehicles.FirstOrDefault(x => x.VehiclesId == model.VehiclesId);
            if (data != null)
            {
                if (ModelState.IsValid)
                {
                    // Update the properties with new values
                    data.EngineNumber = model.EngineNumber;
                    data.chassisNumber = model.chassisNumber;
                    data.RegisterName = model.RegisterName;
                    data.RegisterNID = model.RegisterNID;
                    data.Nationality = model.Nationality;
                    data.Bank = model.Bank;
                    data.PhoneNumber = model.PhoneNumber;
                    data.ManufactureYear = model.ManufactureYear;
                    data.Color = model.Color;
                    data.BodyType = model.BodyType;
                    data.RPM = model.RPM;
                    data.FuelUsed = model.FuelUsed;
                    data.MakersCountry = model.MakersCountry;
                    data.NumberOfCylinders = model.NumberOfCylinders;
                    data.WheelBase = model.WheelBase;
                    data.Update_By = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "System";
                    data.Update_At = DateTime.Now;

                    _context.Update(data);
                    await _context.SaveChangesAsync();

                    return Json(new
                    {
                        icon = "success",
                        message = "Registration updated",
                        title = "Success"
                    });
                }
                else
                {
                    return Json(new
                    {
                        icon = "error",
                        message = "Invalid input data",
                        title = "Error"
                    });
                }
            }
            else
            {
                return Json(new
                {
                    icon = "error",
                    message = "Vehicle not found",
                    title = "Error"
                });
            }
        }



    }
}
