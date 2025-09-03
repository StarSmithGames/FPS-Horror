using Cysharp.Threading.Tasks;
using Game.Core.UI.SaveOptionsDialog;
using Game.Managers.InputManager;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.VVM;
using StarSmithGames.Localization;
using System;
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
        private OptionLeftRightController _resolutionOption;
        
        private readonly UIRootGame _uiRootGame;
        private readonly ILocalizationSystem _localizationSystem;
        
        public OptionsDialogViewModel(
            UIRootGame uiRootGame,
            ILocalizationSystem localizationSystem
            )
        {
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();

            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked += OnTabButtonClicked;
            }
            
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked -= OnTabButtonClicked;
            }
            
            _inputActionCancel.Disable();
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !IsShowing ) return;

            _cancellationTokenSource = new();
            LoadOptions( 0, _cancellationTokenSource.Token ).Forget();
        }

        private async UniTask LoadOptions( int index, CancellationToken cancellationToken = default )
        {
            _currentTabIndex = index;
            
            ModelView.Content.DestroyChildren();
            await UniTask.Yield();
            cancellationToken.ThrowIfCancellationRequested();

            if ( index == 1 )
            {
                await LoadAudio( cancellationToken );
            }
            else if ( index == 2 )
            {
                await LoadGraphics( cancellationToken );
            }

            async UniTask LoadAudio( CancellationToken cancellationToken = default )
            {
                var masterVolume = GameObject.Instantiate( ModelView.OptionLeftRightPrefab, ModelView.Content );
                masterVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_MASTER_VOLUME ) );
                masterVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var dialogueVolume = GameObject.Instantiate( ModelView.OptionLeftRightPrefab, ModelView.Content );
                dialogueVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_DIALOGUE_VOLUME ) );
                dialogueVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var musicVolume = GameObject.Instantiate( ModelView.OptionLeftRightPrefab, ModelView.Content );
                musicVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_MUSIC_VOLUME ) );
                musicVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var sfxVolume = GameObject.Instantiate( ModelView.OptionLeftRightPrefab, ModelView.Content );
                sfxVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_SFX_VOLUME ) );
                sfxVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var ambientVolume = GameObject.Instantiate( ModelView.OptionLeftRightPrefab, ModelView.Content );
                ambientVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_AMBIENT_VOLUME ) );
                ambientVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }

            async UniTask LoadGraphics( CancellationToken cancellationToken = default )
            {
                //RESOLUTION
                var view = GameObject.Instantiate( ModelView.OptionLeftRightPrefab, ModelView.Content );
                view.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_RESOLUTION ) );
                _resolutionOption = new( view );
                var resolutions = Screen.resolutions.ToList();
                _resolutionOption.Initialize( resolutions.IndexOf( Screen.currentResolution ), resolutions.Select( ( x ) => $"{x.width}x{x.height}" ).ToArray() );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        private void OnTabButtonClicked( UITab tab )
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
            _inputActionCancel.Disable();
            
            var dialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< SaveOptionsDialogViewModel >();
            dialog.OnAcceptRejectShowingChanged += AcceptRejectShowingChanged;
            dialog.ShowView();
        }

        private void AcceptRejectShowingChanged( IViewModel dialog, bool result )
        {
            var acceptReject = (SaveOptionsDialogViewModel)dialog;
            acceptReject.OnAcceptRejectShowingChanged -= AcceptRejectShowingChanged;
            
            _inputActionCancel.Enable();

            if ( result )
            {
                Debug.LogError( "Save" );
                HideViewAndDispose();
            }
        }
    }
}