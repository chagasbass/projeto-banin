using System.Text;

namespace Puma.Refund.API.QueryHelpers;

public static class RefundDataQueryHelper
{
    public static string AddRefundData()
    {
        var query = new StringBuilder();
        query.AppendLine("INSERT INTO REFUND_DATA (nome,telefone,email) VALUES(@nome,@telefone,@email)");

        return query.ToString();
    }

    public static string GetAllRefundData()
    {
        var query = new StringBuilder();
        

        return query.ToString();
    }
}
