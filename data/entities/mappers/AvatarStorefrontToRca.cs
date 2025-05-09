using System.Text.RegularExpressions;
using ds_rca.data.remote.dto;
using ds_rca.utils.constants;

namespace ds_rca.data.entities.mappers;

public static class AvatarStorefrontToRca
{
    public static Rca ConvertToRca(this AvatarStorefrontDto dto)
    {
        var node = dto.ResData.AvatarStorefront.Listings.Edges[0].Node;
        var title = node.Item.Nft.Title;
        var description = node.Item.Nft.Description;
        var authorName = node.Artist.RedditorInfo.DisplayName;
        var authorShopUrl = $"https://www.reddit.com/avatar/shop/artist/{node.Artist.RedditorInfo.PrefixedName.Substring(2)}";
        var count = node.Item.Drop.Size;
        var imageUrl = node.Item.Benefits.AvatarOutfit.PreRenderImage.Url;
        var price = node.ProductOffer.PricePackages[0].Price;
        var shopUrl = $"https://www.reddit.com/avatar/shop/product/{node.Id}";
        var traits = GetRcaTraits(node.Item.Benefits.AvatarOutfit.AccessoryIds,
            node.Item.Benefits.AvatarOutfit.BackgroundImage.Url);

        var rca = new Rca
        {
            Title = title,
            Description = description,
            AuthorName = authorName,
            AuthorShopUrl = authorShopUrl,
            Count = count,
            ImageUrl = imageUrl,
            Price = price,
            ShopUrl = shopUrl,
            Traits = traits
        };

        return rca;
    }

    private static RcaTraits GetRcaTraits(List<string> accessoryIds, string backgroundUrl)
    {
        var eyes = "";
        var face = "";
        var tops = "";
        var bottoms = "";
        string? hair = null;
        string? hairBack = null;
        string? hats = null;
        string? left = null;
        string? right = null;

        accessoryIds.ForEach(id =>
        {
            if (Regex.IsMatch(id, AccessoriesRegex.Right))
                right = Regex.Match(id, AccessoriesRegex.Right).Groups[1].Value;

            if (Regex.IsMatch(id, AccessoriesRegex.Left)) left = Regex.Match(id, AccessoriesRegex.Left).Groups[1].Value;

            if (Regex.IsMatch(id, AccessoriesRegex.Bottoms))
                bottoms = Regex.Match(id, AccessoriesRegex.Bottoms).Groups[1].Value;

            if (Regex.IsMatch(id, AccessoriesRegex.Tops)) tops = Regex.Match(id, AccessoriesRegex.Tops).Groups[1].Value;

            if (Regex.IsMatch(id, AccessoriesRegex.Face)) face = Regex.Match(id, AccessoriesRegex.Face).Groups[1].Value;

            if (Regex.IsMatch(id, AccessoriesRegex.Eyes)) eyes = Regex.Match(id, AccessoriesRegex.Eyes).Groups[1].Value;

            if (Regex.IsMatch(id, AccessoriesRegex.Hair))
            {
                var matches = Regex.Match(id, AccessoriesRegex.Hair);
                hair = matches.Groups[1].Value;

                if (matches.Groups[2].Value != "")
                {
                    hairBack = matches.Groups[2].Value;
                }
            }

            if (Regex.IsMatch(id, AccessoriesRegex.Hats)) hats = Regex.Match(id, AccessoriesRegex.Hats).Groups[1].Value;
        });

        var traits = new RcaTraits
        {
            BackgroundUrl = backgroundUrl,
            BottomsUrl = GetAccessoryUrl(bottoms),
            TopsUrl = GetAccessoryUrl(tops),
            RightUrl = GetAccessoryUrl(right),
            LeftUrl = GetAccessoryUrl(left),
            EyesUrl = GetAccessoryUrl(eyes),
            FaceUrl = GetAccessoryUrl(face),
            HairUrl = GetAccessoryUrl(hair),
            HairBackUrl = GetAccessoryUrl(hairBack),
            HatsUrl = GetAccessoryUrl(hats)
        };
        return traits;
    }

    private static string GetAccessoryUrl(string? accessoryId)
    {
        if (accessoryId == null) return null;

        var url = $"https://i.redd.it/snoovatar/snoo_assets/submissions/{accessoryId}_.svg";
        return url;
    }
}