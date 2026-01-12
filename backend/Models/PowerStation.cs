using System.ComponentModel.DataAnnotations;

namespace EMSBackend.Models
{
    public class PowerStation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string Location { get; set; } = string.Empty;

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double? Capacity { get; set; } // kWh

        [MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive, maintenance

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<Device> Devices { get; set; } = new List<Device>();
        public ICollection<ConstructionProcess> ConstructionProcesses { get; set; } = new List<ConstructionProcess>();
    }
}
