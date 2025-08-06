using PuzzlescapeGames.Services.AudioService;
using System;
using UnityEngine;
using AudioSource = PuzzlescapeGames.Services.AudioService.AudioSource;

namespace Game.Managers.AudioManager
{
    public sealed class AudioManager
    {
        public bool IsMusic => true;
        // {
        //     get => _preferences.Value.Music;
        //     set
        //     {
        //         var data = _preferences.Value;
        //         data.Music = value;
        //         _preferences.Value = data;
        //
        //         MuteMusic( !value );
        //     }
        // }
        public bool IsSound => true;
        // {
        //     get => _preferences.Value.Sound;
        //     set
        //     {
        //         var data = _preferences.Value;
        //         data.Sound = value;
        //         _preferences.Value = data;
        //     }
        // }

        private readonly IAudioService _audioService;
        
        public AudioManager( IAudioService audioService )
        {
            _audioService = audioService ?? throw new ArgumentNullException( nameof(audioService) );
        }
        
        public AudioSource PlaySound( AudioClip clip, bool isLoop = false, float volume = 1f, float pitch = 1f  )
        {
            if ( !IsSound ) return null;
            
            var source =  _audioService.PlaySound( clip, isLoop );
            source.Source.volume = volume;
            source.Source.pitch = pitch;
            return source;
        }
    }
}