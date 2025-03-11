using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Dtos;

namespace CustomerEngagementDashboardApp.Services.CustomerInteractionService
{
    public interface ICustomerInteractionService
    {
        Task<PaginatedResult<CustomerInteraction>> GetAllInteractionsAsync(
             int pageNumber,
            int pageSize,
            DateTime? startDate,
            DateTime? endDate,
            string? interactionType,
            string? outcome
        );
        Task<CustomerInteraction> CreateInteractionAsync(CustomerInteraction interaction);
    }
}
