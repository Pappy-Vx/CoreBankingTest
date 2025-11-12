using CoreBanking.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Infrastructure.ServiceBus
{
    public interface IDeadLetterQueueProcessor
    {
        Task ProcessDeadLetterMessagesAsync(string queueOrTopicName, string subscriptionName, CancellationToken cancellationToken);
        Task<int> ReprocessDeadLetterMessagesAsync(string sourceQueue, string destinationQueue, int maxMessages, CancellationToken cancellationToken);
        Task<List<DeadLetterMessage>> GetDeadLetterMessagesAsync(string queueOrTopicName, string subscriptionName, int maxMessages, CancellationToken cancellationToken);
    }
}
