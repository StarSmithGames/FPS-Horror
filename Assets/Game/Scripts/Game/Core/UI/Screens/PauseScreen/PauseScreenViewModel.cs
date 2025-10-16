using Game.Core.UI.OptionsDialog;
using Game.Core.UI.QuitGameDialog;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using PuzzlescapeGames.VVM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.UI.PauseScreen
{
    public sealed class PauseScreenViewModel : ViewModel< UIPauseScreen >
    {
        private List< UIOptionMenuButton > _buttons = new( 3 );
        
        private InputActionValueWrap< Vector2 > _inputActionNavigate;
        private InputActionVoidWrap _inputActionSubmit;
        private InputActionVoidWrap _inputActionCancel;

        private UIOption _lastOption;
        private QuitGameDialogViewModel _quitGameDialog;
        
        private readonly GameManager _gameManager;
        private readonly PauseManager _pauseManager;
        private readonly UIRootGame _uiRootGame;
        
        public PauseScreenViewModel(
            GameManager gameManager,
            PauseManager pauseManager,
            UIRootGame uiRootGame
            )
        {
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
            _pauseManager = pauseManager ?? throw new ArgumentNullException( nameof(pauseManager) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }

        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _buttons.Add( ModelView.ContinueButton );
            _buttons.Add( ModelView.OptionsButton );
            _buttons.Add( ModelView.ExitButton );
            
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                var button = _buttons[ i ];
                button.OnButtonPointerEntered += ButtonPointerEnteredHandler;
                button.OnButtonPointerExited += ButtonPointerExitedHandler;
                button.OnButtonClicked += ButtonClickedHandler;
            }

            GamepadDetector.OnChanged += GamepadChangedHandler;
            
            _inputActionNavigate = InputActionManager.CreateInputActionWrap< Vector2 >( InputManager.Inputs.UI.Navigate );
            // _inputActionSubmit = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Submit, SubmitButtonClickedHandler );
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
            // _inputActionSubmit.Enable();
            _inputActionNavigate.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                var button = _buttons[ i ];
                button.OnButtonPointerEntered -= ButtonPointerEnteredHandler;
                button.OnButtonPointerExited -= ButtonPointerExitedHandler;
                button.OnButtonClicked -= ButtonClickedHandler;
            }
            
            GamepadDetector.OnChanged -= GamepadChangedHandler;
            
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
            // InputActionManager.RemoveInputActionWrap( _inputActionSubmit );
            InputActionManager.RemoveInputActionWrap( _inputActionNavigate );
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing )
            {
                CursorManager.Disable();
                _pauseManager.UnPause();
                _gameManager.SetState( GameState.Game );
                return;
            }
            CursorManager.Enable();
            _pauseManager.Pause();
            _gameManager.SetState( GameState.Menu );
            
            EventSystem.current.SetSelectedGameObject( ModelView.ContinueButton.gameObject );
         
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                _buttons[ i ].Deselect();
            }
            GamepadChangedHandler();
        }
        
        private void GamepadChangedHandler()
        {
            if ( GamepadDetector.IsConnected )
            {
                if ( _buttons.All( ( x ) => !x.IsSelected ) )
                {
                    var button = _buttons.First();
                    button.Select();
                    EventSystem.current.SetSelectedGameObject( button.gameObject );
                }
            }
        }

        private void ButtonPointerEnteredHandler( UIOption option )
        {
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                _buttons[ i ].Deselect();
            }
            option.Select();

            _lastOption = option;
        }

        private void ButtonPointerExitedHandler( UIOption option )
        {
            if ( !GamepadDetector.IsConnected )
            {
                option.Deselect();
            }
        }
        
        private void CancelButtonClickedHandler()
        {
            HideViewAndDispose();
        }
        
        private void SubmitButtonClickedHandler()
        {
            // if ( _lastOption is UIOptionMenuButton )
            // {
            //     _lastOption.Submit();
            // }
        }
        
        private void ButtonClickedHandler( UIOption option )
        {
            _inputActionCancel.Disable();
            _inputActionNavigate.Disable();
            // _inputActionSubmit.Disable();
            
            var index = _buttons.IndexOf( (UIOptionMenuButton)option );
            if ( index == 0 )
            {
                HideViewAndDispose();
            }
            else if ( index == 1 )
            {
                var dialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< OptionsDialogViewModel >();
                dialog.OnShowingChanged += DialogShowingChangedHandler;
                dialog.ShowView();
            }
            else if ( index == 2 )
            {
                _quitGameDialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< QuitGameDialogViewModel >();
                _quitGameDialog.OnShowingChanged += DialogShowingChangedHandler;
                _quitGameDialog.ShowView();
            }
        }

        private void DialogShowingChangedHandler( IViewModel dialog )
        {
            if ( dialog.IsShowing ) return;
            dialog.OnShowingChanged -= DialogShowingChangedHandler;

            _inputActionCancel.Enable();
        }
    }
}