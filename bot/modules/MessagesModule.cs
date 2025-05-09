using ds_rca.data.entities;
using ds_rca.utils;
using NetCord.Gateway;
using NetCord.Rest;

namespace ds_rca.bot.modules;

public class MessagesModule(GatewayClient client)
{
    private string BuildMessage(Rca rca, MessageType type)
    {
        var message = "";

        if (type is MessageType.RCA)
            message += $"**New rca!**\n";
        else
            message += $"**New contract!**\n";

        message += "\n";
        
        message += $"**[{rca.Title}]({rca.ShopUrl})**\n";
        rca.Description.Split('\n').ToList().ForEach(x => message += $"> {x}\n");
        
        message += "\n";
        
        message += "**Additional info:**\n";
        message += $"Price: {rca.Price.FormatToPrice()} •" +
                   $" Amount: {rca.Count} •" +
                   $" Author: [{rca.AuthorName}]({rca.AuthorShopUrl})\n";
        
        message += "\n";
        
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

        if (nextLineOfTrait.Count > 0)
        {
            message += String.Join(" • ", nextLineOfTrait.ToArray());
        }
        
        message += "\n";
        
        message += "\n";
        
        message += "**Overall look:**";
        
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