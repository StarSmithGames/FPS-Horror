using Cysharp.Threading.Tasks;
using Game.Managers.InputManager;
using System;
using System.Threading;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerMovementController
    {
        public event Action OnLanded;

        public Vector3 Forward => _rotation * Vector3.forward;
        public Vector3 Right => _rotation * Vector3.right;
        public Vector3 Up => _rotation * Vector3.up;
        private Vector3 _position;
        
        public float Yaw => _rotation.eulerAngles.y;
        private Quaternion _rotation;
        
        private Vector3 _localScale;
        private Vector2 _moveInput;
        private Vector3 _moveDirection;
        private float _currentSpeed;
        
        public int jumpCount;
        private bool _hasJumped = false;
        
        private RaycastHit _slopeHit;
        
        private CancellationTokenSource _cancellationTokenSource;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public PlayerMovementController(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
        }

        public void Initialize()
        {
            _localScale = _view.transform.localScale;
            
            _cancellationTokenSource = new();
            Tick( _cancellationTokenSource.Token ).Forget();
            FixedTick( _cancellationTokenSource.Token ).Forget();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private async UniTask Tick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                _moveInput = InputManager.Inputs.Player.Movement.ReadValue< Vector2 >();

                _position = _view.transform.position;
                // _rotation = Quaternion.Euler(0, 90, 0);
                
                CheckGrounded();

                // if (canWallBounce) CheckOppositeWall();

                // if (InputManager.jumping && wallOpposite && canWallBounce && playerControl.IsControllable && CheckHeight()) WallBounce();

                // HandleStairs( _moveDirection );
                
                await UniTask.Yield( PlayerLoopTiming.Update );
            }
        }

        private async UniTask FixedTick( CancellationToken cancellationToken = default )
        {
            while ( !cancellationToken.IsCancellationRequested )
            {
                bool isPlayerOnSlope = IsOnSlope();
                // Added Gravity
                // Gravity is added only if we are not on a slope or climbing to prevent unvoluntary sliding
                if ( !isPlayerOnSlope && !_states.IsClimbing ) _view.Rigidbody.AddForce( Vector3.down * 30.19f, ForceMode.Acceleration );

                if ( _view.Rigidbody.velocity.magnitude > _config.MaxSpeedAllowed )
                {
                    _view.Rigidbody.velocity = Vector3.ClampMagnitude( _view.Rigidbody.velocity, _config.MaxSpeedAllowed );
                }

                Movement();
                
                await UniTask.Yield( PlayerLoopTiming.FixedUpdate );
            }
        }

        /// <summary>
        /// Handle all the basics related to the movement of the player.
        /// </summary>
        public void Movement()
        {
            //Extra gravity
            _view.Rigidbody.AddForce( Vector3.down * Time.fixedDeltaTime * 10 );

            //Find actual velocity relative to where player is looking
            Vector2 relativeVelocity = FindVelRelativeToLook();
            float xRelativeVelocity = relativeVelocity.x, yRelativeVelocity = relativeVelocity.y;

            //Counteract sliding and sloppy movement
            // FrictionForce(InputManager.x, InputManager.y, relativeVelocity);
            //If speed is larger than maxspeed, clamp the velocity so you don't go over max speed
            // ClampToCurrentSpeed();

            if ( _view.Rigidbody.velocity.sqrMagnitude < .02f ) _view.Rigidbody.velocity = Vector3.zero;

            // if (!playerControl.IsControllable)
            // {
            //     if (_states.IsGrounded) _view.Rigidbody.velocity = Vector3.zero;
            //     return;
            // }

            if ( IsSliding() && !_config.AllowMoveWhileSliding ) return;

            float airborneMultiplier = !_states.IsGrounded ? _config.ControlAirborne : 1;
            float movementMultipliers = _config.Acceleration * Time.deltaTime * airborneMultiplier;

            if ( IsOnSlope() )
            {
                _moveDirection = GetSlopeDirection();
                _view.Rigidbody.useGravity = false;
                if ( _view.Rigidbody.velocity.y > 0 ) _view.Rigidbody.AddForce( Vector3.down * 150 );
            }
            else
            {
                _moveDirection = ( Forward * _moveInput.y + Right * _moveInput.x ).normalized;
            }

            // if(_moveDirection.magnitude > .1f) userEvents.OnMove.Invoke();

            _view.Rigidbody.AddForce( _moveDirection * movementMultipliers );
        }

        /// <summary>
        /// Find the velocity relative to where the player is looking
        /// Useful for vectors calculations regarding movement and limiting movement
        /// </summary>
        /// <returns></returns>
        private Vector2 FindVelRelativeToLook()
        {
            // Convert velocity to local space relative to the player's look direction
            Vector3 localVel = Quaternion.Euler( 0, -Yaw, 0 ) * _view.Rigidbody.velocity;
            return new Vector2( localVel.x, localVel.z );
        }

        /// <summary>
        /// Handle ground detection. Contributed by Chris Can. Thank you!
        /// </summary>
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
                    jumpCount = _config.MaxJumps;
                    _hasJumped = false;

                    // SoundManager.Instance.PlaySound(sounds.landSFX, 0, 0, false);
                    OnLanded?.Invoke();
                }

                _states.IsGrounded = true;
            }
            else
            {
                if ( _states.IsGrounded )
                {
                    _states.IsGrounded = false;
                    // coyoteTimer = coyoteJumpTime;
                }
            }
        }

        /// <summary>
        /// Get the direction of movement in a slope
        /// </summary>
        /// <returns></returns>
        private Vector3 GetSlopeDirection() => Vector3.ProjectOnPlane( Forward * _moveInput.y + Right * _moveInput.x, _slopeHit.normal ).normalized;
        
        /// <summary>
        /// Determine wether this is determined as slope or not
        /// </summary>
        private bool IsOnSlope()
        {
            if ( Physics.Raycast( _view.transform.position, Vector3.down, out _slopeHit, _localScale.y + _config.GroundCheckDistance ) && _states.IsGrounded )
            {
                float angle = Vector3.Angle( Vector3.up, _slopeHit.normal );
                return angle < _config.MaxSlopeAngle && angle != 0;
            }

            return false;
        }
        
        private bool IsSliding() => _states.IsCrouching && _view.Rigidbody.velocity.magnitude >= _config.CrouchSpeed;

        /// <summary>
        /// Determine wether this is determined as floor or not
        /// </summary>
        private bool IsFloor( Vector3 v ) => Vector3.Angle( Vector3.up, v ) < _config.MaxSlopeAngle;
    }
}