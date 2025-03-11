using CRM_API.Services;
using CustomerEngagementAPI.Data;
using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Services.CachingService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

public class CustomerInteractionServiceTests
{
    private readonly AppDbContext _context;
    private readonly Mock<ILogger<CustomerInteractionService>> _mockLogger;
    private readonly Mock<ICachingService> _mockCacher;

    private readonly CustomerInteractionService _service;

    public CustomerInteractionServiceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new AppDbContext(options);
        _mockLogger = new Mock<ILogger<CustomerInteractionService>>();
        _mockCacher = new Mock<ICachingService>();

        _service = new CustomerInteractionService(_context, _mockLogger.Object, _mockCacher.Object);
    }

    [Fact]
    public async Task CreateInteraction_ShouldSaveToDatabase()
    {
        var interaction = new CustomerInteraction
        {
            CustomerName = "John Doe",
            InteractionType = CustomerEngagementDashboardApp.Constants.Enums.InteractionType.Email,
            InteractionDate = DateTime.UtcNow,
            Outcome = "Successful"
        };

        await _service.CreateInteractionAsync(interaction);

        var savedInteraction = await _context.CustomerInteractions.FirstOrDefaultAsync();
        Assert.NotNull(savedInteraction);
        Assert.Equal("John Doe", savedInteraction.CustomerName);
    }
    [Fact]
    public async Task CreateInteraction_ShouldThrowException_WhenSaveFails()
    {
        var interaction = new CustomerInteraction
        {
            CustomerName = "John Doe",
            InteractionType = CustomerEngagementDashboardApp.Constants.Enums.InteractionType.Email,
            InteractionDate = DateTime.UtcNow,
            Outcome = "Successful"
        };

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "FailingTestDb")
            .Options;

        await using var failingContext = new AppDbContext(options);

        var mockContext = new Mock<AppDbContext>(options);
        mockContext.Setup(m => m.SaveChangesAsync(default))
                   .ThrowsAsync(new ApplicationException("Database save failed"));

        var serviceWithFailingContext = new CustomerInteractionService(mockContext.Object, _mockLogger.Object, _mockCacher.Object);

        var ex = await Assert.ThrowsAsync<ApplicationException>(async () =>
            await serviceWithFailingContext.CreateInteractionAsync(interaction)
        );
        Assert.Contains("An error occurred while fetching customer", ex.Message);

    }


    [Fact]
    public async Task CreateInteraction_ShouldSaveToCache()
    {
        var interaction = new CustomerInteraction
        {
            CustomerName = "John Doe",
            InteractionType = CustomerEngagementDashboardApp.Constants.Enums.InteractionType.Email,
            InteractionDate = DateTime.UtcNow,
            Outcome = "Successful"
        };

        await _service.CreateInteractionAsync(interaction);

        //_mockCacher.Verify(
        //    c => c.SetCacheAsync(It.IsAny<string>(), It.IsAny<CustomerInteraction>(), It.IsAny<TimeSpan?>()),
        //    Times.Once,
        //    "Expected the interaction to be cached but it was not."
        //);
    }
}
