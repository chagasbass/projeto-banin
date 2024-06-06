namespace Puma.Refund.API.Domain.Entities;

public class NotificationRequestItem
{
    public AdditionalData? AdditionalData { get; set; }
    public Amount? Amount { get; set; }
    public string? PspReference { get; set; }
    public string? EventCode { get; set; }
    public DateTime EventDate { get; set; }
    public string? MerchantAccountCode { get; set; }
    public List<string>? Operations { get; set; }
    public string? MerchantReference { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Success { get; set; }

    public NotificationRequestItem() { }
    
}

