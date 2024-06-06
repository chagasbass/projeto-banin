using Puma.Refund.API.Domain.Entities;

namespace Puma.Refund.API.Domain.Repositories;

public interface IRefundDataRepository
{
    Task<RefundData?> AddRefundDataAsync(RefundData refundData);
    Task<IEnumerable<RefundData>> GetAllRefundDataAsync();
}
