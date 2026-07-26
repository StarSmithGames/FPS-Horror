using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using Game.Managers.PauseManager;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerBrain : IPauseable
    {
        public event Action OnLanded;

        public IServiceLocator ServiceLocator { get; }
        
        private CancellationTokenSource _cancellationTokenSource;
        private Vector2 _moveInput;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly PlayerLookController _lookController;
        private readonly PlayerMoveController _moveController;
        private readonly PlayerJumpController _jumpController;
        private readonly PlayerCrouchController _crouchController;
        private readonly PlayerInteractablesController _playerInteractablesController;
        private readonly PlayerFOVController _playerFOVController;
        private readonly PlayerHoveringController _hoveringController;
        private readonly PlayerInputActionsController _inputActionsController;
        private readonly PlayerInventoryController _inventoryController;
        private readonly PlayerMenuTransitionController _menuTransitionController;
        private readonly PlayerJournalController _journalController;
        private readonly PlayerEquipmentController _equipmentController;
        private readonly PlayerLibraryController _libraryController;
        private readonly PlayerInspectionController _inspectionController;
        private readonly PlayerSoundController _soundController;
        private readonly PauseManager _pauseManager;
        
        public PlayerBrain(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            PlayerLookController lookController,
            PlayerMoveController moveController,
            PlayerJumpController jumpController,
            PlayerCrouchController crouchController,
            
            PlayerInteractablesController playerInteractablesController,
            PlayerFOVController playerFOVController,
            PlayerHoveringController hoveringController,
            PlayerInputActionsController inputActionsController,
            PlayerMenuTransitionController menuTransitionController,
            PlayerInventoryController inventoryController,
            PlayerJournalController journalController,
            PlayerEquipmentController equipmentController,
            PlayerLibraryController libraryController,
            PlayerInspectionController inspectionController,
            PlayerSoundController soundController,
            
            PauseManager pauseManager
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _lookController = lookController ?? throw new ArgumentNullException( nameof(lookController) );
            _moveController = moveController ?? throw new ArgumentNullException( nameof(moveController) );
            _jumpController = jumpController ?? throw new ArgumentNullException( nameof(jumpController) );
            _crouchController = crouchController ?? throw new ArgumentNullException( nameof(crouchController) );
            _playerInteractablesController = playerInteractablesController ?? throw new ArgumentNullException( nameof(playerInteractablesController) );
            _playerFOVController = playerFOVController ?? throw new ArgumentNullException( nameof(playerFOVController) );
            _hoveringController = hoveringController ?? throw new ArgumentNullException( nameof(hoveringController) );
            _inputActionsController = inputActionsController ?? throw new ArgumentNullException( nameof(inputActionsController) );
            _menuTransitionController = menuTransitionController ?? throw new ArgumentNullException( nameof(menuTransitionController) );
            _inventoryController = inventoryController ?? throw new ArgumentNullException( nameof(inventoryController) );
            _journalController = journalController ?? throw new ArgumentNullException( nameof(journalController) );
            _equipmentController = equipmentController ?? throw new ArgumentNullException( nameof(equipmentController) );
            _libraryController = libraryController ?? throw new ArgumentNullException( nameof(libraryController) );
            _inspectionController = inspectionController ?? throw new ArgumentNullException( nameof(inspectionController) );
            _soundController = soundController ?? throw new ArgumentNullException( nameof(soundController) );
            _pauseManager = pauseManager ?? throw new ArgumentNullException( nameof(pauseManager) );

            ServiceLocator = new ServiceLocator();
            ServiceLocator.Register( _lookController );
            ServiceLocator.Register( _moveController );
            ServiceLocator.Register( _jumpController );
            ServiceLocator.Register( _crouchController );
            ServiceLocator.Register( _playerInteractablesController );
            ServiceLocator.Register( _playerFOVController );
            ServiceLocator.Register( _hoveringController );
            ServiceLocator.Register( _inputActionsController );
            ServiceLocator.Register( _menuTransitionController );
            ServiceLocator.Register( _inventoryController );
            ServiceLocator.Register( _journalController );
            ServiceLocator.Register( _equipmentController );
            ServiceLocator.Register( _libraryController );
            ServiceLocator.Register( _inspectionController );
            ServiceLocator.Register( _soundController );
        }

        public void Initialize()
        {
            _moveController.Initialize();
            _jumpController.Initialize();
            _crouchController.Initialize();
            _playerInteractablesController.Initialize();
            _playerFOVController.Initialize();
            _hoveringController.Initialize();
            _inputActionsController.Initialize();
            _menuTransitionController.Initialize();

            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
            FixedTick( _cancellationTokenSource.Token ).Forget();

            _inputActionsController.Enable();
            _menuTransitionController.Enable();
            
            _pauseManager.AddObserver( this );
        }

        public void Dispose()
        {
            _pauseManager.RemoveObserver( this );
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            
            _inputActionsController.Disable();
            _menuTransitionController.Disable();
            
            _playerFOVController.Dispose();
            _hoveringController.Dispose();
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
                
                _moveController.HandleVelocities( false, _moveInput );
                
                _soundController.FootSteps();

                await UniTask.Yield( PlayerLoopTiming.Update );
            }
        }

        private async UniTask FixedTick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _moveController.Movement( _crouchController.IsSliding(), _moveInput );
                
                if ( !_states.IsBlocked )
                {
                    _lookController.Look();
                }
                
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
                if ( _moveController.IsFloor( hit.normal ) )
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
        
        public void Pause()
        {
            _states.IsBlocked = true;
            _inputActionsController.DisablePlayer();
        }

        public void UnPause()
        {
            _states.IsBlocked = false;
            _inputActionsController.EnablePlayer();
        }
    }
}