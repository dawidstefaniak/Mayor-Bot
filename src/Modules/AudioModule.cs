using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using MayorBot.Services;

namespace MayorBot.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private readonly AudioService _service;
        private readonly RandomGenService _random;


        // Remember to add an instance of the AudioService
        // to your IServiceCollection when you initialize your bot
        public AudioModule(AudioService service, RandomGenService random)
        {
            _service = service;
            _random = random;
        }

        // You *MUST* mark these commands with 'RunMode.Async'
        // otherwise the bot will not respond until the Task times out.
        [Command("join", RunMode = RunMode.Async)]
        public async Task JoinCmd()
        {
            await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState)?.VoiceChannel);
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
        public async Task PlayKonon([Remainder] double minutes=0)
        {
            await LeaveCmd();
            await JoinCmd();
            //TODO Make service from it
            var kononDir = new DirectoryInfo(@"konon");
            await Context.Channel.SendMessageAsync(kononDir.FullName);
            var files = kononDir.GetFiles(@"*.mp3");
            //This to service
            await Context.Channel.SendMessageAsync("Mayor bot activated!");
            if (minutes>=0)
            while (true)
            {
                await _service.SendAudioAsync(Context.Guild, Context.Channel, $@"{files[_random.GetRandomValueFromZero(files.Length)].FullName}");
                Thread.Sleep(_random.GetRangeValue(minutes * 60000));
            }
        }
        
    }
}