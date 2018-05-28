using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Audio;

namespace MayorBot.Services
{
    public class AudioService
    {
        private readonly ConcurrentDictionary<ulong, IAudioClient> _connectedChannels = new ConcurrentDictionary<ulong, IAudioClient>();

        public async Task JoinAudio(IGuild guild, IVoiceChannel target)
        {
            if (_connectedChannels.TryGetValue(guild.Id, out _))
            {
                return;
            }
            if (target.Guild.Id != guild.Id)
            {
                return;
            }

            var audioClient = await target.ConnectAsync();

            if (_connectedChannels.TryAdd(guild.Id, audioClient))
            {
                // If you add a method to log happenings from this service,
                // you can uncomment these commented lines to make use of that.
                //await Log(LogSeverity.Info, $"Connected to voice on {guild.Name}.");
            }
        }

        public async Task LeaveAudio(IGuild guild)
        {
            if (_connectedChannels.TryRemove(guild.Id, out var client))
            {
                await client.StopAsync();
                //await Log(LogSeverity.Info, $"Disconnected from voice on {guild.Name}.");
            }
        }

        public async Task SendAudioAsync(IGuild guild, IMessageChannel channel, string path, bool isFolder = false)
        {
            if (!File.Exists(path))
            {
                await channel.SendMessageAsync("File does not exist.");
                return;
            }
            if (_connectedChannels.TryGetValue(guild.Id, out var client))
            {
                if(!isFolder)
                using (var ffmpeg = CreateStream(path))
                using (var stream = client.CreatePCMStream(AudioApplication.Music))
                {
                    try
                    {
                        await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream);
                    }
                    finally { await stream.FlushAsync(); }
                }
                else
                {
                    using (var ffmpeg = CreateStreamForFolder(path))
                    using (var stream = client.CreatePCMStream(AudioApplication.Music))
                    {
                        try
                        {
                            await ffmpeg.StandardOutput.BaseStream.CopyToAsync(stream);
                        }
                        finally { await stream.FlushAsync(); }
                    }
                }
            }
        }

        public async Task PlayKononAsync(IGuild guild, IMessageChannel channel)
        {
            await SendAudioAsync(guild, channel, "konon", true);
        }
        private Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }
        private Process CreateStreamForFolder(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg.exe",
                Arguments = $" find {path} -name \"*.mp3\" | xargs -I $ -hide_banner -loglevel panic -i $ -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true
            });
        }

    }
}