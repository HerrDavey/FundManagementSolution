using Fundusze.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Fundusze.Domain.Interfaces;
using Fundusze.Infrastucture;
using Serilog;
using Fundusze.Application.Services;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// Definicja CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7074")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});


builder.Services.AddDbContext<FundsDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging();

app.UseRouting();


app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();
app.MapControllers();

app.Run();