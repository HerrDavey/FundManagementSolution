using Fundusze.UI_Blazor.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // W³¹czamy tryb interaktywny dla serwera

// Rejestracja MudBlazor
builder.Services.AddMudServices();

// Rejestracja HttpClient, który bêdzie ³¹czy³ siê z naszym API
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