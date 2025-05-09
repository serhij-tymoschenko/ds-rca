using System.Text.RegularExpressions;
using ds_rca.data.db;
using ds_rca.utils.constants;
using NetCord;
using NetCord.Services.ApplicationCommands;

namespace ds_rca.bot.modules;

public class SlashCommandsModule : ApplicationCommandModule<ApplicationCommandContext>
{
    [SlashCommand("config", "Configures channels for messages")]
    public string Config(
        [SlashCommandParameter(Name = "rca_channel_id")]
        string rcaChannelId,
        [SlashCommandParameter(Name = "contracts_channel_id")]
        string contractsChannelId
    )
    {
        var user = Context.User as GuildUser;
        var permissions = user.GetPermissions(Context.Guild);
        
        if (permissions.HasFlag(Permissions.Administrator))
        {
            Database.SetServerConfigAsync(Context.Guild.Id, rcaChannelId, contractsChannelId);
            return "Configured";
        }
        
        return "You're not admin";
    }
}