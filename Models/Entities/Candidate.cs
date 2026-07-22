using System.ComponentModel.DataAnnotations;
using TalentSyncAI.Models.Identity;

namespace TalentSyncAI.Models.Entities
{
    public class Candidate
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser User { get; set; } = null!;

        // =========================
        // Personal Information
        // =========================

        public DateTime? DateOfBirth { get; set; }

        [StringLength(20)]
        public string? Gender { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? State { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(10)]
        public string? Pincode { get; set; }

        // =========================
        // Profile Status
        // =========================

        public bool ProfileCompleted { get; set; } = false;

        public int ProfileCompletionPercentage { get; set; } = 20;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}