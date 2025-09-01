using Game.Managers.CursorManager;
using Game.Managers.GameManager;
using Game.Managers.InputManager;
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
        
        private readonly GameManager _gameManager;

        public PauseScreenViewModel( GameManager gameManager )
        {
            _gameManager = gameManager ?? throw new ArgumentNullException( nameof(gameManager) );
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
            
            CursorManager.Enable();
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
            
            CursorManager.Disable();
        }

        protected override void OnViewShowingChanged()
        {
            if ( !ModelView.IsShowing ) return;
            
            _gameManager.SetState( GameState.Pause );
            
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
        
        private void ButtonClickedHandler( UIOptionButton button )
        {
            var index = _buttons.IndexOf( button );
            if ( index == 0 )
            {
                HideViewAndDispose();
                _gameManager.SetState( GameState.Game );
            }
            else if ( index == 1 )
            {
                
            }
            else if ( index == 2 )
            {
                Application.Quit();
            }
        }
    }
}