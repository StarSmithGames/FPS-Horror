using Game.Core.UI.OptionsDialog;
using Game.Managers.CursorManager;
using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Core.UI.MenuScreen
{
    public sealed class MenuScreenViewModel : ViewModel< UIMenuScreen >
    {
        private List< UIOptionMenuButton > _buttons = new( 3 );

        private UIOption _lastOption;
        
        private readonly GameBoostrap _gameBoostrap;
        private readonly UIRootGame _uiRootGame;
        
        public MenuScreenViewModel(
            GameBoostrap gameBoostrap,
            UIRootGame uiRootGame
            )
        {
            _gameBoostrap = gameBoostrap ?? throw new ArgumentNullException( nameof(gameBoostrap) );
            _uiRootGame = uiRootGame ?? throw new ArgumentNullException( nameof(uiRootGame) );
        }
        
        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _buttons.Add( ModelView.StartButton );
            _buttons.Add( ModelView.OptionsButton );
            _buttons.Add( ModelView.ExitButton );
            
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                var button = _buttons[ i ];
                button.OnPointerEntered += ButtonPointerEnteredHandler;
                button.OnPointerExited += ButtonPointerExitedHandler;
                button.OnButtonClicked += ButtonClickedHandler;
            }

            GamepadDetector.OnChanged += GamepadChangedHandler;
            
            CursorManager.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            for ( int i = 0; i < _buttons.Count; i++ )
            {
                var button = _buttons[ i ];
                button.OnPointerEntered -= ButtonPointerEnteredHandler;
                button.OnPointerExited -= ButtonPointerExitedHandler;
                button.OnButtonClicked -= ButtonClickedHandler;
            }
            
            GamepadDetector.OnChanged -= GamepadChangedHandler;
            
            CursorManager.Disable();
        }

        protected override void OnViewCreated()
        {
            EventSystem.current.SetSelectedGameObject( ModelView.StartButton.gameObject );
         
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
        
        private void ButtonClickedHandler( UIOption option )
        {
            var index = _buttons.IndexOf( (UIOptionMenuButton)option );
            if ( index == 0 )
            {
                _gameBoostrap.Start();
            }
            else if ( index == 1 )
            {
                var dialog = _uiRootGame.DialogAggregator.GetOrCreateIfNotExist< OptionsDialogViewModel >();
                dialog.OnShowingChanged += DialogShowingChangedHandler;
                dialog.ShowView();
            }
            else if ( index == 2 )
            {
                Application.Quit();
            }
        }
        
        private void DialogShowingChangedHandler( IViewModel dialog )
        {
            if ( dialog.IsShowing ) return;
            dialog.OnShowingChanged -= DialogShowingChangedHandler;

            EventSystem.current.SetSelectedGameObject( _lastOption.gameObject );
        }
    }
}