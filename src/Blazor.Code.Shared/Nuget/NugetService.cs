using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor.Code.Shared.Nuget;

public class NugetService
{
    private readonly HttpClient _httpClient;

    public NugetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<NugetListDto> SearchAsync(string q, int skip = 0, int take = 20, string? semVerLevel = "2.0.0",
        bool prerelease = false)
    {
        return await
            _httpClient.GetFromJsonAsync<NugetListDto>(
                $"https://azuresearch-usnc.nuget.org/query?q={q}&skip={skip}&take={take}&prerelease={prerelease}&semVerLevel={semVerLevel}");
    }
}