using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerBrain
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Vector2 _moveInput;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerLookController _lookController;
        private readonly PlayerMovementController _movementController;
        
        public PlayerBrain(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            PlayerLookController lookController,
            PlayerMovementController movementController
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _lookController = lookController ?? throw new ArgumentNullException( nameof(lookController) );
            _movementController = movementController ?? throw new ArgumentNullException( nameof(movementController) );
        }

        public void Initialize()
        {
            _movementController.Initialize();
            
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
            FixedTick( _cancellationTokenSource.Token ).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            _movementController.Dispose();
        }


        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _moveInput = InputManager.Inputs.Player.Movement.ReadValue< Vector2 >();
                
                _movementController.CheckGrounded();

                // if (canWallBounce) CheckOppositeWall();

                // if (InputManager.jumping && wallOpposite && canWallBounce && playerControl.IsControllable && CheckHeight()) WallBounce();

                // HandleStairs( _moveDirection );
                
                _lookController.Look();
                
                await UniTask.Yield( PlayerLoopTiming.Update );
            }
        }

        private async UniTask FixedTick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                // Added Gravity
                // Gravity is added only if we are not on a slope or climbing to prevent unvoluntary sliding
                if ( !_movementController.IsOnSlope() && !_states.IsClimbing ) _view.Rigidbody.AddForce( Vector3.down * 30.19f, ForceMode.Acceleration );

                _movementController.Movement( _moveInput );
                
                await UniTask.Yield( PlayerLoopTiming.FixedUpdate );
            }
        }
    }
}