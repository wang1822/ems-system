using Microsoft.EntityFrameworkCore;
using EMSBackend.Models;

namespace EMSBackend.Data
{
    public class EMSDbContext : DbContext
    {
        public EMSDbContext(DbContextOptions<EMSDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<PowerStation> PowerStations { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceMonitoring> DeviceMonitoring { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<FactoryTest> FactoryTests { get; set; }
        public DbSet<ConstructionProcess> ConstructionProcesses { get; set; }
        public DbSet<Statistics> Statistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Device>()
                .HasIndex(d => d.SerialNumber)
                .IsUnique();

            // Configure relationships
            modelBuilder.Entity<Device>()
                .HasOne(d => d.PowerStation)
                .WithMany(p => p.Devices)
                .HasForeignKey(d => d.PowerStationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DeviceMonitoring>()
                .HasOne(dm => dm.Device)
                .WithMany(d => d.MonitoringData)
                .HasForeignKey(dm => dm.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FactoryTest>()
                .HasOne(ft => ft.Device)
                .WithMany(d => d.FactoryTests)
                .HasForeignKey(ft => ft.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConstructionProcess>()
                .HasOne(cp => cp.PowerStation)
                .WithMany(p => p.ConstructionProcesses)
                .HasForeignKey(cp => cp.PowerStationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
