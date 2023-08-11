using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Transport_licensing_tax_management.Migrations;

namespace Transport_licensing_tax_management.DataModel
{
    public class Vehicles
    {
        [Key]
        public long VehiclesId { get; set; }
        public string VehiclesNumber { get; set; } = "";
        public string EngineNumber{ get; set; } = "";
        public string chassisNumber { get; set; } = "";
        public int EngineCC { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime AppointmentDate { get; set; }

        //Owner information start
        public string RegisterName { get; set; } = "";
        public int RegisterNID { get; set; }
        public string Nationality { get; set; } = "Bangladesh";
        public string? Bank { get; set; }
        public string? PhoneNumber { get; set; }

        //Car info
        public int ManufactureYear { get; set; }
        public string? Color { get; set; }
        public string? BodyType { get; set; }
        public string? RPM { get; set; }
        public string? FuelUsed { get; set; }
        public string? MakersCountry { get; set; }
        public int NumberOfCylinders { get; set; }
        public string? WheelBase { get; set; }


        //for trace the input
        public string  RegSerial { get; set; } = RandomStringGenerator.GenerateRandomString(8);
        public string Create_By { get; set; } = "System";
        public DateTime Create_At{ get; set; } = DateTime.Now;
        public string? Update_By { get; set; }
        public DateTime Update_At{ get; set; } = DateTime.Now;

    }
    public static class RandomStringGenerator
    {
        public static string GenerateRandomString(int length)
        {
            const string chars = "AKASHBRTA0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            return result;
        }
    }

}
