namespace DefaultNamespace;


public readonly struct PacketKey<TValue>(string key);
public class Packet
{
    private object? _value;
    public void Set<TValue>(PacketKey<TValue> key, TValue foo)
    {
       _value = foo; 
    }

    public TValue? Get<TValue>(PacketKey<TValue> key)
    {
        return (TValue?)_value;
    }
}