namespace Transport_licensing_tax_management.DataModel
{
  
    public class RegistrationFee
    {
        public int ID { get; set; }
        public double EnginePowerS { get; set; }
        public double EnginePowerE { get; set; }
        public double Amount { get; set; }
    }
    public class TaxFee
    {
        public int ID { get; set; }
        public double EnginePowerS { get; set; }
        public double EnginePowerE { get; set; }
        public double Amount { get; set; }
    }
    public class Payments
    {
        public long PaymentsID { get; set; }
        public string? PaymentCode { get; set; }
        public string? CarNumber { get; set; }
        public double Amount { get; set; }
        public string Type { get; set; }
        public DateTime Create_At { get; set; } = DateTime.Now;
        public DateTime Update_At { get; set; }
    }
}
