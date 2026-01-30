using System.Text.Json;
using DefaultNamespace;

namespace DistributedSystems.Tests;

public static class CarExtensions
{
    private static PacketKey<string> Id = new(nameof(Id));
    public static PacketKey<Packet> Package = new(nameof(Package));
        
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

        var value = PacketTests.Any.AString();
        packet.ID = value;
        Assert.Equal(value, packet.ID);
    }

    [Fact]
    public void GetPackageFromJson()
    {
        var packet = Packet.From(JsonData());
        
        var packet1 = packet.Get(CarExtensions.Package);
        
        Assert.NotNull(packet1);
        Assert.Equal("Bar", packet1.ID);
    }

    private static IDictionary<string, object?>? JsonData()
    {
        var jsonElement = JsonElement.Parse(
            """
            {
              "Id" : "Foo",
              "Package" : {
                "Id" : "Bar"
              }
            }
            """);
        return jsonElement.Deserialize<IDictionary<string,object?>>();
    }
}