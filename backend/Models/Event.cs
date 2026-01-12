using System.ComponentModel.DataAnnotations;

namespace EMSBackend.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        public int? DeviceId { get; set; }

        public int? PowerStationId { get; set; }

        [Required]
        [MaxLength(20)]
        public string EventType { get; set; } = string.Empty; // warning, error, maintenance, info

        [MaxLength(20)]
        public string? Severity { get; set; } // low, medium, high, critical

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public bool Resolved { get; set; } = false;

        public DateTime? ResolvedAt { get; set; }

        [MaxLength(100)]
        public string? ResolvedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
