using Microsoft.EntityFrameworkCore;
using CustomerEngagementAPI.Data;
using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Services.CustomerInteractionService;
using CustomerEngagementDashboardApp.Services.CachingService;
using static CustomerEngagementDashboardApp.Constants.Enums;
using CustomerEngagementDashboardApp.Dtos;

namespace CRM_API.Services
{
    public class CustomerInteractionService : ICustomerInteractionService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CustomerInteractionService> _logger;
        private readonly ICachingService _cache;


        public CustomerInteractionService(AppDbContext context, ILogger<CustomerInteractionService> logger, ICachingService cache)
        {
            _context = context;
            _logger = logger;
            _cache = cache;
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
                _logger.LogInformation(
                    "Fetching customer interactions. Page: {PageNumber}, PageSize: {PageSize}, StartDate: {StartDate}," +
                    "EndDate: {EndDate}, InteractionType: {InteractionType}, Outcome: {Outcome}",
                    pageNumber, pageSize, startDate, endDate, interactionType, outcome
                );

                string cacheKey = $"customer_interactions_page_{pageNumber}_size_{pageSize}_startDate_{startDate}_endDate_{endDate}_interactionType_{interactionType}_outcome_{outcome}";

                var cachedInteractions = await _cache.GetCacheAsync<PaginatedResult<CustomerInteraction>>(cacheKey);
                if (cachedInteractions != null)
                {
                    _logger.LogInformation($"Returning data from cache. Interactions: {cachedInteractions.Items.Count()}");
                    return cachedInteractions;
                }

                var query = _context.CustomerInteractions.AsQueryable();

                if (startDate.HasValue)
                    query = query.Where(i => i.InteractionDate >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(i => i.InteractionDate <= endDate.Value);

                if (!string.IsNullOrEmpty(interactionType))
                {
                    if (Enum.TryParse(interactionType, out InteractionType parsedType))
                        query = query.Where(i => i.InteractionType == parsedType);
                    else
                        _logger.LogWarning("Invalid InteractionType value: {InteractionType}", interactionType);
                }

                if (!string.IsNullOrEmpty(outcome))
                    query = query.Where(i => i.Outcome == outcome);

                int totalItems = await query.CountAsync();
                int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);


                var interactions = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();


                var paginatedResult = new PaginatedResult<CustomerInteraction>
                {
                    Items = interactions,
                    TotalPages = totalPages,
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    TotalItems = totalItems
                };


                await _cache.SetCacheAsync(cacheKey, paginatedResult, TimeSpan.FromMinutes(10));
                _logger.LogInformation("Successfully fetched {Count} customer interactions.", interactions.Count);

                return paginatedResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer interactions.");
                throw new ApplicationException("An error occurred while fetching customer interactions.", ex);
            }
        }

        public async Task<CustomerInteraction> CreateInteractionAsync(CustomerInteraction interaction)
        {
            try
            {
                _logger.LogInformation("Creating a new customer interaction for {CustomerName}.", interaction.CustomerName);
                
                _context.CustomerInteractions.Add(interaction);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("new interaction added. {Id}", interaction.Id);
                return interaction;
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer interactions.");
                throw new ApplicationException("An error occurred while fetching customer interactions.", ex);
            }
        }
    }
}