namespace CoreBanking.API.Models.Requests;

public record CreateCustomerRequest
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public DateTime DateOfBirth { get; init; }

        // Existing customer fields...
        public string? AccountType { get; set; }
        public decimal InitialDeposit { get; set; } = 0m;
        public string Currency { get; set; } = "NGN";
    
}