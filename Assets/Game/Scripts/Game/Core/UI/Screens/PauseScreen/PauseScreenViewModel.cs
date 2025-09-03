using Game.Core.UI.OptionsDialog;
using Game.Core.UI.QuitGameDialog;
using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using PuzzlescapeGames.Extensions;
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
        private List< UIOptionButton > _buttons = new( 3 );
        
        private readonly InputActionWrap _inputActionCancel;
        
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

            _inputActionCancel = new( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
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
            
            InputManager.Inputs.UI.Navigate.Enable();
            InputManager.Inputs.UI.Submit.Enable();
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
            
            InputManager.Inputs.UI.Navigate.Disable();
            InputManager.Inputs.UI.Submit.Disable();
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing )
            {
                _inputActionCancel.Disable();
                CursorManager.Disable();
                _pauseManager.UnPause();
                _gameManager.SetState( GameState.Game );
                return;
            }
            _inputActionCancel.Enable();
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

        private void ButtonPointerEnteredHandler( UIOptionButton button )
        {
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                _buttons[ i ].Deselect();
            }
            button.Select();
        }

        private void ButtonPointerExitedHandler( UIOptionButton button )
        {
            if ( !GamepadDetector.IsConnected )
            {
                button.Deselect();
            }
        }

        private void CancelButtonClickedHandler()
        {
            HideViewAndDispose();
        }
        
        private void ButtonClickedHandler( UIOptionButton button )
        {
            var index = _buttons.IndexOf( button );
            if ( index == 0 )
            {
                HideViewAndDispose();
            }
            else if ( index == 1 )
            {
                _inputActionCancel.Disable();
                var dialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< OptionsDialogViewModel >();
                dialog.OnShowingChanged += DialogShowingChangedHandler;
                dialog.ShowView();
            }
            else if ( index == 2 )
            {
                _inputActionCancel.Disable();
                var dialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< QuitGameDialogViewModel >();
                // dialog.SetAcceptAction( Application.Quit );
                dialog.OnShowingChanged += DialogShowingChangedHandler;
                dialog.ShowView();
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