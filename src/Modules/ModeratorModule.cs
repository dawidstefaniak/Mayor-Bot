using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace MayorBot.Modules
{
    [Name("Moderator")]
    [RequireContext(ContextType.Guild)]
    public class ModeratorModule : ModuleBase<SocketCommandContext>
    {
        [Command("kick")]
        [Summary("Kick the specified user.")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        public async Task Kick([Remainder]SocketGuildUser user)
        {
            await ReplyAsync($"cya {user.Mention} :wave:");
            await user.KickAsync();
        }

        [Command("ban")]
        [Summary("Ban the specified user.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Ban([Remainder] SocketGuildUser user)
        {
            await ReplyAsync($"bye {user.Mention} :wave:. You've got banned :D");
            SocketGuild socket = user.Guild;
            await socket.AddBanAsync(user);
        }

        [Command("unban")]
        [Summary("Unban the specified user.")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        public async Task Unban([Remainder] SocketGuildUser user)
        {
            await ReplyAsync($"You unbanned {user.Mention}.");
            SocketGuild socket = user.Guild;
            await socket.RemoveBanAsync(user);
        }
    }
}
