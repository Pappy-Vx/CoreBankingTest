using CoreBanking.API.Hubs.Models;
using System.Data;

namespace CoreBanking.API.Hubs.Interceptors
{
   
    public interface IBankingClient
    {
        // Transaction notifications
        Task ReceiveTransactionNotification(TransactionNotification notification);
        Task ReceiveBalanceUpdate(BalanceUpdate update);

        // System alerts
        Task ReceiveSystemAlert(SystemAlert alert);

        // Fraud detection
        Task ReceiveFraudAlert(FraudAlert alert);

        // Connection management
        Task ConnectionStateChanged(CoreBanking.API.Hubs.Models.ConnectionState state);
    }
}
