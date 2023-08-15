using System.ComponentModel.DataAnnotations;
namespace Transport_licensing_tax_management.DataModel
{
    public class Notices
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Heading { get; set; }
        [Required]
        public string Detail { get; set; }
        public string Date { get; set; }
    }
}
