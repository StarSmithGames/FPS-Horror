using Game.Extensions;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerMovementController
    {
        public event Action OnLanded;
        
        private readonly float FrictionThreshold = 0.1f;
        
        private Vector3 _localScale;
        private Vector3 _moveDirection;
        private float _currentSpeed;
        
        public int jumpCount;
        private bool _hasJumped = false;
        
        private RaycastHit _slopeHit;
        
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
        }

        public void Dispose()
        {
            
        }

        public void Movement( Vector2 moveInput )
        {
            if ( _view.Rigidbody.velocity.magnitude > _config.MaxSpeedAllowed )
            {
                _view.Rigidbody.velocity = Vector3.ClampMagnitude( _view.Rigidbody.velocity, _config.MaxSpeedAllowed );
            }
            
            //Extra gravity
            _view.Rigidbody.AddForce( Vector3.down * Time.fixedDeltaTime * 10 );

            //Find actual velocity relative to where player is looking
            Vector2 relativeVelocity = FindVelRelativeToLook();

            //Counteract sliding and sloppy movement
            FrictionForce( moveInput.x, moveInput.y, relativeVelocity );
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
                _moveDirection = GetSlopeDirection( moveInput );
                _view.Rigidbody.useGravity = false;
                if ( _view.Rigidbody.velocity.y > 0 ) _view.Rigidbody.AddForce( Vector3.down * 150 );
            }
            else
            {
                _moveDirection = ( _view.transform.rotation.Forward() * moveInput.y + _view.transform.rotation.Right() * moveInput.x ).normalized;
            }

            // if(_moveDirection.magnitude > .1f) userEvents.OnMove.Invoke();

            _view.Rigidbody.AddForce( _moveDirection * movementMultipliers );
            
            void FrictionForce( float x, float y, Vector2 mag )
            {
                // Prevent from adding friction on an airborne body
                if ( !_states.IsGrounded ) return; //|| InputManager.jumping || hasJumped) return;

                float friction = IsSliding() ? _config.SlideFrictionForceAmount : _config.ControlsResponsiveness;

                // Counter movement ( Friction while moving )
                // Prevent from sliding not on purpose
                if ( Math.Abs( mag.x ) > FrictionThreshold && Math.Abs( x ) < 0.5f || ( mag.x < -FrictionThreshold && x > 0 ) || ( mag.x > FrictionThreshold && x < 0 ) )
                {
                    _view.Rigidbody.AddForce( _config.Acceleration * _view.transform.rotation.Right() * Time.deltaTime * -mag.x * friction );
                }

                if ( Math.Abs( mag.y ) > FrictionThreshold && Math.Abs( y ) < 0.05f || ( mag.y < -FrictionThreshold && y > 0 ) || ( mag.y > FrictionThreshold && y < 0 ) )
                {
                    _view.Rigidbody.AddForce( _config.Acceleration * _view.transform.rotation.Forward() * Time.deltaTime * -mag.y * friction );
                }
            }
        }

        /// <summary>
        /// Find the velocity relative to where the player is looking
        /// Useful for vectors calculations regarding movement and limiting movement
        /// </summary>
        /// <returns></returns>
        private Vector2 FindVelRelativeToLook()
        {
            // Convert velocity to local space relative to the player's look direction
            Vector3 localVel = Quaternion.Euler( 0, -_view.transform.rotation.Yaw(), 0 ) * _view.Rigidbody.velocity;
            return new Vector2( localVel.x, localVel.z );
        }

        /// <summary>
        /// Handle ground detection. Contributed by Chris Can. Thank you!
        /// </summary>
        public void CheckGrounded()
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
        private Vector3 GetSlopeDirection( Vector3 moveInput ) => Vector3.ProjectOnPlane( _view.transform.rotation.Forward() * moveInput.y + _view.transform.rotation.Right() * moveInput.x, _slopeHit.normal ).normalized;
        
        /// <summary>
        /// Determine wether this is determined as slope or not
        /// </summary>
        public bool IsOnSlope()
        {
            if ( Physics.Raycast( _view.transform.position, Vector3.down, out _slopeHit, _localScale.y + _config.GroundCheckDistance ) && _states.IsGrounded )
            {
                float angle = Vector3.Angle( Vector3.up, _slopeHit.normal );
                return angle < _config.MaxSlopeAngle && angle != 0;
            }

            return false;
        }
        
        public bool IsSliding() => _states.IsCrouching && _view.Rigidbody.velocity.magnitude >= _config.CrouchSpeed;

        /// <summary>
        /// Determine wether this is determined as floor or not
        /// </summary>
        public bool IsFloor( Vector3 v ) => Vector3.Angle( Vector3.up, v ) < _config.MaxSlopeAngle;
    }
}