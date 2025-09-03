using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;
using System;

namespace Game.Core.UI.Dialogs
{
    public class AcceptRejectModalDialogViewModel< T > : ViewModel< T >
        where T : AcceptRejectModalDialog
    {
        public event Action< IViewModel, bool > OnAcceptRejectShowingChanged;
        
        private InputActionWrap _inputActionSubmit;
        private InputActionWrap _inputActionCancel;
        
        protected override void SubscribeView()
        {
            base.SubscribeView();

            ModelView.OnAcceptButtonClicked += AcceptButtonClickedHandler;
            ModelView.OnRejectButtonClicked += RejectButtonClickedHandler;
            
            _inputActionSubmit = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Submit, AcceptButtonClickedHandler );
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, RejectButtonClickedHandler );
            
            _inputActionSubmit.Enable();
            _inputActionCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            ModelView.OnAcceptButtonClicked -= AcceptButtonClickedHandler;
            ModelView.OnRejectButtonClicked -= RejectButtonClickedHandler;

            _inputActionSubmit.Disable();
            _inputActionCancel.Disable();
            InputActionManager.RemoveInputActionWrap( _inputActionSubmit );
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );

            OnAcceptRejectShowingChanged = null;
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