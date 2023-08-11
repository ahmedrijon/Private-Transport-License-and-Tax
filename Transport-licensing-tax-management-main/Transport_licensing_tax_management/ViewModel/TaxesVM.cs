using System.ComponentModel.DataAnnotations;

namespace Transport_licensing_tax_management.ViewModel
{
    public class TaxesVM
    {
        [Key]
        public long taxID { get; set; }
        public string? VehiclesNumber { get; set; }
        public double Fees { get; set; }
        public DateTime Issu_Date { get; set; }
        public DateTime Expired_Date { get; set; }
        public int RegisterNID { get; set; }
        public double Fine { get; set; }
    }
}
