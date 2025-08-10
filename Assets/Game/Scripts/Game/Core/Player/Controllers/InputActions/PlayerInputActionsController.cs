using Game.Managers.InputManager;
using System;

namespace Game.Core.Player
{
    public sealed class PlayerInputActionsController
    {
        private readonly InputHolder _inputLighter;
        private readonly PlayerInventoryController _inventoryController;
        public PlayerInputActionsController(
            InputKeyActionsSettings inputKeyActionsSettings,
            PlayerInventoryController inventoryController
            )
        {
            _inputLighter = new( inputKeyActionsSettings.LighterAction.InputAction, InputLighterCompletedHandler );
            _inventoryController = inventoryController ?? throw new ArgumentNullException( nameof(inventoryController) );
        }
        
        public void Enable()
        {
            _inputLighter.Enable();
        }

        public void Disable()
        {
            _inputLighter.Disable();
        }

        private void InputLighterCompletedHandler()
        {
            _inventoryController.SelectLighter();
        }
    }
}