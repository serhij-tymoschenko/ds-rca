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
            message += $"**New rca! • New rca! • New rca! • New rca! • New rca!**\n";
        else
            message += $"**New contract! • New contract! • New contract! • New contract! • New contract!**\n";
        
        message += $"**[{rca.Title}]({rca.ShopUrl})**\n";
        rca.Description.Split('\n').ToList().ForEach(x => message += $"> {x}\n");
        
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

        var nextLineOfTrait = new List<String>();
        if (rca.Traits.HairUrl != null) nextLineOfTrait.Add($"[hair]({rca.Traits.HairUrl})");

        if (rca.Traits.HairBackUrl != null) nextLineOfTrait.Add($"[hair_back]({rca.Traits.HairBackUrl}) •");

        if (rca.Traits.HatsUrl != null) nextLineOfTrait.Add($"[hats]({rca.Traits.HatsUrl})");

        if (rca.Traits.LeftUrl != null) nextLineOfTrait.Add($"[left]({rca.Traits.LeftUrl})");

        if (rca.Traits.RightUrl != null) nextLineOfTrait.Add($"[right]({rca.Traits.RightUrl})");

        return message + String.Join(" • ", nextLineOfTrait.ToArray());
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