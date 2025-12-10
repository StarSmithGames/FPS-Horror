using Game.Managers.InputManager;
using PuzzlescapeGames.VVM;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class ResourcesScreenViewModel : ViewModel< UIResourcesScreen >
    {
        private InputActionVoidWrap _inputActionCancel;
        
        protected override void SubscribeView()
        {
            base.SubscribeView();
            
            _inputActionCancel = InputActionManager.CreateInputActionWrap( InputManager.Inputs.UI.Cancel, CancelButtonClickedHandler );
            _inputActionCancel.Enable();
        }

        protected override void UnSubscribeView()
        {
            base.UnSubscribeView();
            
            InputActionManager.RemoveInputActionWrap( _inputActionCancel );
        }

        private void CancelButtonClickedHandler()
        {
            HideViewAndDispose();
        }
    }
}