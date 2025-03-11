using CustomerEngagementDashboardApp.DbModel;
using CustomerEngagementDashboardApp.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using static CustomerEngagementDashboardApp.Constants.Enums;

public class CustomerRankingTests
{
    [Fact]
    public void GetTopKCustomers_ShouldReturnCorrectResults()
    {
        var interactions = new List<CustomerInteraction>
        {
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
        };

        interactions.Add(new CustomerInteraction { Id = interactions[0].Id });
        interactions.Add(new CustomerInteraction { Id = interactions[0].Id });
        interactions.Add(new CustomerInteraction { Id = interactions[1].Id });
        interactions.Add(new CustomerInteraction { Id = interactions[1].Id });
        interactions.Add(new CustomerInteraction { Id = interactions[1].Id });

        int k = 2;

        var result = CustomerRanking.GetTopKCustomers(interactions, k);

        Assert.Equal(k, result.Count);
        Assert.Equal(interactions[1].Id.ToString(), result[0].CustomerId);
        Assert.Equal(interactions[0].Id.ToString(), result[1].CustomerId);
    }

    [Fact]
    public void GetTopKCustomers_ShouldReturnEmptyList_WhenKIsZero()
    {
        var interactions = new List<CustomerInteraction>
        {
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() }
        };

        int k = 0;

        var result = CustomerRanking.GetTopKCustomers(interactions, k);

        Assert.Empty(result);
    }

    [Fact]
    public void GetTopKCustomers_ShouldReturnEmptyList_WhenNoInteractions()
    {
        var interactions = new List<CustomerInteraction>();
        int k = 3;

        var result = CustomerRanking.GetTopKCustomers(interactions, k);

        Assert.Empty(result);
    }

    [Fact]
    public void GetTopKCustomers_ShouldReturnAllCustomers_WhenKIsGreaterThanListSize()
    {
        var interactions = new List<CustomerInteraction>
        {
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() },
            new CustomerInteraction { Id = Guid.NewGuid() }
        };

        int k = 10;

        var result = CustomerRanking.GetTopKCustomers(interactions, k);

        Assert.Equal(interactions.Count, result.Count);
    }

    [Fact]
    public void TestTopKCustomers_SmallDataset()
    {
        var interactions = new List<CustomerInteraction>
    {
        new CustomerInteraction { Id = Guid.NewGuid() },
        new CustomerInteraction { Id = Guid.NewGuid() },
        new CustomerInteraction { Id = Guid.NewGuid() },
        new CustomerInteraction { Id = Guid.NewGuid() },
        new CustomerInteraction { Id = Guid.NewGuid() },
        new CustomerInteraction { Id = Guid.NewGuid() }
    };

        var result = CustomerRanking.GetTopKCustomers(interactions, 2);

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.NotNull(item.CustomerId));
        Assert.All(result, item => Assert.True(item.Count > 0));
    }


    [Fact]
    public void TestTopKCustomers_LargeDataset_Performance()
    {
        var enumValues = Enum.GetValues(typeof(InteractionType));
        var interactions = new List<CustomerInteraction>();
        var random = new Random();

        for (int i = 0; i < 1_000_000; i++) 
        {
            interactions.Add(new CustomerInteraction
            {
                Id = Guid.NewGuid(),
                CustomerName = "User " + (i),
                InteractionDate = DateTime.UtcNow,
                InteractionType = (InteractionType)enumValues.GetValue(random.Next(enumValues.Length))
            });
        }


        CustomerRanking.GetTopKCustomers(interactions, 100);


        var stopwatch = Stopwatch.StartNew();
        var result = CustomerRanking.GetTopKCustomers(interactions, 100);

        System.IO.File.WriteAllLines("top_customers_result.txt", result.Select(c => $"{c.CustomerId}"));

        stopwatch.Stop();

        Assert.Equal(100, result.Count);
        Assert.True(stopwatch.ElapsedMilliseconds < 1500, $"Performance exceeded: {stopwatch.ElapsedMilliseconds} ms");
    }

}