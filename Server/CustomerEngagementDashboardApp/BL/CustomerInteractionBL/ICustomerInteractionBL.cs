using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Dtos;

namespace CustomerEngagementDashboardApp.BL.CustomerInteractionBL
{
    public interface ICustomerInteractionBL
    {
        Task<PaginatedResult<CustomerInteraction>> GetAllInteractionsAsync(
                int pageNumber,
                int pageSize,
                DateTime? startDate,
                DateTime? endDate,
                string interactionType,
                string outcome
            );
        Task<CustomerInteraction?> CreateInteraction(CustomerInteractionDTO dto);
    }
}
