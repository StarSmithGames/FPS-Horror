using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;
using System;
using System.Linq;
using UnityEngine.EventSystems;

namespace Game.Core.UI.Dialogs
{
    public class AcceptRejectModalDialogViewModel< T > : ViewModel< T >
        where T : AcceptRejectModalDialog
    {
        public event Action< IViewModel, bool > OnAcceptRejectShowingChanged;
        
        private InputActionVoidWrap _inputActionSubmit;
        private InputActionVoidWrap _inputActionCancel;
        
        protected override void SubscribeView()
        {
            base.SubscribeView();

            ModelView.OnAcceptButtonClicked += AcceptButtonClickedHandler;
            ModelView.OnRejectButtonClicked += RejectButtonClickedHandler;
            
            _inputActionSubmit = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Submit, AcceptButtonClickedHandler );
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, RejectButtonClickedHandler );
            _inputActionSubmit.Enable();
            _inputActionCancel.Enable();

            for ( int i = 0; i < ModelView.Buttons.Count; i++ )
            {
                ModelView.Buttons[ i ].OnPointerEntered += ButtonPointerEnteredHandler;
            }
            EventSystem.current.SetSelectedGameObject( ModelView.Buttons.Last().gameObject );
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            ModelView.OnAcceptButtonClicked -= AcceptButtonClickedHandler;
            ModelView.OnRejectButtonClicked -= RejectButtonClickedHandler;
            
            for ( int i = 0; i < ModelView.Buttons.Count; i++ )
            {
                ModelView.Buttons[ i ].OnPointerEntered -= ButtonPointerEnteredHandler;
            }
            
            InputActionManager.RemoveInputActionWrap( _inputActionSubmit );
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );

            OnAcceptRejectShowingChanged = null;
        }
        
        private void ButtonPointerEnteredHandler( UIOption option )
        {
            for ( int i = 0; i < ModelView.Buttons.Count; i++ )
            {
                ModelView.Buttons[ i ].Deselect();
            }
            option.Select();
        }

        private void AcceptButtonClickedHandler()
        {
            OnAcceptRejectShowingChanged?.Invoke( this, true );
            HideViewAndDispose();
        }

        private void RejectButtonClickedHandler()
        {
            OnAcceptRejectShowingChanged?.Invoke( this, false );
            HideViewAndDispose();
        }
    }
}