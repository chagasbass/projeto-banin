namespace Puma.Refund.API.Domain.Entities;

public class Amount
{
    public int Value { get; set; }
    public string? Currency { get; set; }

    public Amount() { }

}

