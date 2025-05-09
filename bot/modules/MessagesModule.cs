using ds_rca.data.entities;
using ds_rca.utils;
using NetCord.Gateway;
using NetCord.Rest;

namespace ds_rca.bot.modules;

public class MessagesModule(GatewayClient client)
{
    private string BuildTitleSection(MessageType type)
    {
        var title = "";
        if (type is MessageType.RCA)
            title += "**New rca!**\n";
        else
            title += "**New contract!**\n";
        return title;
    }

    private string BuildNftMessageSection(Rca rca)
    {
        var nftInfo = "**NFT info:**\n";
        nftInfo += $"Price: {rca.Price.FormatToPrice()} • " 
                   + $"Amount: {rca.Count} • "
                   + $"Author: [{rca.AuthorName}]({rca.AuthorShopUrl})\n";
        return nftInfo;
    }

    private string BuildAvatarMessageSection(Rca rca)
    {
        var avatarMessage = $"**[{rca.Title}]({rca.ShopUrl})**\n";
        rca.Description
            .Split('\n')
            .ToList()
            .ForEach(line => avatarMessage += $"> {line}\n");
        return avatarMessage;
    }

    private string BuildTraitsMessageSection(Rca rca)
    {
        var traitsMessage = "**Avatar traits:**\n";
        traitsMessage += $"[face]({rca.Traits.FaceUrl}) • "
                         + $"[eyes]({rca.Traits.EyesUrl}) • "
                         + $"[tops]({rca.Traits.TopsUrl}) • "
                         + $"[bottoms]({rca.Traits.BottomsUrl}) • "
                         + $"[background]({rca.Traits.BackgroundUrl})"
                         + "\n";

        var nextLineTraits = new List<string>();
        if (rca.Traits.HairUrl != null) nextLineTraits.Add($"[hair]({rca.Traits.HairUrl})");

        if (rca.Traits.HairBackUrl != null) nextLineTraits.Add($"[hair back]({rca.Traits.HairBackUrl})");

        if (rca.Traits.HatsUrl != null) nextLineTraits.Add($"[hats]({rca.Traits.HatsUrl})");

        if (rca.Traits.LeftUrl != null) nextLineTraits.Add($"[left]({rca.Traits.LeftUrl})");

        if (rca.Traits.RightUrl != null) nextLineTraits.Add($"[right]({rca.Traits.RightUrl})");

        if (nextLineTraits.Count > 0)
            traitsMessage += string.Join(" • ", nextLineTraits.ToArray()) + "\n";

        return traitsMessage;
    }

    private string BuildMessage(Rca rca, MessageType type)
    {
        var message = BuildTitleSection(type)
                      + "\n"
                      + BuildNftMessageSection(rca)
                      + "\n"
                      + BuildAvatarMessageSection(rca)
                      + "\n"
                      + BuildTraitsMessageSection(rca)
                      + "\n"
                      + "**Overall look:**";
        return message;
    }

    public async Task SendRcaDetailsAsync(ulong channelId, Rca rca, MessageType type)
    {
        try
        {
            var content = BuildMessage(rca, type);
            var embed = new EmbedProperties
            {
                Image = new EmbedImageProperties(rca.ImageUrl)
            };
            var message = new MessageProperties
            {
                Content = content,
                Embeds = new[] { embed }
            };

            await client.Rest.SendMessageAsync(channelId, message);
        }
        catch (Exception e)
        {
            Bot.Log($"Error sending RCA details: {e.Message}");
        }
    }
}