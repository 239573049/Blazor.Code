using Microsoft.JSInterop;

namespace Blazor.Components;


public class HelperJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> moduleTask;

    public HelperJsInterop(IJSRuntime jsRuntime)
    {
        moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "./_content/Blazor.Components/helperJsInterop.js").AsTask());
    }

    public async ValueTask OpenAssembly<T>(string code, DotNetObjectReference<T> objectReference) where T : class
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("openAssembly", code, objectReference);
    }

    public async ValueTask RevokeObjectURL(string url)
    {
        var module = await moduleTask.Value;
        await module.InvokeVoidAsync("revokeObjectURL", url);
    }

    public async ValueTask DisposeAsync()
    {
        if (moduleTask.IsValueCreated)
        {
            var module = await moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}
