using DefaultNamespace;
using static DistributedSystems.Tests.PacketTests.Any;

namespace DistributedSystems.Tests;

public class PacketTests
{
    [Fact]
    public void SetAndGet()
    {
        var key = new PacketKey<string>("Test");
        var value = AString();
        
        var packet = new Packet();
        packet.Set(key, value);
        
        Assert.Equal(value, packet.ValueOrDefault(key));
    }

    [Fact]
    public void SetAndGetWithDifferentKey()
    {
        var packet = new Packet();
        packet.Set(AKey(), AString());

        var value = packet.ValueOrDefault(AKey());
        Assert.Null(value);
    }

    [Fact]
    public void SetGetAndGetWithDifferentKeyAndValue()
    {
        var packet = new Packet();
        packet.Set(AKey(), AString());
        var key = AKey();
        var value = AString();
        packet.Set(key, value);

        Assert.Equal(value, packet.ValueOrDefault(key));
    }


    [Fact]
    public void AnyKeyReturnNull()
    {
        var packet = new Packet();
        
        Assert.Null(packet.ValueOrDefault(AKey()));
    }
    
    [Fact]
    public void AnyKeyReturnFalse()
    {
        var packet = new Packet();
        
        Assert.False(packet.TryGetValue(AKey(), out _));
    }

    internal static class Any
    {
        public static string AString() 
            => Guid.NewGuid().ToString();

        public static PacketKey<string> AKey() 
            => new(AString());
    }
}
