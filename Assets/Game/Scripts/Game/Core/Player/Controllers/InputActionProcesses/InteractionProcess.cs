using Game.Core.UI;
using Game.Core.UI.GameScreen;
using Game.Core.World.InteractionSystem;
using Game.Managers.InputManager;
using System;

namespace Game.Core.Player.InputActionProcesses
{
    public sealed class InteractionProcess
    {
        private UIInfoButton _ui;
        private IInteractable _interactable;
        
        private readonly InputActionProvider _provider;

        public InteractionProcess( Func< bool > breaker )
        {
            _provider = new( InputManager.Inputs.Player.Interact, Interaction, 0.33f, breaker: breaker, progress: InteractionProgress, callback: InteractionFinished );
        }

        public void Enable( UIInfoButton ui, IInteractable interactable )
        {
            _ui = ui ?? throw new ArgumentNullException( nameof(ui) );
            _interactable = interactable ?? throw new ArgumentNullException( nameof(interactable) );
            _provider.Enable();
        }

        public void Disable()
        {
            _provider.Disable();
            _ui = null;
            _interactable = null;
        }

        private void Interaction()
        {
            _interactable.Interact();
        }

        private void InteractionProgress( float value )
        {
            _ui.SetFillAmount( value );
        }

        private void InteractionFinished( bool result )
        {
            _ui.SetFillAmount( 0 );
        }
    }
}