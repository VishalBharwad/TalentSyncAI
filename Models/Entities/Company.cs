using System.ComponentModel.DataAnnotations;
using TalentSyncAI.Models.Identity;
using TalentSyncAI.Enums;

namespace TalentSyncAI.Models.Entities
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; } = string.Empty;

        public string? Website { get; set; }

        public string? Industry { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        public string? Pincode { get; set; }

        public string? Description { get; set; }

        public string? LogoPath { get; set; }

        public CompanyStatus Status { get; set; } = CompanyStatus.Pending;

        public bool ProfileCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Identity Relationship
        public string RecruiterId { get; set; } = string.Empty;

        public ApplicationUser? Recruiter { get; set; }
    }
}