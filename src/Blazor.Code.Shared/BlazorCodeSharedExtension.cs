using Blazor.Code.Shared;
using Blazor.Code.Shared.Nuget;
using Masa.Blazor.Extensions.Languages.Razor;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection;

public static class BlazorCodeSharedExtension
{
    public static IServiceProvider ServiceProvider { get; private set; }

    public static CodeEnvironment CodeEnvironment
    {
        get;
        private set;
    }

    public static List<PortableExecutableReference> PortableExecutableReferences = new();

    private static List<string> Assemblys = new List<string>()
    {
        "WindowsBase",
        "System.Xml",
        "System.IO.FileSystem",
        "System.Xml.XmlSerializer",
        "System.Xml.XmlDocument",
        "System.Xml.XPath",
        "System.Xml.XPath.XDocument",
        "System.Xml.XDocument",
        "System.Xml.Serialization",
        "System.Private.Xml.Linq",
        "System.Drawing",
        "Microsoft.Win32.Registry",
        "Microsoft.Win32.Primitives",
        "System.Xml.ReaderWriter",
        "Microsoft.VisualBasic",
        "Microsoft.VisualBasic.Core",
        "System.Xml.Linq",
        "Microsoft.VisualBasic.Core",
        "Masa.Blazor.Extensions.Languages.Razor",
        "System.Windows",
        "System.IO.Compression.dll",
        "System.Private.Xml",

    };

    public static async void AddBlazorCodeShared(this IServiceCollection services, CodeEnvironment environment = CodeEnvironment.WebAssembly)
    {
        CodeEnvironment = environment;
        // 默认添加 全局引用
        CompileRazorProjectFileSystem.AddGlobalUsing("@using Masa.Blazor");
        CompileRazorProjectFileSystem.AddGlobalUsing("@using BlazorComponent");
        CompileRazorProjectFileSystem.AddGlobalUsing("@using Masa.Blazor.Presets");

        services.AddMasaBlazor();

        services.AddSingleton(typeof(KeyEventBus<>));
        services.AddScoped<NugetService>();

        await Task.Run(async () =>
        {
            await GetReferenceAsync(services);
            RazorCompile.Initialized(PortableExecutableReferences, GetRazorExtension());
        });
    }

    static async Task GetReferenceAsync(IServiceCollection services)
    {
        HttpClient? httpClient = null;

        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            try
            {
                if(Assemblys.Any(x=>assembly.GetName().Name == x))
                {
                    return;
                }
                if (CodeEnvironment == CodeEnvironment.WebAssembly)
                {
                    if (httpClient == null)
                    {
                        httpClient = services.BuildServiceProvider().GetService<HttpClient>();
                    }
                    using var stream = await httpClient!.GetStreamAsync($"_framework/{assembly.GetName().Name}.dll");
                    if (stream.Length > 0)
                    {
                        PortableExecutableReferences?.Add(MetadataReference.CreateFromStream(stream));
                    }
                }
                else
                {
                    // Server是在服务器运行可以直接获取文件 如果是嵌入到maui或者winform的也可以直接获取
                    PortableExecutableReferences?.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }

    static List<RazorExtension> GetRazorExtension()
    {
        var exits = new List<RazorExtension>();

        foreach (var asm in typeof(BlazorCodeSharedExtension).Assembly.GetReferencedAssemblies())
        {
            exits.Add(new AssemblyExtension(asm.FullName, AppDomain.CurrentDomain.Load(asm.FullName)));
        }

        return exits;
    }

    /// <summary>
    /// 添加服务提供
    /// </summary>
    /// <param name="serviceProvider"></param>
    public static void UseServiceProvider(this IServiceProvider serviceProvider)
        => ServiceProvider = serviceProvider;
}