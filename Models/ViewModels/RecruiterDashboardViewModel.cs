namespace TalentSyncAI.Models.ViewModels
{
    public class RecruiterDashboardViewModel
    {
        public string RecruiterName { get; set; } = string.Empty;

        public int TotalJobs { get; set; }

        public int TotalApplications { get; set; }

        public int TotalInterviews { get; set; }

        public int TotalShortlisted { get; set; }
    }
}
