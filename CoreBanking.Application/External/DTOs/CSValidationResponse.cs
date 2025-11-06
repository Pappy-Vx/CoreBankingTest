using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreBanking.Application.External.DTOs
{
    public record CSValidationResponse
    {
        public bool IsValid { get; init; }
        public string Reason { get; init; } = string.Empty;
    }
}
