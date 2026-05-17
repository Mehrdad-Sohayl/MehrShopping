using Microsoft.EntityFrameworkCore;
using PersonalInfo.Api.Domain;

namespace PersonalInfo.Api.Data
{
    public class PersonalInfoDbContext : DbContext
    {
        public PersonalInfoDbContext(DbContextOptions options) : base(options)
        {
        }

        protected PersonalInfoDbContext()
        {
        }

        public DbSet<Person> People => Set<Person>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(e =>
            {
                e.HasKey(p => p.NationalCode);
                e.Property(p => p.FirstName).IsRequired();
                e.Property(p => p.LastName).IsRequired();
            });


        }
    }
}
