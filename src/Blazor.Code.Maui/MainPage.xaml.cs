using Blazor.Code.Shared.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Blazor.Code.Maui;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        // 添加绑定razor组件标签
        blazorWebView.RootComponents.RegisterAsCustomElement<DynamicRendering>("render-razor");
        blazorWebView.RootComponents.RegisterAsCustomElement<GlobalUsing>("global-using");
        blazorWebView.RootComponents.RegisterAsCustomElement<AssemblyLoad>("assembly-load");
    }
}
