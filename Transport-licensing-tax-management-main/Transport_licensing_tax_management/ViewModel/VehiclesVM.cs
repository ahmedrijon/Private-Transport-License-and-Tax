using System.ComponentModel.DataAnnotations;

namespace Transport_licensing_tax_management.ViewModel
{
    public class VehiclesVM
    {
        [Key]
        public long VehiclesId { get; set; }
        public string VehiclesNumber { get; set; } = "";
        public string EngineNumber { get; set; } = "";
        public string chassisNumber { get; set; } = "";
        public string RegisterName { get; set; } = "";
        public int RegisterNID { get; set; }
        public int EngineCC { get; set; }
        public int Owned { get; set; }
        public string Status { get; set; } = "Pending";

        //for trace the input
        public long Create_By { get; set; }
        public DateTime Create_At { get; set; } = DateTime.Now;
        public long Update_By { get; set; }
        public DateTime Update_At { get; set; }
    }
}
