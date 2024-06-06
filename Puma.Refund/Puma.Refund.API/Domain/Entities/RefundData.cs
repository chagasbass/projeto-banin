namespace Puma.Refund.API.Domain.Entities;

public class RefundData(string? pspReference, string? merchantReference, string? contract)
{
    public string? PspReference { get; set; } = pspReference;
    public string? MerchantReference { get; set; } = merchantReference;
    public string? Contract { get; set; } = contract;
}