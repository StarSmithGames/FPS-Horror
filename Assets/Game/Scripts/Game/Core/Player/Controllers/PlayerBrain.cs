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

        private List< InputActionHolder > _inputHolders = new();
        private CancellationTokenSource _cancellationTokenSource;
        private Vector2 _moveInput;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerLookController _lookController;
        private readonly PlayerMovementController _movementController;
        private readonly PlayerJumpController _jumpController;
        private readonly PlayerCrouchController _crouchController;
        private readonly CameraFOVController _cameraFOVController;
        private readonly CameraVisionController _cameraVisionController;
        private readonly PlayerHoveringController _hoveringController;
        private readonly PlayerInputActionsController _inputActionsController;
        private readonly PlayerSoundController _soundController;
        
        public PlayerBrain(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            PlayerLookController lookController,
            PlayerMovementController movementController,
            PlayerJumpController jumpController,
            PlayerCrouchController crouchController,
            
            CameraFOVController cameraFOVController,
            CameraVisionController cameraVisionController,
            PlayerHoveringController hoveringController,
            PlayerInputActionsController inputActionsController,
            PlayerSoundController soundController
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _lookController = lookController ?? throw new ArgumentNullException( nameof(lookController) );
            _movementController = movementController ?? throw new ArgumentNullException( nameof(movementController) );
            _jumpController = jumpController ?? throw new ArgumentNullException( nameof(jumpController) );
            _crouchController = crouchController ?? throw new ArgumentNullException( nameof(crouchController) );
            _cameraFOVController = cameraFOVController ?? throw new ArgumentNullException( nameof(cameraFOVController) );
            _cameraVisionController = cameraVisionController ?? throw new ArgumentNullException( nameof(cameraVisionController) );
            _hoveringController = hoveringController ?? throw new ArgumentNullException( nameof(hoveringController) );
            _inputActionsController = inputActionsController ?? throw new ArgumentNullException( nameof(inputActionsController) );
            _soundController = soundController ?? throw new ArgumentNullException( nameof(soundController) );
        }

        public void Initialize()
        {
            _movementController.Initialize();
            _jumpController.Initialize();
            _crouchController.Initialize();
            _cameraFOVController.Initialize();
            _cameraVisionController.Initialize();
            _hoveringController.Initialize();

            InputManager.OnJump += JumpClickedHandler;
            _inputHolders.Add( new( InputManager.Inputs.Player.Crouch, onStartHold: _crouchController.StartCrouch, onEndHold: _crouchController.StopCrouch ) );
            InputManager.AddInputHolders( _inputHolders );
            
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
            LastTick( _cancellationTokenSource.Token ).Forget();
            FixedTick( _cancellationTokenSource.Token ).Forget();
            
            _inputActionsController.Enable();
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
            _cameraVisionController.Dispose();
            _hoveringController.Dispose();
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _moveInput = InputManager.Inputs.Player.Movement.ReadValue< Vector2 >();
                _states.IsSprinting = InputManager.Inputs.Player.Sprint.IsPressed();
                
                CheckGrounded();

                // if (canWallBounce) CheckOppositeWall();

                // if (InputManager.jumping && wallOpposite && canWallBounce && playerControl.IsControllable && CheckHeight()) WallBounce();

                // HandleStairs( _moveDirection );
                
                _movementController.HandleVelocities( false, _moveInput );
                
                _soundController.FootSteps();

                await UniTask.Yield( PlayerLoopTiming.Update );
            }
        }
        
        private async UniTask LastTick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                if ( !_states.IsBlocked )
                {
                    _lookController.Look();
                }

                await UniTask.Yield( PlayerLoopTiming.LastUpdate );
            }
        }

        private async UniTask FixedTick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _movementController.Movement( _crouchController.IsSliding(), _moveInput );
                
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
            if ( Physics.Raycast( origin, Vector3.down, out RaycastHit hit, _config.GroundSettings.GroundCheckDistance, _config.GroundSettings.GroundLayer ) )
            {
                if ( _movementController.IsFloor( hit.normal ) )
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
        
        //FootSteps
        //AimAssist
        //Climbing Ladders
        //Stamina
        
        private void JumpClickedHandler()
        {
            if ( _states.IsBlocked ) return;
            
            _jumpController.Jump( _moveInput );
        }
    }
}