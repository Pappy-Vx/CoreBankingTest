using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Core.Models
{
    public class FailedJob
    {
        public string JobId { get; set; } = string.Empty;
        public DateTime? FailedAt { get; set; }
        public string ExceptionMessage { get; set; } = string.Empty;
        public string ExceptionDetails { get; set; } = string.Empty;
    }
}
