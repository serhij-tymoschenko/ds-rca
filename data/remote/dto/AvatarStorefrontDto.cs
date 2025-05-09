using Newtonsoft.Json;

namespace ds_rca.data.remote.dto;

public class AvatarStorefrontDto
{
    [JsonProperty("data")] public required Data ResData { get; set; }

    public class Data
    {
        [JsonProperty("avatarStorefront")] public AvatarStorefront AvatarStorefront { get; set; }
    }

    public class AvatarStorefront
    {
        [JsonProperty("listings")] public Listings Listings { get; set; }
    }

    public class Listings
    {
        [JsonProperty("edges")] public List<Edge> Edges { get; set; }
    }

    public class Edge
    {
        [JsonProperty("node")] public Node Node { get; set; }
    }

    public class Node
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("totalQuantity")] public int TotalQuantity { get; set; }

        [JsonProperty("productOffer")] public ProductOffer ProductOffer { get; set; }

        [JsonProperty("artist")] public StorefrontArtist Artist { get; set; }

        [JsonProperty("item")] public StorefrontItem Item { get; set; }
    }

    public class ProductOffer
    {
        [JsonProperty("pricePackages")] public List<PricePackage> PricePackages { get; set; }
    }

    public class PricePackage
    {
        [JsonProperty("price")] public string Price { get; set; }
    }

    public class StorefrontArtist
    {
        [JsonProperty("redditorInfo")] public RedditorInfo RedditorInfo { get; set; }
    }

    public class RedditorInfo
    {
        [JsonProperty("displayName")] public string DisplayName { get; set; }
        [JsonProperty("prefixedName")] public string PrefixedName { get; set; }
    }

    public class StorefrontItem
    {
        [JsonProperty("images")] public List<Image> Images { get; set; }

        [JsonProperty("drop")] public Drop Drop { get; set; }

        [JsonProperty("benefits")] public Benefits Benefits { get; set; }

        [JsonProperty("nft")] public Nft Nft { get; set; }
    }

    public class Image
    {
        [JsonProperty("url")] public string Url { get; set; }
    }

    public class Drop
    {
        [JsonProperty("size")] public int Size { get; set; }
    }

    public class Benefits
    {
        [JsonProperty("avatarOutfit")] public AvatarOutfit AvatarOutfit { get; set; }
    }

    public class AvatarOutfit
    {
        [JsonProperty("accessoryIds")] public List<string> AccessoryIds { get; set; }

        [JsonProperty("backgroundImage")] public BackgroundImage BackgroundImage { get; set; }

        [JsonProperty("preRenderImage")] public PreRenderImage PreRenderImage { get; set; }
    }

    public class BackgroundImage
    {
        [JsonProperty("url")] public string Url { get; set; }
    }

    public class PreRenderImage
    {
        [JsonProperty("url")] public string Url { get; set; }
    }

    public class Nft
    {
        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("description")] public string Description { get; set; }
    }
}