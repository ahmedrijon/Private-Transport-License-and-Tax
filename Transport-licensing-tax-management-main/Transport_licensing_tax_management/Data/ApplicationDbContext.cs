using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Transport_licensing_tax_management.DataModel;

namespace Transport_licensing_tax_management.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Transport_licensing_tax_management.DataModel.Vehicles>? Vehicles { get; set; }
        public DbSet<Transport_licensing_tax_management.DataModel.RegistrationFee>? RegistrationFee { get; set; }
        public DbSet<Transport_licensing_tax_management.DataModel.TaxFee>? TaxFee { get; set; }
        public DbSet<Transport_licensing_tax_management.DataModel.Payments>? Payments { get; set; }
        public DbSet<Transport_licensing_tax_management.DataModel.Taxes>? Taxes { get; set; }
        public DbSet<Transport_licensing_tax_management.DataModel.Notices>? Notices { get; set; }
    }
}