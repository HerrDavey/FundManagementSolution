using Fundusze.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Fundusze.Domain.Interfaces;
using Fundusze.Infrastucture;
using Serilog; // Dodajemy using

// Konfiguracja Seriloga odczytywana z appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build())
    .CreateLogger();

try
{
    Log.Information("Aplikacja WebAPI uruchamia siê.");

    var builder = WebApplication.CreateBuilder(args);

    // U¿ywamy Seriloga jako dostawcy logów
    builder.Host.UseSerilog();

    // Add services to the container.

    // Dodanie FundsDbContext
    builder.Services.AddDbContext<FundsDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Rejestracja UnitOfWork (zamiast pojedynczych repozytoriów)
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    // Dodajemy middleware Seriloga do logowania ¿¹dañ HTTP
    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplikacja WebAPI nie mog³a siê uruchomiæ.");
}
finally
{
    Log.CloseAndFlush();
}