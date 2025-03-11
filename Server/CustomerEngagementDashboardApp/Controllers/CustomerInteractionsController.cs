using Microsoft.AspNetCore.Mvc;
using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Services.CustomerInteractionService;
using CustomerEngagementDashboardApp.Dtos;
using CRM_API.Services;
using System.Reflection;
using CustomerEngagementDashboardApp.BL.CustomerInteractionBL;
using CustomerEngagementDashboardApp.Utils;

namespace CustomerEngagementDashboardApp.Controllers
{
    [Route("api/interactions")]
    [ApiController]
    public class CustomerInteractionsController : ControllerBase
    {
        private readonly ICustomerInteractionBL _interactionBL;
        private readonly ILogger<CustomerInteractionsController> _logger;

        public CustomerInteractionsController(ICustomerInteractionBL interactionBL, ILogger<CustomerInteractionsController> logger)
        {
            _interactionBL = interactionBL;
            _logger = logger;
        }

        // GET: api/interactions
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<CustomerInteraction>>> GetAllInteractionsAsync(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize = 20,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] string? interactionType = null, 
            [FromQuery] string? outcome = null
        )
        {
            _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} Started - pageNumber - {pageNumber}");

            var interactions = await _interactionBL.GetAllInteractionsAsync(pageNumber, pageSize, startDate, endDate, interactionType, outcome);
            return Ok(interactions);
        }

        // Do we need to use IAsyncEnumerable? How should we implement streaming for efficient memory usage and performance?

        //[HttpGet]
        //public async IAsyncEnumerable<CustomerInteraction> GetAllInteractions()
        //{
        //    await foreach (var interaction in _interactionService.GetAllInteractionsStreamAsync())
        //    {
        //        yield return interaction;
        //    }
        //}

        // GET: api/interactions/topK
        [HttpGet("topK")]
        public async Task<ActionResult<List<(string CustomerId, int Count)>>> GetTopK(List<CustomerInteraction> unsortedList, int k)
        {
            _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} Started");

            return Ok(CustomerRanking.GetTopKCustomers(unsortedList, k));
        }

        // POST: api/interactions
        [HttpPost]
        public async Task<ActionResult<CustomerInteraction>> CreateInteraction(CustomerInteractionDTO dto)
        {
            _logger.LogInformation($"{MethodBase.GetCurrentMethod()?.Name} Started");

            if (dto.InteractionDate > DateTime.UtcNow)
                return BadRequest("Interaction date cannot be in the future.");

            var createdInteraction = await _interactionBL.CreateInteraction(dto);
            return CreatedAtAction(nameof(CreateInteraction), new { id = createdInteraction?.Id }, createdInteraction);
        }


    }
}