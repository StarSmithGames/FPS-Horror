using Cysharp.Threading.Tasks;
using Game.Core.UI.SaveOptionsDialog;
using Game.Managers.InputManager;
using Game.Systems.StorageSystem;
using PuzzlescapeGames.Extensions;
using PuzzlescapeGames.Localization;
using PuzzlescapeGames.VVM;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialogViewModel : ViewModel< OptionsDialog >
    {
        private int _currentTabIndex = -1;
        private CancellationTokenSource _cancellationTokenSource;

        private GameplayTabController _gameplayTabController;
        private AudioTabController _audioTabController;
        private GraphicsTabController _graphicsTabController;
        private ControlsTabController _controlsTabController;

        private InputActionVoidWrap _inputActionCancel;
        private InputActionVoidWrap _inputActionLB;
        private InputActionVoidWrap _inputActionRB;
        
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
            _inputActionCancel.Enable();
            _inputActionLB.Enable();
            _inputActionRB.Enable();

            GamepadDetector.OnChanged += GamepadChangedHandler;
            GamepadChangedHandler();
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
            
            _gameplayTabController?.Dispose();
            _audioTabController?.Dispose();
            _graphicsTabController?.Dispose();
            _controlsTabController?.Dispose();

            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
            InputActionManager.RemoveInputActionWrap( _inputActionLB );
            InputActionManager.RemoveInputActionWrap( _inputActionRB );
            
            GamepadDetector.OnChanged -= GamepadChangedHandler;
        }

        protected override void OnViewShowingChanged()
        {
            if ( !IsShowing ) return;
            
            for ( int i = 0; i < ModelView.Contents.Count; i++ )
            {
                ModelView.Contents[ i ].DestroyChildren();
            }
            
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
            for ( int i = 0; i < ModelView.TabsRoots.Count; i++ )
            {
                ModelView.TabsRoots[ i ].gameObject.SetActive( false );
            }
            ModelView.TabsRoots[ index ].gameObject.SetActive( true );
            ModelView.SetScrollRectContent( (RectTransform)ModelView.TabsRoots[ index ] );
            ModelView.EnterKey.Enable( false );
            
            // ModelView.EnableControlTip( false );
            
            if ( index == 0 )
            {
                
            }
            if ( index == 1 )
            {
                if ( _gameplayTabController == null )
                {
                    _gameplayTabController = new( ModelView.TabsSettings, _dataHolder, _localizationSystem );
                    _gameplayTabController.Subscribe();
                }
                _gameplayTabController.InitializeAndLoad( ModelView.Contents[ index ], cancellationToken ).Forget();
            }
            else if ( index == 2 )
            {
                if ( _audioTabController == null )
                {
                    _audioTabController = new( ModelView.TabsSettings, _dataHolder, _localizationSystem );
                    _audioTabController.Subscribe();
                }
                _audioTabController.InitializeAndLoad( ModelView.Contents[ index ], cancellationToken ).Forget();
            }
            else if ( index == 3 )
            {
                if ( _graphicsTabController == null )
                {
                    _graphicsTabController = new( ModelView.TabsSettings, _dataHolder, _localizationSystem );
                    _graphicsTabController.Subscribe();
                }
                _graphicsTabController.InitializeAndLoad( ModelView.Contents[ index ], cancellationToken ).Forget();
            }
            else if ( index == 4 )
            {
                if ( _controlsTabController == null )
                {
                    _controlsTabController = new( ModelView.TabsSettings, _dataHolder, _localizationSystem );
                    _controlsTabController.Subscribe();
                }
                _controlsTabController.InitializeAndLoad( ModelView.Contents[ index ], cancellationToken ).Forget();
                
                // ModelView.EnableControlTip( true );
            }
        }

        private void Save()
        {
            _gameplayTabController?.Save();
            _audioTabController?.Save();
            _graphicsTabController?.Save();
            _controlsTabController?.Save();
            
            _dataHolder.SaveGeneral();
        }

        private void GamepadChangedHandler()
        {
            ModelView.SetTips( GamepadDetector.IsConnected ? 0 : -1 );
            ModelView.SetControlTip( GamepadDetector.IsConnected ? 0 : -1 );
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
        
        private void CancelButtonClickedHandler()
        {
            bool isGameplay = _gameplayTabController != null && _gameplayTabController.IsDirty();
            bool isAudio = _audioTabController != null && _audioTabController.IsDirty();
            bool isGraphics = _graphicsTabController != null && _graphicsTabController.IsDirty();
            bool isControls = _controlsTabController != null && _controlsTabController.IsDirty();
            
            if ( isGameplay ||
                 isAudio ||
                 isGraphics ||
                 isControls )
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