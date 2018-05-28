using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MayorBot.Services;

namespace MayorBot.Modules
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private readonly AudioService _service;
        private readonly DirectoryInfo _kononDir = new DirectoryInfo("konon");
        private readonly Random _random = new Random();

        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public AudioModule(AudioService service)
        {
            _service = service;
        }

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        }

        // Remember to add preconditions to your commands,
        // this is merely the minimal amount necessary.
        // Adding more commands of your own is also encouraged.
        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
  
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }

        [Command("konon", RunMode = RunMode.Async)]
        public async Task PlayKonon()
        {
            foreach (var file in _kononDir.GetFiles("*.mp3"))
            {
                await _service.SendAudioAsync(Context.Guild, Context.Channel, $"konon/{file.Name}");
            }
        }
    }
}