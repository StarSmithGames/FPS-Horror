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
using UnityEngine.EventSystems;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialogViewModel : ViewModel< OptionsDialog >
    {
        private int _currentTabIndex = -1;
        private CancellationTokenSource _cancellationTokenSource;
        private List< OptionController > _options = new();

        private bool _isAudioInitialized;
        private OptionBarController _masterVolumeOption;
        private OptionBarController _dialogueVolumeOption;
        private OptionBarController _musicVolumeOption;
        private OptionBarController _sfxVolumeOption;
        private OptionBarController _ambientVolumeOption;
        private List< OptionController > _audioOptions = new();
        
        private bool _isGraphicsInitialized;
        private OptionSelectorController _resolutionOption;
        private OptionToggleController _fullScreenOption;
        private OptionToggleController _vsyncOption;
        private List< OptionController > _graphicsOptions = new();
        
        private bool _isControlsInitialized;
        private OptionToggleController _sprintOption;
        private OptionToggleController _crouchOption;
        private OptionBarController _mouseSensitivityOption;
        private OptionBarController _controllerXSensitivityOption;
        private OptionBarController _controllerYSensitivityOption;
        private List< OptionController > _controlsOptions = new();

        private InputActionVoidWrap _inputActionCancel;
        private InputActionVoidWrap _inputActionLB;
        private InputActionVoidWrap _inputActionRB;
        private InputActionValueWrap< Vector2 > _inputActionNavigate;

        private OptionController _lastOption;
        
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
            _inputActionLB = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.LB, LBButtonClickedHandler );
            _inputActionRB = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.RB, RBButtonClickedHandler );
            _inputActionNavigate = InputActionManager.CreateInputActionWrap< Vector2 >( InputManager.Inputs.UI.Navigate, onPerformed: NavigateChangedHandler );
            _inputActionCancel.Enable();
            _inputActionLB.Enable();
            _inputActionRB.Enable();
            _inputActionNavigate.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked -= OnTabButtonClickedHandler;
            }
            
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
            InputActionManager.RemoveInputActionWrap( _inputActionLB );
            InputActionManager.RemoveInputActionWrap( _inputActionRB );
            InputActionManager.RemoveInputActionWrap( _inputActionNavigate );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !IsShowing ) return;
            
            for ( int i = 0; i < ModelView.Contents.Count; i++ )
            {
                ModelView.Contents[ i ].DestroyChildren();
            }
            _options.Clear();
            
            TryLoadTab( 0 );
        }

        private void TryLoadTab( int index )
        {
            if ( _currentTabIndex != index )
            {
                _currentTabIndex = index;

                for ( int i = 0; i < ModelView.Tabs.Count; i++ )
                {
                    if ( _currentTabIndex == i )
                    {
                        ModelView.Tabs[ i ].Select();
                    }
                    else
                    {
                        ModelView.Tabs[ i ].Deselect();
                    }
                }
                
                if ( _cancellationTokenSource == null )
                {
                    _cancellationTokenSource = new();
                }
                LoadOptions( index, _cancellationTokenSource.Token ).Forget();
            }
        }
        
        private async UniTask LoadOptions( int index, CancellationToken cancellationToken = default )
        {
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
                    LoadAudio( ModelView.Contents[ index ], cancellationToken ).Forget();
                    await UniTask.Yield();
                }
                EventSystem.current.SetSelectedGameObject( _audioOptions.First().View.gameObject );
            }
            else if ( index == 2 )
            {
                if ( !_isGraphicsInitialized )
                {
                    _isGraphicsInitialized = true;
                    LoadGraphics( ModelView.Contents[ index ], cancellationToken ).Forget();
                    await UniTask.Yield();
                }
                EventSystem.current.SetSelectedGameObject( _graphicsOptions.First().View.gameObject );
            }
            else if ( index == 3 )
            {
                if ( !_isControlsInitialized )
                {
                    _isControlsInitialized = true;
                    LoadControls( ModelView.Contents[ index ], cancellationToken ).Forget();
                    await UniTask.Yield();
                }
                EventSystem.current.SetSelectedGameObject( _controlsOptions.First().View.gameObject );
            }
            
            async UniTask LoadAudio( Transform content, CancellationToken cancellationToken = default )
            {
                var data = _dataHolder.GeneralStorageData.Audio.Value;

                _masterVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_MASTER_VOLUME, data.MasterVolume );
                _audioOptions.Add( _masterVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _dialogueVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_DIALOGUE_VOLUME, data.DialogueVolume );
                _audioOptions.Add( _dialogueVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _musicVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_MUSIC_VOLUME, data.MusicVolume );
                _audioOptions.Add( _musicVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _sfxVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_SFX_VOLUME, data.SFXVolume );
                _audioOptions.Add( _sfxVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _ambientVolumeOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_AMBIENT_VOLUME, data.AmbientVolume );
                _audioOptions.Add( _ambientVolumeOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }

            async UniTask LoadGraphics( Transform content, CancellationToken cancellationToken = default )
            {
                var data = _dataHolder.GeneralStorageData.Graphics.Value;

                var resolutions = Screen.resolutions.ToList();
                _resolutionOption = CreateSelector( content, LocalizationIds.UI_OPTIONS_DIALOG_RESOLUTION, resolutions.IndexOf( Screen.currentResolution ), resolutions.Select( ( x ) => $"{x.width}x{x.height}" ).ToArray() );
                _graphicsOptions.Add( _resolutionOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _fullScreenOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_FULL_SCREEN, data.IsFullScreen );
                _graphicsOptions.Add( _fullScreenOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();

                _vsyncOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_VSYNC, data.IsVsync );
                _graphicsOptions.Add( _vsyncOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
            
            async UniTask LoadControls( Transform content, CancellationToken cancellationToken = default )
            {
                var data = _dataHolder.GeneralStorageData.Controls.Value;

                _sprintOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_TOGGLE_SPRINT, data.IsSprintToggle );
                _controlsOptions.Add( _sprintOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _crouchOption = CreateToggle( content, LocalizationIds.UI_OPTIONS_DIALOG_TOGGLE_CROUCH, data.IsCrouchToggle );
                _controlsOptions.Add( _crouchOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _mouseSensitivityOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_MOUSE_SENSITIVITY, data.MouseXSensitivity, 2, 10, postfix: "" );
                _controlsOptions.Add( _mouseSensitivityOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _controllerXSensitivityOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_CONTROLLER_X_SENSITIVITY, data.ControllerXSensitivity, 35, 150, postfix: "" );
                _controlsOptions.Add( _controllerXSensitivityOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
                
                _controllerYSensitivityOption = CreateBar( content, LocalizationIds.UI_OPTIONS_DIALOG_CONTROLLER_Y_SENSITIVITY, data.ControllerYSensitivity, 35, 150, postfix: "" );
                _controlsOptions.Add( _controllerYSensitivityOption );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }

            OptionSelectorController CreateSelector( Transform content, string nameId, int value, params string[] options )
            {
                OptionSelectorController option = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), value );
                option.SetName( _localizationSystem.Translate( nameId ) );
                option.Initialize( options );
                option.View.OnButtonPointerEntered += ButtonPointerEnteredHandler;
                option.View.OnButtonPointerExited += ButtonPointerExitedHandler;
                
                _options.Add( option );

                return option;
            }
            
            OptionBarController CreateBar( Transform content, string nameId, float value, float min = 0, float max = 100, float step = 1, string postfix = "%" )
            {
                OptionBarController option = new( GameObject.Instantiate( ModelView.OptionLeftRightPrefab, content ), value, min, max, step, postfix );
                option.SetName( _localizationSystem.Translate( nameId ) );
                option.Initialize();
                option.View.OnButtonPointerEntered += ButtonPointerEnteredHandler;
                option.View.OnButtonPointerExited += ButtonPointerExitedHandler;
                
                _options.Add( option );

                return option;
            }

            OptionToggleController CreateToggle( Transform content, string nameId, bool value )
            {
                OptionToggleController option = new( GameObject.Instantiate( ModelView.OptionTogglePrefab, content ), value );
                option.SetName( _localizationSystem.Translate( nameId ) );
                option.Initialize();
                option.View.OnButtonPointerEntered += ButtonPointerEnteredHandler;
                option.View.OnButtonPointerExited += ButtonPointerExitedHandler;

                _options.Add( option );

                return option;
            }
        }

        private void Save()
        {
            for ( int i = 0; i < _options.Count; i++ )
            {
                _options[ i ].ResetDirty();
            }

            if ( _isAudioInitialized )
            {
                var audio = _dataHolder.GeneralStorageData.Audio.Value;
                audio.MasterVolume = (int)_masterVolumeOption.Value;
                audio.DialogueVolume = (int)_dialogueVolumeOption.Value;
                audio.MusicVolume = (int)_musicVolumeOption.Value;
                audio.SFXVolume = (int)_sfxVolumeOption.Value;
                audio.AmbientVolume = (int)_ambientVolumeOption.Value;
            }

            if ( _isGraphicsInitialized )
            {
                var graphics = _dataHolder.GeneralStorageData.Graphics.Value;
                var resolution = Screen.resolutions[ _resolutionOption.Index ];
                graphics.ResolutionWidth = resolution.width;
                graphics.ResolutionHeight = resolution.height;
                graphics.IsFullScreen = _fullScreenOption.IsOn;
                graphics.IsVsync = _vsyncOption.IsOn;
            }

            if ( _isControlsInitialized )
            {
                var controls = _dataHolder.GeneralStorageData.Controls.Value;
                controls.IsSprintToggle = _sprintOption.IsOn;
                controls.IsCrouchToggle = _crouchOption.IsOn;
                controls.MouseXSensitivity = _mouseSensitivityOption.Value;
                controls.MouseYSensitivity = _mouseSensitivityOption.Value;
                controls.ControllerXSensitivity = _controllerXSensitivityOption.Value;
                controls.ControllerYSensitivity = _controllerYSensitivityOption.Value;
            }
            
            _dataHolder.SaveGeneral();

            if ( _isGraphicsInitialized )
            {
                var graphics = _dataHolder.GeneralStorageData.Graphics.Value;
                Screen.SetResolution( graphics.ResolutionWidth, graphics.ResolutionHeight, graphics.IsFullScreen );
                QualitySettings.vSyncCount = graphics.IsVsync ? 1 : 0;
            }
            Application.targetFrameRate = 60;
        }

        private void OnTabButtonClickedHandler( UITab tab )
        {
            TryLoadTab( ModelView.Tabs.IndexOf( tab ) );
        }

        private void LBButtonClickedHandler()
        {
            TryLoadTab( Mathf.Clamp( _currentTabIndex - 1, 0, ModelView.Tabs.Count - 1 ) );
        }
        
        private void RBButtonClickedHandler()
        {
            TryLoadTab( Mathf.Clamp( _currentTabIndex + 1, 0, ModelView.Tabs.Count - 1 ) );
        }

        private void ButtonPointerEnteredHandler( UIOption option )
        {
            for ( int i = 0; i < _options.Count; i++ )
            {
                _options[ i ].Deselect();
            }
            var controller = _options.Find( ( x ) => x.View == option );
            controller.Select();

            _lastOption = controller;
        }

        private void ButtonPointerExitedHandler( UIOption uiOption )
        {
            
        }

        private void NavigateChangedHandler( Vector2 value )
        {
            if ( _lastOption is OptionBarController percentOption )
            {
                if ( value.x < 0 )
                {
                    percentOption.Left();
                }
                else if ( value.x > 0 )
                {
                    percentOption.Right();
                }
            }
            else if ( _lastOption is OptionSelectorController selectorOption )
            {
                if ( value.x < 0 )
                {
                    selectorOption.Left();
                }
                else if ( value.x > 0 )
                {
                    selectorOption.Right();
                }
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