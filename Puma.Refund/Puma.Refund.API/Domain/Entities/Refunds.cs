namespace Puma.Refund.API.Domain.Entities;

public class Refunds
{
    public string? Live { get; set; }
    public List<NotificationItem>? NotificationItems { get; set; }

    public Refunds()
    {
        NotificationItems = new List<NotificationItem>();
    }
}