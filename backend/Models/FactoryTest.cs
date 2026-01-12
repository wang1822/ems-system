using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSBackend.Models
{
    public class FactoryTest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int DeviceId { get; set; }

        [Required]
        [MaxLength(100)]
        public string TestName { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? TestType { get; set; } // electrical, mechanical, software, etc.

        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, in_progress, passed, failed

        public string? Result { get; set; } // JSON string with test results

        [MaxLength(100)]
        public string? TestedBy { get; set; }

        public DateTime? TestedAt { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("DeviceId")]
        public Device? Device { get; set; }
    }
}
