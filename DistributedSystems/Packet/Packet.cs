using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DefaultNamespace;


public readonly struct PacketKey<TValue>(string key)
{
    public string Key { get; } = key;
}

public class Packet
{
    private IDictionary<string, object?> _values;

    public Packet() : this(new Dictionary<string, object?>())
    {
    }

    private Packet(IDictionary<string, object?>? values)
    {
        _values = values;
    }

    public void Set<TValue>(PacketKey<TValue> key, TValue value) 
        => _values[key.Key] = value;

    public bool TryGetValue<TValue>(PacketKey<TValue> key, [MaybeNullWhen(false)] out TValue value)
    {
        if (_values.TryGetValue(key.Key, out object? _value) && _value is TValue tvalue)
        {
            value = tvalue;
            return true;
        }

        if (_value is JsonElement)
        {
            value = ((JsonElement)_value).Deserialize<TValue>();
            return value != null;
        }

        value = default;
        return false;
    }

    public TValue? ValueOrDefault<TValue>(PacketKey<TValue> key) 
        => TryGetValue(key, out var val) ? val : default;

    public TValue Get<TValue>(PacketKey<TValue> key) => ValueOrDefault(key)!;

    public static Packet From(IDictionary<string, object?>? deserialize)
    {
        return new Packet(deserialize);
    }
}