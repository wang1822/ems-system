using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSBackend.Models
{
    public class Device
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PowerStationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DeviceType { get; set; } = string.Empty; // battery, inverter, meter, etc.

        [MaxLength(100)]
        public string? Manufacturer { get; set; }

        [MaxLength(100)]
        public string? Model { get; set; }

        [MaxLength(100)]
        public string? SerialNumber { get; set; }

        public string? Specifications { get; set; } // JSON string

        public DateTime? InstallationDate { get; set; }

        public DateTime? WarrantyExpiry { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "offline"; // online, offline, maintenance, error

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PowerStationId")]
        public PowerStation? PowerStation { get; set; }

        public ICollection<DeviceMonitoring> MonitoringData { get; set; } = new List<DeviceMonitoring>();
        public ICollection<FactoryTest> FactoryTests { get; set; } = new List<FactoryTest>();
    }
}
