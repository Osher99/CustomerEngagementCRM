using static CustomerEngagementDashboardApp.Constants.Enums;

namespace CustomerEngagementDashboardApp.Dtos
{
    public class CustomerInteractionDTO
    {
        public string CustomerName { get; set; }
        public InteractionType InteractionType { get; set; }
        public DateTime InteractionDate { get; set; }
        public string Outcome { get; set; }
        public string Notes { get; set; }
    }
}
