using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMSBackend.Models
{
    public class ConstructionProcess
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PowerStationId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Phase { get; set; } = string.Empty; // site_survey, approval, installation, testing, etc.

        [MaxLength(20)]
        public string Status { get; set; } = "planning"; // planning, approval_pending, approved, in_progress, completed, cancelled

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? ExpectedCompletion { get; set; }

        [MaxLength(100)]
        public string? ResponsiblePerson { get; set; }

        public string? Description { get; set; }

        public string? Documents { get; set; } // JSON array of document URLs

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        [ForeignKey("PowerStationId")]
        public PowerStation? PowerStation { get; set; }
    }
}
