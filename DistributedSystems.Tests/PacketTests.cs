using DefaultNamespace;

namespace DistributedSystems.Tests;

public class PacketTests
{
    [Fact]
    public void SetAndGet()
    {
        var key = new PacketKey<string>("Test");
        var value = Any.String();
        
        var packet = new Packet();
        packet.Set(key, value);
        
        Assert.Equal(value, packet.Get(key));
    }

    static class Any
    {
        public static string String()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
