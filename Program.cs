using BookApi.Models;
using BookApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<BookService>();

var app = builder.Build();

app.MapPost("/books", (Book book, BookService svc) =>
{
    svc.Add(book);
    return Results.Created($"/books/{book.Id}", book);
});

app.MapGet("/books", (BookService svc) => Results.Ok(svc.GetAll()));

app.MapPost("/books/{id}/checkout", (int id, BookService svc) =>
{
    return svc.Checkout(id) ? Results.Ok() : Results.NotFound();
});

app.MapPost("/books/{id}/return", (int id, BookService svc) =>
{
    return svc.Return(id) ? Results.Ok() : Results.NotFound();
});

app.Run();
