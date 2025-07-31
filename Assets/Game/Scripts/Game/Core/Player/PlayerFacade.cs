using System;

namespace Game.Core.Player
{
    public sealed class PlayerFacade
    {
        public PlayerLookController LookController { get; }
        public PlayerMovementController MovementController { get; }

        public PlayerFacade(
            PlayerLookController lookController,
            PlayerMovementController movementController
            )
        {
            LookController = lookController ?? throw new ArgumentNullException( nameof(lookController) );
            MovementController = movementController ?? throw new ArgumentNullException( nameof(movementController) );
        }
    }
}