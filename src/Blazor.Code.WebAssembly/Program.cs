using Blazor.Code.Shared.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.RegisterAsCustomElement<DynamicRendering>("render-razor");
builder.RootComponents.RegisterAsCustomElement<GlobalUsing>("global-using");
builder.RootComponents.RegisterAsCustomElement<AssemblyLoad>("assembly-load");

builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazorCodeShared();

var app = builder.Build();


app.Services.UseServiceProvider();

await app.RunAsync();
