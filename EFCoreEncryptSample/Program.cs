using EFCoreEncryptSample;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options=>options.UseSqlServer("Server=.;Database=EncryptDb;User ID=sa;Password=abc_123;MultipleActiveResultSets=true;TrustServerCertificate=true"));

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapGet("api/student", async (AppDbContext context) =>
{
    var students = await context.Students.AsNoTracking().ToListAsync();
    return Results.Ok(students);
});
app.MapPost("api/student", async (Student student, AppDbContext context) =>
{
    await context.Students.AddAsync(student);
    await context.SaveChangesAsync();
    return Results.Ok(student);
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
