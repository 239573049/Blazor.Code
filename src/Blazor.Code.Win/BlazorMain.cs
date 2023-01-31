using Blazor.Code.Shared.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Code.Win;

public partial class BlazorMain : Form
{
    public BlazorMain()
    {
        InitializeComponent();

        var services = new ServiceCollection();
        services.AddWindowsFormsBlazorWebView();
        services.AddBlazorCodeShared(Shared.CodeEnvironment.Server);
        blazor.RootComponents.RegisterAsCustomElement<DynamicRendering>("render-razor");
        blazor.RootComponents.RegisterAsCustomElement<GlobalUsing>("global-using");
        blazor.RootComponents.RegisterAsCustomElement<AssemblyLoad>("assembly-load");
        blazor.HostPage = "wwwroot\\index.html";
        blazor.Services = services.BuildServiceProvider();

        blazor.Services.UseServiceProvider();
    }
}
