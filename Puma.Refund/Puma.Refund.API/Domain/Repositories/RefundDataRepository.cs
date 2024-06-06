using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Puma.Refund.API.Domain.Entities;
using Puma.Refund.API.QueryHelpers;
using Puma.Refund.Extensions.Shared.Configurations;
using Puma.Refund.Extensions.Shared.LogFilters.Services;

namespace Puma.Refund.API.Domain.Repositories;

public class RefundDataRepository(ILogServices logServices,
                                  INotificationServices notificationServices,
                                  IOptions<BaseConfigurationOptions> options) : IRefundDataRepository
{


    public async Task<RefundData?> AddRefundDataAsync(RefundData refundData)
    {
        try
        {
            var parametro = new
            {
                nome = refundData.PspReference,
                telefone = refundData.MerchantReference,
                email = refundData.Contract
            };

            using var connection = new SqlConnection(options.Value.StringConexaoBancoDeDados);
            await connection.OpenAsync();

            await connection.ExecuteAsync(RefundDataQueryHelper.AddRefundData(), parametro, commandType: CommandType.Text);

            return refundData;
        }
        catch (Exception ex)
        {
            logServices.LogData.AddException(ex);
            logServices.WriteLogWhenRaiseExceptions();

            notificationServices.AddNotification(new Flunt.Notifications.Notification("RefundData-Insert", "Problemas na inserção do dado"));

            return default;
        }
    }

    public async Task<IEnumerable<RefundData>> GetAllRefundDataAsync()
    {
        try
        {
            using var connection = new SqlConnection(options.Value.StringConexaoBancoDeDados);
            await connection.OpenAsync();

            var refunds = await connection.QueryAsync<RefundData>(RefundDataQueryHelper.GetAllRefundData(), commandType: CommandType.Text);

            return refunds;

        }
        catch (Exception ex)
        {
            logServices.LogData.AddException(ex);
            logServices.WriteLogWhenRaiseExceptions();

            notificationServices.AddNotification(new Flunt.Notifications.Notification("RefundData-Select", "Problemas na listagem do dado"));

            return [];
        }
    }
}