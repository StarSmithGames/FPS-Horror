using Cysharp.Threading.Tasks;
using Game.Core.UI.SaveOptionsDialog;
using Game.Managers.InputManager;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.VVM;
using StarSmithGames.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialogViewModel : ViewModel< OptionsDialog >
    {
        private InputActionWrap _inputActionCancel;
        private int _currentTabIndex;
        private CancellationTokenSource _cancellationTokenSource;
        private List< OptionController > _options = new();

        private bool _isAudioInitialized;
        private OptionPercentsController _masterVolumeOption;
        private OptionPercentsController _dialogueVolumeOption;
        private OptionPercentsController _musicVolumeOption;
        private OptionPercentsController _sfxVolumeOption;
        private OptionPercentsController _ambientVolumeOption;
        
        private bool _isGraphicsInitialized;
        private OptionSelectorController _resolutionOption;
        private OptionToggleController _fullScreenOption;
        private OptionToggleController _vsyncOption;
        
        private readonly UIRootGame _uiRootGame;
        private readonly DataHolder _dataHolder;
        private readonly ILocalizationSystem _localizationSystem;
        
        public OptionsDialogViewModel(
            UIRootGame uiRootGame,
            DataHolder dataHolder,
            ILocalizationSystem localizationSystem
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _dataHolder = dataHolder ?? throw new ArgumentNullException( nameof(dataHolder) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();

            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked += OnTabButtonClickedHandler;
            }
            
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked -= OnTabButtonClickedHandler;
            }
            
            _inputActionCancel.Disable();
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !IsShowing ) return;

            for ( int i = 0; i < ModelView.Contents.Count; i++ )
            {
                ModelView.Contents[ i ].DestroyChildren();
            }
            _options.Clear();

            _cancellationTokenSource = new();
            LoadOptions( 0, _cancellationTokenSource.Token ).Forget();
        }

        private async UniTask LoadOptions( int index, CancellationToken cancellationToken = default )
        {
            _currentTabIndex = index;
            
            for ( int i = 0; i < ModelView.Contents.Count; i++ )
            {
                ModelView.Contents[ i ].gameObject.SetActive( false );
            }
            ModelView.Contents[ index ].gameObject.SetActive( true );
            
            if ( index == 1 )
            {
                if ( !_isAudioInitialized )
                {
                    _isAudioInitialized = true;
                    await LoadAudio( ModelView.Contents[ 1 ], cancellationToken );
                }
            }
            else if ( index == 2 )
            {
                if ( !_isGraphicsInitialized )
                {
                    _isGraphicsInitialized = true;
                    await LoadGraphics( ModelView.Contents[ 2 ], cancellationToken );
                }
            }

            async UniTask LoadAudio( Transform content, CancellationToken cancellationToken = default )
            {
                var data = _dataHolder.GeneralStorageData.Audio.Value;
                
                _masterVolumeOption = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), data.MasterVolume );
                _masterVolumeOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_MASTER_VOLUME ) );
                _masterVolumeOption.Initialize();
                _options.Add( _masterVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                _dialogueVolumeOption = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), data.DialogueVolume );
                _dialogueVolumeOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_DIALOGUE_VOLUME ) );
                _dialogueVolumeOption.Initialize();
                _options.Add( _dialogueVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                _musicVolumeOption = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), data.MusicVolume );
                _musicVolumeOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_MUSIC_VOLUME ) );
                _musicVolumeOption.Initialize();
                _options.Add( _musicVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                _sfxVolumeOption = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), data.SFXVolume );
                _sfxVolumeOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_SFX_VOLUME ) );
                _sfxVolumeOption.Initialize();
                _options.Add( _sfxVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                _ambientVolumeOption = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), data.AmbientVolume );
                _ambientVolumeOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_AMBIENT_VOLUME ) );
                _ambientVolumeOption.Initialize();
                _options.Add( _ambientVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }

            async UniTask LoadGraphics( Transform content, CancellationToken cancellationToken = default )
            {
                var data = _dataHolder.GeneralStorageData.Graphics.Value;
                
                //RESOLUTION
                var resolutions = Screen.resolutions.ToList();
                _resolutionOption = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), resolutions.IndexOf( Screen.currentResolution ) );
                _resolutionOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_RESOLUTION ) );
                _resolutionOption.Initialize( resolutions.Select( ( x ) => $"{x.width}x{x.height}" ).ToArray() );
                _options.Add( _resolutionOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                //FULL SCREEN
                _fullScreenOption = new( GameObject.Instantiate( ModelView.OptionTogglePrefab, content ), data.IsFullScreen );
                _fullScreenOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_FULL_SCREEN ) );
                _fullScreenOption.Initialize();
                _options.Add( _fullScreenOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                //VSYNC
                _vsyncOption = new( GameObject.Instantiate( ModelView.OptionTogglePrefab, content ), data.IsVsync );
                _vsyncOption.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_VSYNC ) );
                _vsyncOption.Initialize();
                _options.Add( _vsyncOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        private void Save()
        {
            for ( int i = 0; i < _options.Count; i++ )
            {
                _options[ i ].ResetDirty();
            }
            
            var audio = _dataHolder.GeneralStorageData.Audio.Value;
            audio.MasterVolume = _masterVolumeOption.Value;
            audio.DialogueVolume = _dialogueVolumeOption.Value;
            audio.MusicVolume = _musicVolumeOption.Value;
            audio.SFXVolume = _sfxVolumeOption.Value;
            audio.AmbientVolume = _ambientVolumeOption.Value;

            var graphics = _dataHolder.GeneralStorageData.Graphics.Value;
            var resolution = Screen.resolutions[ _resolutionOption.Index ];
            graphics.ResolutionWidth = resolution.width;
            graphics.ResolutionHeight = resolution.height;
            graphics.IsFullScreen = _fullScreenOption.IsOn;
            graphics.IsVsync = _vsyncOption.IsOn;
            
            _dataHolder.SaveGeneral();
            
            Screen.SetResolution( graphics.ResolutionWidth, graphics.ResolutionHeight, graphics.IsFullScreen );
            QualitySettings.vSyncCount = graphics.IsVsync ? 1 : 0;
            Application.targetFrameRate = 60;
        }

        private void OnTabButtonClickedHandler( UITab tab )
        {
            int index = ModelView.Tabs.IndexOf( tab );
            if ( _currentTabIndex != index )
            {
                _cancellationTokenSource?.Cancel();
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = new();
                LoadOptions( index ).Forget();
            }
        }
        
        private void CancelButtonClickedHandler()
        {
            if ( _options.Any( ( x ) => x.IsDirty ) )
            {
                _inputActionCancel.Disable();
            
                var dialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< SaveOptionsDialogViewModel >();
                dialog.OnAcceptRejectShowingChanged += AcceptRejectShowingChanged;
                dialog.ShowView();
            }
            else
            {
                HideViewAndDispose();
            }
        }

        private void AcceptRejectShowingChanged( IViewModel dialog, bool result )
        {
            var acceptReject = (SaveOptionsDialogViewModel)dialog;
            acceptReject.OnAcceptRejectShowingChanged -= AcceptRejectShowingChanged;
            
            _inputActionCancel.Enable();
            
            if ( result )
            {
                Save();
            }
            HideViewAndDispose();
        }
    }
}