using System.Text.Json;
using Masa.Blazor.Extensions.Languages.Razor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Semi.Design.Blazor;

namespace Blazor.Components.Pages;

public partial class Render
{
    private SMonacoEditor? _monacoEditor;

    [Parameter][SupplyParameterFromQuery] public string? Path { get; set; }

    private string Code;

    private object Options;

    private Type? ComponentType;

    /// <summary>
    /// 项目GitHub地址
    /// </summary>
    private const string GitHub = "https://github.com/239573049/Blazor.Code/tree/master";

    /// <summary>
    /// 编译引用的程序集
    /// </summary>
    private static readonly List<PortableExecutableReference> PortableExecutableReferences = new();

    /// <summary>
    /// 加载程序集弹窗
    /// </summary>
    private bool loadAssemblyVisible = false;

    /// <summary>
    /// 需要被加载的程序集地址
    /// </summary>
    private string AssemblyPath = string.Empty;

    /// <summary>
    /// 是否第一次加载
    /// </summary>
    private static bool first = true;

    /// <summary>
    /// GitHub Token
    /// </summary>
    private string GitHubToken;

    /// <summary>
    /// 设置弹窗是否显示
    /// </summary>
    private bool settingModalVisible;

    /// <summary>
    /// 运行按钮加载器
    /// </summary>
    private bool RunCodeLoading = false;

    /// <summary>
    /// 编译器需要使用的程序集
    /// </summary>
    private static List<string> Assemblys = new()
    {
        "BlazorComponent",
        "Masa.Blazor",
        "OneOf",
        "FluentValidation",
        "netstandard",
        "FluentValidation.DependencyInjectionExtensions",
        "System",
        "Microsoft.AspNetCore.Components",
        "System.Linq.Expressions",
        "System.Net.Http.Json",
        "System.Private.CoreLib",
        "Microsoft.AspNetCore.Components.Web",
        "System.Collections",
        "System.Linq",
        "System.Runtime"
    };

    public static List<string> LoadAssembly;

    private void OnCancel()
    {
        settingModalVisible = false;
    }

    private DotNetObjectReference<Render>? objRef;
    private void OnSave()
    {
        settingModalVisible = false;
    }

    protected override async Task OnInitializedAsync()
    {
        objRef = DotNetObjectReference.Create(this);
        Options = new
        {
            value = @"<MCard Class=""overflow-hidden"">
    <MAppBar Absolute
             Color=""#6A76AB""
             Dark
             ShrinkOnScroll
             Prominent
             Src=""https://picsum.photos/1920/1080?random""
             FadeImgOnScroll
             ScrollTarget=""#scrolling-sheet-3"">
        <ImgContent>
            <MImage Gradient=""to top right, rgba(100,115,201,.7), rgba(25,32,72,.7)"" @attributes=""@context""></MImage>
        </ImgContent>

        <ChildContent>
            <MAppBarNavIcon></MAppBarNavIcon>
            <MAppBarTitle>Title</MAppBarTitle>
            <MSpacer></MSpacer>

            <MButton Icon>
                <MIcon>mdi-magnify</MIcon>
            </MButton>
            <MButton Icon>
                <MIcon>mdi-heart</MIcon>
            </MButton>
            <MButton Icon>
                <MIcon>mdi-dots-vertical</MIcon>
            </MButton>

        </ChildContent>
        <ExtensionContent>
            <MTabs AlignWithTitle>
                <MTab>Tab 1</MTab>
                <MTab>Tab 2</MTab>
                <MTab>Tab 3</MTab>
            </MTabs>
        </ExtensionContent>
    </MAppBar>

    <MSheet Class=""overflow-y-auto"" MaxHeight=""600"" Id=""scrolling-sheet-3"">
        <MContainer Style=""height: 1000px;""></MContainer>
    </MSheet>
</MCard>", // Initial code
            language = "razor", // Syntactic support language
            automaticLayout = true, // Automatically ADAPTS to parent container size
            theme = "vs-dark" // monaco theme 
        };

        await GetReferenceAsync();

        await base.OnInitializedAsync();
    }

