namespace Blazor.Code.Shared.Model;

public class ComponentList
{
    public string Title { get; set; }

    public string? Data { get; set; }

    public ComponentList(string title, string? data)
    {
        Title = title;
        Data = data;
    }

    public ComponentList()
    {
    }

    public List<ComponentList> Childrens { get; set; } = new List<ComponentList>();
}
