using AutoMapper;
using CustomerEngagementDashboardApp.Controllers;
using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Dtos;
using CustomerEngagementDashboardApp.Services.CustomerInteractionService;
using Microsoft.EntityFrameworkCore;

namespace CustomerEngagementDashboardApp.BL.CustomerInteractionBL
{
    public class CustomerInteractionBL : ICustomerInteractionBL
    {
        private readonly ICustomerInteractionService _interactionService;
        private readonly ILogger<CustomerInteractionBL> _logger;
        private readonly IMapper _mapper;

        public CustomerInteractionBL(ICustomerInteractionService interactionService, ILogger<CustomerInteractionBL> logger, IMapper mapper)
        {
            _interactionService = interactionService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CustomerInteraction>> GetAllInteractionsAsync(
            int pageNumber,
            int pageSize,
            DateTime? startDate,
            DateTime? endDate,
            string? interactionType,
            string? outcome
        )
        {
            try
            {
                _logger.LogInformation("Fetching all customer interactions.");
                var interactions = await _interactionService.GetAllInteractionsAsync(pageNumber, pageSize, startDate, endDate, interactionType, outcome);
                return interactions;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while fetching customer interactions.");
                throw;
            }
        }

        public async Task<CustomerInteraction?> CreateInteraction(CustomerInteractionDTO dto)
        {
            try
            {
                _logger.LogInformation("Creating new customer interaction for {CustomerName}.", dto.CustomerName);

                var interaction = _mapper.Map<CustomerInteraction>(dto);
                var createdInteraction = await _interactionService.CreateInteractionAsync(interaction);

                if (createdInteraction != null)
                {
                    _logger.LogInformation("Customer interaction created successfully for {CustomerName}.", dto.CustomerName);
                    return createdInteraction;
                }
                else
                {
                    _logger.LogWarning("Failed to create interaction for {CustomerName}.", dto.CustomerName);
                    throw new ApplicationException($"Failed to create interaction for customer {dto.CustomerName}. Please check the input data and try again.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating a customer interaction.");
                throw;
            }
        }
    }
}