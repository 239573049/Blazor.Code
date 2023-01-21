using System.Collections.Concurrent;

namespace Blazor.Code.Shared;

public class KeyEventBus<T>
{
    private readonly ConcurrentDictionary<string, List<Action<T>>> _concurrentDictionary;

    public KeyEventBus()
    {
        _concurrentDictionary = new ConcurrentDictionary<string, List<Action<T>>>();
    }

    public async Task PushAsync(string name, T entity)
    {
        _concurrentDictionary.TryGetValue(name, out var value);

        value?.ForEach(x =>
        {
            x.Invoke(entity);
        });
        await Task.CompletedTask;
    }

    public void Subscription(string name, Action<T> func)
    {
        if (_concurrentDictionary.TryGetValue(name, out var list))
        {
            list?.Add(func);
        }
        else
        {
            _concurrentDictionary.TryAdd(name, new List<Action<T>>()
            {
                func
            });
        }
    }

    /// <inheritdoc />
    public void Remove(string name)
    {
        _concurrentDictionary.Remove(name, out _);
    }
}