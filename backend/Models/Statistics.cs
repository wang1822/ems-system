using System.ComponentModel.DataAnnotations;

namespace EMSBackend.Models
{
    public class Statistics
    {
        [Key]
        public int Id { get; set; }

        public int? PowerStationId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StatisticType { get; set; } = string.Empty; // uptime, charge, discharge, alerts, etc.

        [MaxLength(20)]
        public string? Period { get; set; } // daily, weekly, monthly, yearly

        public DateTime? PeriodStart { get; set; }

        public DateTime? PeriodEnd { get; set; }

        public double? Value { get; set; }

        [MaxLength(20)]
        public string? Unit { get; set; }

        public string? Data { get; set; } // JSON string with detailed data

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
