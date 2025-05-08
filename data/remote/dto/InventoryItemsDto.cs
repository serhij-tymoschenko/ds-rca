using Newtonsoft.Json;

namespace ds_rca.data.remote.dto;

public class InventoryItemsDto
{
    [JsonProperty("data")] public required Data ResData { get; set; }

    public class Data
    {
        [JsonProperty("inventoryItems")] public required InventoryItems InventoryItems { get; set; }
    }

    public class InventoryItems
    {
        [JsonProperty("edges")] public required List<Edge> Edges { get; set; }
    }

    public class Edge
    {
        [JsonProperty("node")] public required Node Node { get; set; }
    }

    public class Node
    {
        [JsonProperty("benefits")] public required Benefits Benefits { get; set; }
    }

    public class Benefits
    {
        [JsonProperty("avatarOutfit")] public required AvatarOutfit AvatarOutfit { get; set; }
    }

    public class AvatarOutfit
    {
        [JsonProperty("id")] public required string Id { get; set; }
    }
}