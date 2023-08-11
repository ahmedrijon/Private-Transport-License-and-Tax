using System.ComponentModel.DataAnnotations;

namespace Transport_licensing_tax_management.DataModel
{
    public class Taxes
    {
        [Key]
        public long taxID { get; set; }
        [Required]
        public string VehiclesNumber { get; set; }
        public long PaymentsID { get; set; }
        public DateTime Issu_Date { get; set; } = DateTime.Now;
        public DateTime Expired_Date { get; set; }
        public int RegisterNID { get; set; } = 0;

    }
}
