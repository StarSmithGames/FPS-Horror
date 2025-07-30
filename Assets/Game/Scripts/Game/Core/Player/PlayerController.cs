using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerController
    {
        public PlayerObject View { get; }
        public PlayerConfig Config { get; }
        
        public PlayerFacade Facade { get; }
        
        public PlayerController(
            PlayerObject view,
            PlayerConfig config,
            PlayerFacade facade
            )
        {
            View = view ?? throw new ArgumentNullException( nameof(view) );
            Config = config ?? throw new ArgumentNullException( nameof(config) );
            Facade = facade ?? throw new ArgumentNullException( nameof(facade) );
            
            View.SetController( this );
        }
        
        public void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            Facade.MovementController.Initialize();
        }

        public void Dispose()
        {
            Facade.MovementController.Dispose();
        }
    }
}