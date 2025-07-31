using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerController
    {
        public PlayerObject View { get; }
        public PlayerConfig Config { get; }
        
        public PlayerFacade Facade { get; }
        

        private readonly PlayerBrain _brain;
        
        public PlayerController(
            PlayerObject view,
            PlayerConfig config,
            PlayerFacade facade,
            PlayerBrain brain
            )
        {
            View = view ?? throw new ArgumentNullException( nameof(view) );
            Config = config ?? throw new ArgumentNullException( nameof(config) );
            Facade = facade ?? throw new ArgumentNullException( nameof(facade) );
            _brain = brain ?? throw new ArgumentNullException( nameof(brain) );
            
            View.SetController( this );
        }
        
        public void Initialize()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            _brain.Initialize();
        }

        public void Dispose()
        {
            _brain.Dispose();
        }
    }
}