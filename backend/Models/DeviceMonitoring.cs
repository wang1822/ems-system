using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSBackend.Models
{
    public class DeviceMonitoring
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DeviceId { get; set; }

        public double? BatteryLevel { get; set; } // percentage

        public double? Temperature { get; set; } // celsius

        public double? Voltage { get; set; } // volts

        public double? Current { get; set; } // amperes

        public double? PowerOutput { get; set; } // watts

        public double? EnergyGenerated { get; set; } // kWh

        public double? EnergyConsumed { get; set; } // kWh

        [MaxLength(20)]
        public string? Status { get; set; }

        [MaxLength(50)]
        public string? ErrorCode { get; set; }

        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }
    }
}
