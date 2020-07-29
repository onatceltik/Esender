using Esender.Models;
using Microsoft.EntityFrameworkCore;

namespace Esender.ModelsAppDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        public DbSet<EmailModel> Emails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Esender.Models.EmailModel>()
                .ToTable("EmailTable", "dbo");
        }
    }
}
