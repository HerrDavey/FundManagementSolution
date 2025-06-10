using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Fundusze.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Ta linia mówi Blazorowi, ¿eby wyrenderowa³ komponent <App> w elemencie <div id="app">
builder.RootComponents.Add<App>("#app");
// Ta linia dodaje obs³ugê tagów <PageTitle> i <HeadContent>
builder.RootComponents.Add<HeadOutlet>("head::after");

// Rejestracja MudBlazor
builder.Services.AddMudServices();

// Rejestracja HttpClient
builder.Services.AddScoped(sp => new HttpClient
{
    // Upewnij siê, ¿e ten port jest zgodny z Twoim WebAPI
    BaseAddress = new Uri("https://localhost:7033")
});

await builder.Build().RunAsync();