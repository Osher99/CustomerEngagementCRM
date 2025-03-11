using static CustomerEngagementDashboardApp.Constants.Enums;
using System.ComponentModel.DataAnnotations;

namespace CustomerEngagementDashboardApp.DbModel
{
    public class CustomerInteraction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public InteractionType InteractionType { get; set; }

        [Required]
        public DateTime InteractionDate { get; set; }

        [Required]
        public string Outcome { get; set; } = string.Empty;

        public string? Notes { get; set; }
    }
}