    private async Task GotoUrl(string url)
    {
        await JSRuntime.InvokeVoidAsync("window.open", url, "_blank");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            var value = (await JSRuntime.InvokeAsync<string>("window.localStorage.getItem", "assembly")).Replace("'","");
            LoadAssembly = JsonSerializer.Deserialize<List<string>>(value);

        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadAssemblyAsync()
    {
        if (string.IsNullOrEmpty(AssemblyPath))
        {
            return;
        }

        try
        {
            var stream = await HttpClient.GetStreamAsync(AssemblyPath);
            if (stream?.Length > 0)
            {
                PortableExecutableReferences?.Add(MetadataReference.CreateFromStream(stream));
                await PopupService.ToastSuccessAsync("加载程序集成功");
            }
            else
            {
                await PopupService.ToastErrorAsync("加载程序集异常");
            }
        }
        catch (Exception)
        {
            await PopupService.ToastErrorAsync("加载程序集异常");
        }
    }

    /// <summary>
    /// 加载代码文件
    /// 支持GitHub
    /// </summary>
    /// <returns></returns>
    private async Task GetCode()
    {
        if (string.IsNullOrEmpty(Path))
        {
            return;
        }

        Path = Path.Replace("http://", "https://").Replace("https://github.com/", "https://raw.githubusercontent.com/")
            .Replace("/blob/", "/");
        if (!Path.StartsWith("https://raw.githubusercontent.com/"))
        {
            Path = "https://raw.githubusercontent.com/" + Path;
        }

        try
        {
            var httpResponseMessage = await HttpClient!.GetAsync(Path);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var code = await httpResponseMessage.Content.ReadAsStringAsync();
                await _monacoEditor.SetValue(code);
                if (!string.IsNullOrEmpty(code))
                {
                    Code = code;
                    _ = Task.Run(async () => { await RunCode(); });
                }
            }
            else
            {
                await PopupService.ToastErrorAsync("加载文件内容出现异常，异常状态码：" + httpResponseMessage.StatusCode);
            }
        }
        catch (Exception e)
        {
            await PopupService.ToastErrorAsync("加载文件内容出现异常，异常状态码：" + e.Message);
        }
    }

    /// <summary>
    /// 执行代码
    /// </summary>
    /// <returns></returns>
    private async Task RunCode()
    {
        RunCodeLoading = true;
        // 不等待加载动画无法执行
        await Task.Delay(10);
        var code = await _monacoEditor.GetValue();
        if (!string.IsNullOrEmpty(code))
        {
            try
            {
                ComponentType = RazorCompile.CompileToType(new CompileRazorOptions()
                {
                    Code = code
                });
                _ = InvokeAsync(async () =>
                {
                    StateHasChanged();
                    await PopupService.ToastInfoAsync("动态编辑代码成功！");
                });
            }
            catch (Exception e)
            {
                await PopupService.ToastErrorAsync("编译异常：" + e.Message);
            }
        }

        RunCodeLoading = false;
    }

    /// <summary>
    /// 加载编译所需依赖
    /// </summary>
    /// <returns></returns>
    private async Task GetReferenceAsync()
    {
        if (!first)
        {
            return;
        }

        await PopupService.ToastInfoAsync("首次运行加载组件引用,请稍等...");

        foreach (var assembly in Assemblys)
        {
            try
            {
                await using var stream = await HttpClient!.GetStreamAsync($"_framework/{assembly}.dll");
                if (stream?.Length > 0)
                {
                    PortableExecutableReferences?.Add(MetadataReference.CreateFromStream(stream));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        RazorCompile.Initialized(PortableExecutableReferences, GetRazorExtension);
        await PopupService.ToastInfoAsync("首次运行加载组件引用成功！");
        first = false;
    }

    private static List<RazorExtension> GetRazorExtension
        => typeof(BlazorComponentsExtension).Assembly.GetReferencedAssemblies()
            .Select(asm => new AssemblyExtension(asm.FullName, AppDomain.CurrentDomain.Load(asm.FullName)))
            .Cast<RazorExtension>().ToList();
}