using System;

namespace Game.Core.Player
{
    public sealed class PlayerFacade
    {
        public PlayerMovementController MovementController { get; }

        public PlayerFacade( PlayerMovementController movementController )
        {
            MovementController = movementController ?? throw new ArgumentNullException( nameof(movementController) );
        }
    }
}