using System.Diagnostics.CodeAnalysis;

namespace DefaultNamespace;


public readonly struct PacketKey<TValue>(string key)
{
    public string Key { get; } = key;
}

public class Packet
{
    private IDictionary<string, object?> _values = new Dictionary<string, object?>();
    public void Set<TValue>(PacketKey<TValue> key, TValue value) 
        => _values[key.Key] = value;

    public bool TryGetValue<TValue>(PacketKey<TValue> key, [MaybeNullWhen(false)] out TValue value)
    {
        if (_values.TryGetValue(key.Key, out object? _value) && _value is TValue tvalue)
        {
            value = tvalue;
            return true;
        }

        value = default;
        return false;
    }

    public TValue? ValueOrDefault<TValue>(PacketKey<TValue> key) 
        => TryGetValue(key, out var val) ? val : default;
}