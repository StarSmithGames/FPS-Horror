using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerInputActionsController
    {
        private readonly InputActionHolder _inputActionLighter;
        private readonly PlayerInventoryController _inventoryController;
        public PlayerInputActionsController(
            InputKeyActionsSettings inputKeyActionsSettings,
            PlayerInventoryController inventoryController
            )
        {
            _inputActionLighter = new( inputKeyActionsSettings.LighterAction.InputAction, InputLighterCompletedHandler );
            _inventoryController = inventoryController ?? throw new ArgumentNullException( nameof(inventoryController) );
        }
        
        public void Enable()
        {
            _inputActionLighter.Enable();
        }

        public void Disable()
        {
            _inputActionLighter.Disable();
        }

        private void InputLighterCompletedHandler()
        {
            _inventoryController.SelectLighter();
        }
    }
}