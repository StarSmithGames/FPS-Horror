using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.UI.MenuScreen
{
    public sealed class MenuScreenViewModel : ViewModel< UIMenuScreen >
    {
        private List< UIOptionButton > _buttons = new( 3 );

        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _buttons.Add( ModelView.StartButton );
            _buttons.Add( ModelView.OptionsButton );
            _buttons.Add( ModelView.ExitButton );
            
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                var button = _buttons[ i ];
                button.OnButtonPointerEntered += ButtonPointerEnteredHandler;
                button.OnButtonPointerExited += ButtonPointerExitedHandler;
                button.OnButtonClicked += ButtonClickedHandler;
            }

            InputManager.OnControllerChanged += ControllerChangedHandler;
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
            
            InputManager.OnControllerChanged -= ControllerChangedHandler;
        }

        protected override void OnViewCreated()
        {
            InputManager.Inputs.UI.Navigate.Enable();
            InputManager.Inputs.UI.Submit.Enable();
            EventSystem.current.SetSelectedGameObject( ModelView.StartButton.gameObject );
         
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                _buttons[ i ].Deselect();
            }
            ControllerChangedHandler();
        }

        private void ControllerChangedHandler()
        {
            if ( InputManager.IsController )
            {
                if ( _buttons.All( ( x ) => !x.IsSelected ) )
                {
                    _buttons.First().Select();
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
            if ( !InputManager.IsController )
            {
                button.Deselect();
            }
        }
        
        private void ButtonClickedHandler( UIOptionButton button )
        {
            Debug.LogError( "ButtonClickedHandler" );
            
            // EventSystem.current.SetSelectedGameObject( null );
        }
    }
}