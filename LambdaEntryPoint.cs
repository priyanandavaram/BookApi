using Amazon.Lambda.AspNetCoreServer;
using Microsoft.Extensions.Hosting;

namespace BookApi;

public class LambdaEntryPoint : APIGatewayHttpApiV2ProxyFunction
{
    // This tells the Lambda adapter how to build the app host when running in Lambda.
    protected override void Init(IHostBuilder builder)
    {
        // For a simple app we let the default builder use Program.cs
        builder.ConfigureWebHostDefaults(webBuilder =>
        {
            // Use the same Program.cs pipeline
            webBuilder.UseStartup<Startup>();
        });
    }
}
