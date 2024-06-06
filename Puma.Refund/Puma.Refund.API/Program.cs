using Puma.Refund.Extensions.EndpointModules;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = LogIntegrationsExtensions.ConfigureStructuralLogWithSerilog();
builder.Logging.AddSerilog(Log.Logger);

try
{
    var configuration = builder.Configuration;

    #region configuracoes das extensoes

    /*A extension do application Insights só deve ser inserida quando a chave estiver no appSettings
    *.AddApplicationInsightsApiTelemetry(configuration)
    * 
    *Caso seja necessário usar a extension para resiliência em chamadas htttp ativar a extension
    *AddWorkerResiliencesPatterns
    * 
    * Existe uma implementação de log mais simples usando o padrão
    * do aspnetCore. Caso queira usa-lo 
    * as configurações do serilog devem ser retiradas e o serviço de log deve ser reimplementado
    * o try catch não é mais necessário
    * retirar a execução da extension :
    * adicionar na pilha a extension AddMinimalApiAspNetCoreHttpLogging
    */

    builder.Services.AddEndpointsApiExplorer()
                    .AddBaseConfigurationOptionsPattern(configuration)
                    .AddSwaggerDocumentation(configuration)
                    .AddLogServiceDependencies()
                    .AddNotificationControl()
                    .AddRequestResponseCompress()
                    .AddDependencyInjections()
                    .AddApiCustomResults()
                    .AddGlobalExceptionHandlerMiddleware()
                    .AddFilterToSystemLogs()
                    .AddMinimalApiVersionsing()
                    .AddEndpointModuleExtensions();

    #endregion

    var app = builder.Build();

    #region configuracoes dos middlewares

    app.UseResponseCompression()
       .UseExceptionHandler()
       .UseMiddleware<SerilogRequestLoggerMiddleware>()
       .UseSwagger()
       .UseSwaggerUI()
       .UseHttpsRedirection();

    #region caso haja autenticação na api descomentar o código 

    //app.UseRouting()
    //   .UseMiddleware<UnauthorizedTokenMiddleware>()
    //   .UseAuthentication()
    //   .UseAuthorization();

    #endregion

    #endregion

    app.MapEndpointModules();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminado inexperadamente.");
}
finally
{
    Log.CloseAndFlush();
}