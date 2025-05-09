using ds_rca.data.entities;
using ds_rca.utils;
using NetCord.Gateway;
using NetCord.Rest;

namespace ds_rca.bot.modules;

public class MessagesModule(GatewayClient client)
{
    private string BuildMessage(Rca rca, MessageType type)
    {
        var message = "\n";

        if (type is MessageType.RCA)
            message += $"**••••••••••New rca:**\n";
        else
            message += $"**••••••••••New contract:**\n";
        
        message += $"**[{rca.Title}]({rca.ShopUrl})**\n";
        rca.Description.Split('\n').ToList().ForEach(x => message += $"> {x}\n");
        message += "\n";
        
        message += "**Additional info:**\n";
        message += $"Price: {rca.Price.FormatToPrice()} •" +
                   $" Amount: {rca.Count} •" +
                   $" Author: [{rca.AuthorName}]({rca.AuthorShopUrl})\n";
        
        
        message += "**Avatar traits:**\n";
        message += $"[face]({rca.Traits.FaceUrl}) •" +
                   $" [eyes]({rca.Traits.EyesUrl}) •" +
                   $" [tops]({rca.Traits.TopsUrl}) •" +
                   $" [bottoms]({rca.Traits.BottomsUrl}) •" +
                   $" [background]({rca.Traits.BackgroundUrl})";

        message += "\n";

        var nextLineOfTrait = "";
        if (rca.Traits.HairUrl != null) nextLineOfTrait += $"[hair]({rca.Traits.HairUrl}) •";

        if (rca.Traits.HairBackUrl != null) nextLineOfTrait += $" [hair_back]({rca.Traits.HairBackUrl}) •";

        if (rca.Traits.HatsUrl != null) nextLineOfTrait += $" [hats]({rca.Traits.HatsUrl}) •";

        if (rca.Traits.LeftUrl != null) nextLineOfTrait += $" [left]({rca.Traits.LeftUrl}) •";

        if (rca.Traits.RightUrl != null) nextLineOfTrait += $" [right]({rca.Traits.RightUrl})";

        return message + nextLineOfTrait.Trim();
    }

    public async Task SendRcaDetailsAsync(ulong channelId, Rca rca, MessageType type)
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

        client.Rest.SendMessageAsync(channelId, message);
    }
}