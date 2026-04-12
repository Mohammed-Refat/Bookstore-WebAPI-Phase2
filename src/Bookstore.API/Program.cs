using Bookstore.Application.Services;
using Bookstore.Application.Validators;
using Bookstore.Infrastructure.Repositories;
using Bookstore.Domain.Interfaces;
using FluentValidation;
using Bookstore.API.Middleware;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

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


// ── Global Error Handler ──────────────────────────────────────────
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ── Rate Limiting ─────────────────────────────────────────────────
builder.Services.AddRateLimiter(options =>
{
    options.AddSlidingWindowLimiter("per-user", limiterOptions =>
    {
        // Max 10 requests per 10-second sliding window
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromSeconds(10);
        limiterOptions.SegmentsPerWindow = 2;
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0; // no queuing — reject immediately
    });

    // Return 429 Too Many Requests with Problem Details when limit is hit
    options.OnRejected = async (context, ct) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            title = "Too Many Requests",
            status = 429,
            detail = "You have exceeded the request limit. Please slow down.",
            instance = context.HttpContext.Request.Path
        }, ct);
    };
});

var app = builder.Build();

// ── Middleware Pipeline ───────────────────────────────────────────

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();

// Makes Program class visible to the test project
public partial class Program { }