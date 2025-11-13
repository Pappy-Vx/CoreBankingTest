using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.Common.Models
{
    public class MaintenanceOperationDetail
    {
        public string OperationType { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    }
}
