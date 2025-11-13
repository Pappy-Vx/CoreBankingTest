using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Core.Models
{
    public class JobStatistics
    {
        public int Servers { get; set; }
        public long Succeeded { get; set; }
        public long Failed { get; set; }
        public long Enqueued { get; set; }
        public long Processing { get; set; }
        public long Scheduled { get; set; }
        public long Deleted { get; set; }
        public long Recurring { get; set; }
        public long Retries { get; set; }
    }
}
