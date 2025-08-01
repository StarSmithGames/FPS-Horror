using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerBrain
    {
        public event Action OnLanded;

        private List< InputHolder > _inputHolders = new();
        private CancellationTokenSource _cancellationTokenSource;
        private Vector2 _moveInput;
        private bool _isSprinting;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerLookController _lookController;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerJumpController _jumpController;
        private readonly CameraFOVController _cameraFOVController;
        
        public PlayerBrain(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            PlayerLookController lookController,
            PlayerMovementController movementController,
            PlayerJumpController jumpController,
            CameraFOVController cameraFOVController
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _lookController = lookController ?? throw new ArgumentNullException( nameof(lookController) );
            _movementController = movementController ?? throw new ArgumentNullException( nameof(movementController) );
            _jumpController = jumpController ?? throw new ArgumentNullException( nameof(jumpController) );
            _cameraFOVController = cameraFOVController ?? throw new ArgumentNullException( nameof(cameraFOVController) );
        }

        public void Initialize()
        {
            _movementController.Initialize();
            _jumpController.Initialize();
            _cameraFOVController.Initialize();

            InputManager.OnJump += JumpClickedHandler;
            _inputHolders.Add( new( InputManager.Inputs.Player.Sprint, onStartHold: SprintStartedHandler, onEndHold: SprintEndedHandler ) );
            InputManager.AddInputHolders( _inputHolders );
            
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
            FixedTick( _cancellationTokenSource.Token ).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            InputManager.OnJump -= JumpClickedHandler;
            for ( int i = 0; i < _inputHolders.Count; i++ )
            {
                InputManager.RemoveInputHolder( _inputHolders[ i ] );
            }
            _inputHolders.Clear();
            
            _cameraFOVController.Dispose();
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _moveInput = InputManager.Inputs.Player.Movement.ReadValue< Vector2 >();
                
                CheckGrounded();

                // if (canWallBounce) CheckOppositeWall();

                // if (InputManager.jumping && wallOpposite && canWallBounce && playerControl.IsControllable && CheckHeight()) WallBounce();

                // HandleStairs( _moveDirection );
                
                _lookController.Look();
                _movementController.HandleVelocities( _isSprinting, false, _moveInput );

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

        private void CheckGrounded()
        {
            if ( _states.IsSteppingStairs )
            {
                _states.IsGrounded = true;
                return;
            }

            Vector3 origin = _view.CapsuleCollider.bounds.center;

            bool foundGround = false;
            if ( Physics.Raycast( origin, Vector3.down, out RaycastHit hit, _config.GroundCheckDistance, _config.GroundLayer ) )
            {
                if ( IsFloor( hit.normal ) )
                {
                    foundGround = true;
                }
            }

            if ( foundGround )
            {
                if ( !_states.IsGrounded )
                {
                    // SoundManager.Instance.PlaySound(sounds.landSFX, 0, 0, false);
                    _jumpController.OnLanded();
                    OnLanded?.Invoke();
                }

                _states.IsGrounded = true;
            }
            else
            {
                if ( _states.IsGrounded )
                {
                    _states.IsGrounded = false;
                }
            }
        }
        
        private bool IsFloor( Vector3 v ) => Vector3.Angle( Vector3.up, v ) < _config.MaxSlopeAngle;

        private void JumpClickedHandler()
        {
            _jumpController.Jump( _moveInput );
        }

        private void SprintStartedHandler()
        {
            _isSprinting = true;
        }
        
        private void SprintEndedHandler()
        {
            _isSprinting = false;
        }
    }
}