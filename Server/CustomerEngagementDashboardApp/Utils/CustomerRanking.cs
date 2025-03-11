using CustomerEngagementDashboardApp.DbModel;
using System.Collections.Concurrent;
using System.Linq;

namespace CustomerEngagementDashboardApp.Utils
{
    public static class CustomerRanking
    {
        public static List<(string CustomerId, int Count)> GetTopKCustomers(List<CustomerInteraction> interactions, int k)
        {
            if (k <= 0) return new List<(string, int)>();


            var customerCount = new Dictionary<string, int>();

            foreach (var interaction in interactions)
            {
                if (customerCount.ContainsKey(interaction.Id.ToString()))
                    customerCount[interaction.Id.ToString()]++;
                else
                    customerCount[interaction.Id.ToString()] = 1;
            }

            return customerCount
                .OrderByDescending(c => c.Value)
                .Take(k)
                .Select(c => (c.Key, c.Value))
                .ToList();
        }

        //public class CustomerComparer : IComparer<(string CustomerId, int Count)>
        //{
        //    public int Compare((string CustomerId, int Count) x, (string CustomerId, int Count) y)
        //    {
        //        int compareCount = x.Count.CompareTo(y.Count);
        //        if (compareCount != 0) return compareCount;
        //        return string.Compare(x.CustomerId, y.CustomerId);
        //    }
        //}
    }
}
