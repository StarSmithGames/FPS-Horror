using Cysharp.Threading.Tasks;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Localization;
using StarSmithGames.Localization;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class AudioTabController : TabController
    {
        private OptionBarController _masterVolumeOption;
        private OptionBarController _dialogueVolumeOption;
        private OptionBarController _musicVolumeOption;
        private OptionBarController _sfxVolumeOption;
        private OptionBarController _ambientVolumeOption;
        
        private readonly AudioData _data;
        
        public AudioTabController(
            TabsSettings settings,
            DataHolder dataHolder,
            ILocalizationSystem localizationSystem
        ) : base( settings, localizationSystem )
        {
            _data = dataHolder.GeneralStorageData.Audio.Value;
        }

        public override void Save()
        {
            base.Save();
            
            if ( _isInitialized )
            {
                _data.MasterVolume = (int)_masterVolumeOption.Value;
                _data.DialogueVolume = (int)_dialogueVolumeOption.Value;
                _data.MusicVolume = (int)_musicVolumeOption.Value;
                _data.SFXVolume = (int)_sfxVolumeOption.Value;
                _data.AmbientVolume = (int)_ambientVolumeOption.Value;
            }
        }

        protected override async UniTask Load( Transform content, CancellationToken cancellationToken = default )
        {
            _masterVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_MASTER_VOLUME, _data.MasterVolume );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
                
            _dialogueVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_DIALOGUE_VOLUME, _data.DialogueVolume );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
                
            _musicVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_MUSIC_VOLUME, _data.MusicVolume );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
                
            _sfxVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_SFX_VOLUME, _data.SFXVolume );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
                
            _ambientVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_AMBIENT_VOLUME, _data.AmbientVolume );
            // await UniTask.Yield();
            // cancellationToken.ThrowIfCancellationRequested();
        }
    }
}