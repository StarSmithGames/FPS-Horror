using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;
using UnityEngine;

namespace Game.Core.UI.QuitGameDialog
{
    public sealed class QuitGameDialogViewModel : ViewModel< QuitGameDialog >
    {
        private readonly InputActionHolder _inputActionCancel;
        
        public QuitGameDialogViewModel()
        {
            _inputActionCancel = new( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
        }
        
        protected override void SubscribeView()
        {
            base.SubscribeView();

            ModelView.OnAcceptButtonClicked += AcceptButtonClickedHandler;
            ModelView.OnRejectButtonClicked += RejectButtonClickedHandler;
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            ModelView.OnAcceptButtonClicked -= AcceptButtonClickedHandler;
            ModelView.OnRejectButtonClicked -= RejectButtonClickedHandler;
        }

        protected override void OnViewShowingChanged()
        {
            if ( IsShowing )
            {
                _inputActionCancel.Enable();
            }
            else
            {
                _inputActionCancel.Disable();
            }
        }

        private void AcceptButtonClickedHandler()
        {
            Application.Quit();
        }

        private void RejectButtonClickedHandler()
        {
            HideViewAndDispose();
        }

        private void CancelButtonClickedHandler()
        {
            HideViewAndDispose();
        }
    }
}