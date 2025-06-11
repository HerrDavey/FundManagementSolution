using Fundusze.BlazorAppServer.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options => options.DetailedErrors = true);

// Rejestracja MudBlazor
builder.Services.AddMudServices();

// Rejestracja HttpClient
builder.Services.AddScoped(sp =>
{
    var apiUrl = builder.Configuration["ApiBaseUrl"]
                 ?? throw new InvalidOperationException("ApiBaseUrl is not configured.");
    return new HttpClient { BaseAddress = new Uri(apiUrl) };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();