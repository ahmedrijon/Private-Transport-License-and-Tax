using Microsoft.AspNetCore.Identity;

namespace Transport_licensing_tax_management.ViewModel
{
    public class StatusVM
    {
        public long VehiclesId { get; set; }
        public string? Status { get; set; }
        public string RegSerial { get; set; } = "Error";
        public double FeesAmount { get; set; } 
        public DateTime Appointment { get; set; } 
    }
}
