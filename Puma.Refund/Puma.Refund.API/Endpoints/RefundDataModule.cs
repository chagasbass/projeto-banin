using Carter;
using Puma.Refund.API.Domain.Entities;
using Puma.Refund.API.Domain.Repositories;
using System.Text.Json;

namespace Puma.Refund.API.Endpoints;

public class RefundDataModule : ICarterModule
{
    private static ApiVersionSet VersionEndpoints(IEndpointRouteBuilder app)
    {
        return app.NewApiVersionSet()
                   .HasApiVersion(new ApiVersion(1))
                   .ReportApiVersions()
                   .Build();
    }

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var versionamento = VersionEndpoints(app);

        #region Adição de refund

        app.MapPost("/v{version:apiVersion}/refunds", async (IApiCustomResults customResults,
                               IRefundDataRepository refundDataRespository,
                               INotificationServices notificationServices,
                                Refunds refunds) =>
        {
            var newRedundData = new RefundData(refunds.NotificationItems.First().NotificationRequestItem.PspReference,
                                                refunds.NotificationItems.First().NotificationRequestItem.MerchantReference,
                                                JsonSerializer.Serialize(refunds));

            var insertedRefuns = await refundDataRespository.AddRefundDataAsync(newRedundData);

            if (notificationServices.HasNotifications())
            {
                notificationServices.AddStatusCode(StatusCodeOperation.BadRequest);

                return customResults.FormatApiResponse(new CommandResult(notificationServices.GetNotifications(), false, "Erros na operação"));
            }

            notificationServices.AddStatusCode(StatusCodeOperation.Created);
            return customResults.FormatApiResponse(new CommandResult(insertedRefuns, true, "Inserido com sucesso"), "v1/refunds");

        }).Produces<CommandResult>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
          .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
          .WithName("Refunds")
          .WithTags("Refunds")
          .WithDescription("")
          .WithSummary("Add a new Refund")
          .WithOpenApi()
          .WithApiVersionSet(versionamento)
          .MapToApiVersion(1);

        #endregion

        #region listagem de refunds
        app.MapGet("/v{version:apiVersion}/refunds", async (IApiCustomResults customResults,
                             IRefundDataRepository refundDataRespository,
                             INotificationServices notificationServices) =>
        {

            var refunds = await refundDataRespository.GetAllRefundDataAsync();

            if (notificationServices.HasNotifications())
            {
                notificationServices.AddStatusCode(StatusCodeOperation.BadRequest);

                return customResults.FormatApiResponse(new CommandResult(notificationServices.GetNotifications(), false, "Erros na operação"));
            }

            if (!refunds.Any())
            {
                notificationServices.AddStatusCode(StatusCodeOperation.NotFound);
                return customResults.FormatApiResponse(new CommandResult(true, "A pesquisa não retornou resultados"));
            }

            notificationServices.AddStatusCode(StatusCodeOperation.OK);
            return customResults.FormatApiResponse(new CommandResult(refunds, true));

        }).Produces<CommandResult>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest, typeof(ProblemDetails))
        .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
        .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
        .WithName("Refunds-All")
        .WithTags("RefundsAll")
        .WithDescription("")
        .WithSummary("Get all Refunds")
        .WithOpenApi()
        .WithApiVersionSet(versionamento)
        .MapToApiVersion(1);


        #endregion
    }
}
