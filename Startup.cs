using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BookApi;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Services.BookService>();
    }

    public void Configure(IApplicationBuilder app)
    {
        // Recreate the same minimal endpoints used by Program.cs.
        // (When running inside Lambda the Program.cs-based web host is not executed,
        // so we register endpoints here)
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapPost("/books", async context =>
            {
                var book = await context.Request.ReadFromJsonAsync<Models.Book>();
                if (book is null) { context.Response.StatusCode = 400; return; }
                var svc = context.RequestServices.GetRequiredService<Services.BookService>();
                svc.Add(book);
                context.Response.StatusCode = 201;
                await context.Response.WriteAsJsonAsync(book);
            });

            endpoints.MapGet("/books", async context =>
            {
                var svc = context.RequestServices.GetRequiredService<Services.BookService>();
                await context.Response.WriteAsJsonAsync(svc.GetAll());
            });

            endpoints.MapPost("/books/{id}/checkout", async context =>
            {
                var idStr = context.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out var id)) { context.Response.StatusCode = 400; return; }
                var svc = context.RequestServices.GetRequiredService<Services.BookService>();
                if (svc.Checkout(id)) { context.Response.StatusCode = 200; return; }
                context.Response.StatusCode = 404;
            });

            endpoints.MapPost("/books/{id}/return", async context =>
            {
                var idStr = context.Request.RouteValues["id"]?.ToString();
                if (!int.TryParse(idStr, out var id)) { context.Response.StatusCode = 400; return; }
                var svc = context.RequestServices.GetRequiredService<Services.BookService>();
                if (svc.Return(id)) { context.Response.StatusCode = 200; return; }
                context.Response.StatusCode = 404;
            });
        });
    }
}
