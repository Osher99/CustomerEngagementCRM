using CRM_API.Services;
using CustomerEngagementAPI.Data;
using CustomerEngagementDashboardApp.BL.CustomerInteractionBL;
using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Services.CachingService;
using CustomerEngagementDashboardApp.Services.CustomerInteractionService;
using CustomerEngagementDashboardApp.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static CustomerEngagementDashboardApp.Constants.Enums;

var builder = WebApplication.CreateBuilder(args);

// DI
builder.Services.AddScoped<ICustomerInteractionBL, CustomerInteractionBL>();
builder.Services.AddScoped<ICachingService, CachingService>();
builder.Services.AddScoped<ICustomerInteractionService, CustomerInteractionService>();

// Configure Postgres
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// Validators
builder.Services.AddValidatorsFromAssemblyContaining<CustomerInteractionValidator>();

// Automapper profiles
builder.Services.AddAutoMapper(typeof(CustomerInteractionProfile));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379";
    options.InstanceName = "CRMCache_";
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();

var app = builder.Build();
app.UseCors("AllowAll"); // subject to change


app.UseAuthentication();
app.UseAuthorization();

// Insert random data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!dbContext.CustomerInteractions.Any())
    {
        var random = new Random();
        var interactions = new List<CustomerInteraction>();

        for (int i = 0; i < 100; i++)
        {
            var customerName = "Customer " + random.Next(1, 101);
            var interactionType = (InteractionType)random.Next(0, Enum.GetValues(typeof(InteractionType)).Length);
            var outcome = random.Next(0, 2) == 0 ? "Successful" : "Unsuccessful"; 
            var notes = "Notes for interaction " + i;

            var interaction = new CustomerInteraction
            {
                CustomerName = customerName,
                InteractionType = interactionType,
                InteractionDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                Outcome = outcome,
                Notes = notes
            };

            interactions.Add(interaction);
        }

        dbContext.CustomerInteractions.AddRange(interactions);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("100 random interactions have been inserted.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
