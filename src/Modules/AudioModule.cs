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
    public class AudioModule : ModuleBase<ICommandContext>
    {
        // Scroll down further for the AudioService.
        // Like, way down
        private readonly AudioService _service;
        private readonly RandomGenService _random;

        //Konon directory is folder with all mp3's 
        private readonly DirectoryInfo _kononDir = new DirectoryInfo("konon");


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
        public async Task PlayKonon([Remainder] double minutes)
        {
            var files = _kononDir.GetFiles("*.mp3");
            
            if (minutes>=0)
            while (true)
            {
                await _service.SendAudioAsync(Context.Guild, Context.Channel, $"konon/{files[_random.GetRandomValueFromZero(files.Length)]}");
                Thread.Sleep(_random.GetRangeValue((minutes * 60000)));
            }
        }
        
    }
}