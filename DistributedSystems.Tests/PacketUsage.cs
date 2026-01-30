using System.Text.Json;
using DefaultNamespace;

namespace DistributedSystems.Tests;

public static class CarExtensions
{
    private static PacketKey<string> Id = new(nameof(Id));
        
    extension(Packet packet)
    {
        public string ID
        {
            get => packet.Get(Id);
            set => packet.Set(Id, value);
        }
    }
}

public class PacketUsage
{
    [Fact]
    public void WorksWithJsonElement()
    {
        var packet = Packet.From(JsonData());

        Assert.Null(packet.Get(PacketTests.Any.AKey()));
        Assert.Equal("Foo", packet.ID);
    }

    [Fact]
    public void SetToJson()
    {
        var packet = Packet.From(JsonData());
        Assert.Equal("Foo", packet.ID);
        
        packet.ID = "Bar";
        
        Assert.Equal("Bar", packet.ID);
    }

    private static IDictionary<string, object?>? JsonData()
    {
        var jsonElement = JsonElement.Parse(
            """
            {
              "Id" : "Foo"
            }
            """);
        var deserialize = jsonElement.Deserialize<IDictionary<string,object?>>();
        return deserialize;
    }
}