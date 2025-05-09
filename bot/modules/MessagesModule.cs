using ds_rca.data.entities;
using NetCord.Gateway;
using NetCord.Rest;

namespace ds_rca.bot.modules;

public class MessagesModule(GatewayClient client)
{
    private string BuildMessage(Rca rca, MessageType type)
    {
        var message = "";

        if (type is MessageType.RCA)
            message += "**Found new rca:**\n";
        else
            message += "**Found new contract:**\n";

        message += "**Author:** " +
                   $"{rca.AuthorName} \n" +
                   "**Name:** " +
                   $"{rca.Title}\n" +
                   "**Description:** " +
                   $"{rca.Description}\n" +
                   "**Count:** " +
                   $"{rca.Count}\n" +
                   "**Price:** " +
                   $"{rca.Price}\n" +
                   "**Shop:** " +
                   $"{rca.ShopUrl}\n" +
                   "**Traits:** " +
                   $"[face]({rca.Traits.FaceUrl}) " +
                   $"[eyes]({rca.Traits.EyesUrl}) " +
                   $"[tops]({rca.Traits.TopsUrl}) " +
                   $"[bottoms]({rca.Traits.BottomsUrl}) " +
                   $"[background]({rca.Traits.BackgroundUrl}) ";

        if (rca.Traits.HairUrl != null) message += $"[hair]({rca.Traits.HairUrl}) ";

        if (rca.Traits.HairBackUrl != null) message += $"[hair_back]({rca.Traits.HairBackUrl}) ";

        if (rca.Traits.HatsUrl != null) message += $"[hats]({rca.Traits.HatsUrl}) ";

        if (rca.Traits.LeftUrl != null) message += $"[left]({rca.Traits.LeftUrl}) ";

        if (rca.Traits.RightUrl != null) message += $"[right]({rca.Traits.RightUrl})";

        return message;
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