using ManageApartment.Entities;
using Microsoft.EntityFrameworkCore;


namespace ManageApartment.Repositories
{
    public class ApplicationDbContext : DbContext  
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Flat> Flats { get; set; }
        public DbSet<Resident> Residents { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Flat>()
            .HasMany(flat => flat.Residents)
            .WithOne(resident => resident.Flat)
            .HasForeignKey(resident => resident.FlatId);

            modelBuilder.Entity<Apartment>()
                .HasMany(apartment => apartment.Flats)
                .WithOne(flat => flat.Apartment)
                .HasForeignKey(flat => flat.ApartmentId);
        }
    }
}
