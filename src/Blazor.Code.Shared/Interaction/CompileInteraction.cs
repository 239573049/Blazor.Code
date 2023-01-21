using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Blazor.Code.Shared;

public static class CompileInteraction
{
    /// <summary>
    /// 编译Razor
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    [JSInvokable]
    public static async Task CompileRazor(string code)
    {
        var keyEventBus = BlazorCodeSharedExtension.ServiceProvider?.GetRequiredService<KeyEventBus<string>>();

        await keyEventBus?.PushAsync(BlazorCodeConstant.RenderComponent, code);
    }
}