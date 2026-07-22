using System.ComponentModel.DataAnnotations;

namespace TalentSyncAI.Models.ViewModels
{
    public class CandidateProfileViewModel
    {
        [Display(Name = "Date of Birth")]
        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Country { get; set; }

        [Display(Name = "PIN Code")]
        public string? Pincode { get; set; }
    }
}
