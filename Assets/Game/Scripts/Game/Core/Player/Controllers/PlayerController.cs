using Game.Core.World.InteractionSystem;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerController : IInteractor
    {
        public PlayerObject View { get; }
        public PlayerConfig Config { get; }

        public IServiceLocator ServiceLocator => _brain.ServiceLocator;

        private readonly PlayerBrain _brain;
        
        public PlayerController(
            PlayerObject view,
            PlayerConfig config,
            PlayerBrain brain
            )
        {
            View = view ?? throw new ArgumentNullException( nameof(view) );
            Config = config ?? throw new ArgumentNullException( nameof(config) );
            _brain = brain ?? throw new ArgumentNullException( nameof(brain) );
            
            View.SetController( this );
        }
        
        public void Initialize()
        {
            _brain.Initialize();
        }

        public void Dispose()
        {
            _brain.Dispose();
        }

        public void Teleport( Vector3 targetPosition, Vector3 targetRotation )
        {
            ServiceLocator.GetAs< PlayerMoveController >().SetPosition( targetPosition );
            ServiceLocator.GetAs< PlayerLookController >().SetRotation( targetRotation );
        }
    }
}