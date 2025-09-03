using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.VVM;
using StarSmithGames.Localization;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialogViewModel : ViewModel< OptionsDialog >
    {
        private int _currentTabIndex;
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly InputActionHolder _inputActionCancel;
        private readonly ILocalizationSystem _localizationSystem;
        
        public OptionsDialogViewModel( ILocalizationSystem localizationSystem )
        {
            _localizationSystem = localizationSystem ?? throw new ArgumentNullException( nameof(localizationSystem) );
            
            _inputActionCancel = new( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();

            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked += OnTabButtonClicked;
            }
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            for ( int i = 0; i < ModelView.Tabs.Count; i++ )
            {
                ModelView.Tabs[ i ].OnButtonClicked -= OnTabButtonClicked;
            }
        }

        protected override void OnViewShowingChanged()
        {
            if ( IsShowing )
            {
                _inputActionCancel.Enable();

                _cancellationTokenSource = new();
                LoadOptions( 0, _cancellationTokenSource.Token ).Forget();
            }
            else
            {
                _inputActionCancel.Disable();
            }
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
                var masterVolume = GameObject.Instantiate( ModelView.OptionLeftRightSelectorPrefab, ModelView.Content );
                masterVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_MASTER_VOLUME ) );
                masterVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var dialogueVolume = GameObject.Instantiate( ModelView.OptionLeftRightSelectorPrefab, ModelView.Content );
                dialogueVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_DIALOGUE_VOLUME ) );
                dialogueVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var musicVolume = GameObject.Instantiate( ModelView.OptionLeftRightSelectorPrefab, ModelView.Content );
                musicVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_MUSIC_VOLUME ) );
                musicVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var sfxVolume = GameObject.Instantiate( ModelView.OptionLeftRightSelectorPrefab, ModelView.Content );
                sfxVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_SFX_VOLUME ) );
                sfxVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            
                var ambientVolume = GameObject.Instantiate( ModelView.OptionLeftRightSelectorPrefab, ModelView.Content );
                ambientVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_AMBIENT_VOLUME ) );
                ambientVolume.SetText( $"{100}%" );
                await UniTask.Yield();
                cancellationToken.ThrowIfCancellationRequested();
            }

            async UniTask LoadGraphics( CancellationToken cancellationToken = default )
            {
                var masterVolume = GameObject.Instantiate( ModelView.OptionLeftRightSelectorPrefab, ModelView.Content );
                masterVolume.SetName( _localizationSystem.Translate( LocalizationIds.UI_OPTIONS_DIALOG_RESOLUTION ) );
                masterVolume.SetText( $"{Screen.currentResolution.width}x{Screen.currentResolution.height}" );
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
            HideViewAndDispose();
        }
    }
}