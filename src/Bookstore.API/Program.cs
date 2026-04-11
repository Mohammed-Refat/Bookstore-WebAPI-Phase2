using Bookstore.Application.Services;
using Bookstore.Application.Validators;
using Bookstore.Infrastructure.Repositories;
using Bookstore.Domain.Interfaces;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers & Swagger ────────────────────────────────────────
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ── DI Registrations ─────────────────────────────────────────────
// "When anyone asks for IBookRepository, give them InMemoryBookRepository"
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();

// "When anyone asks for IBookService, give them BookService"
builder.Services.AddScoped<IBookService, BookService>();

// Register all FluentValidation validators from the Application project
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookRequestValidator>();

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();