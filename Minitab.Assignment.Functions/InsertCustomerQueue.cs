using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Minitab.Assignment.DomainModels;

namespace Minitab.Assignment.Functions
{
    public static class InsertCustomerQueue
    {
        [FunctionName("InsertCustomer")]
        public static void Run([QueueTrigger("customer-items", Connection = "AzureWebJobsStorage")] string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            }
            catch(Exception ex) 
            {
                log.LogInformation(ex.ToString());
            }
        }
    }
}
