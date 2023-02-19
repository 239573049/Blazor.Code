using Masa.Blazor.Extensions.Languages.Razor;
namespace Microsoft.Extensions.DependencyInjection;

public static class BlazorComponentsExtension
{

    public static IServiceCollection AddComponent(this IServiceCollection services)
    {
        services.AddMasaBlazor(options =>
        {
            options.ConfigureTheme(theme =>
            {
                theme.Themes.Light.Primary = "#4318FF";
                theme.Themes.Light.Secondary = "#5CBBF6";
                theme.Themes.Light.Accent = "#005CAF";
                theme.Themes.Light.UserDefined["Tertiary"] = "#e57373";
            });
        });
        services.AddSemiDesignBlazorMonacoEditor();
        services.AddScoped<HelperJsInterop>();

        // 默认添加 全局引用
        CompileRazorProjectFileSystem.AddGlobalUsing("@using Masa.Blazor");
        CompileRazorProjectFileSystem.AddGlobalUsing("@using BlazorComponent");
        CompileRazorProjectFileSystem.AddGlobalUsing("@using Masa.Blazor.Presets");

        return services;
    }

}