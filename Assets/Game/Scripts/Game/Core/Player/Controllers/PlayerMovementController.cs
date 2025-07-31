using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerMovementController
    {
        public event Action OnLanded;
        
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

        /// <summary>
        /// Handle all the basics related to the movement of the player.
        /// </summary>
        public void Movement( Vector2 moveInput )
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
                _moveDirection = GetSlopeDirection( moveInput );
                _view.Rigidbody.useGravity = false;
                if ( _view.Rigidbody.velocity.y > 0 ) _view.Rigidbody.AddForce( Vector3.down * 150 );
            }
            else
            {
                _moveDirection = ( _states.Forward * moveInput.y + _states.Right * moveInput.x ).normalized;
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
            Vector3 localVel = Quaternion.Euler( 0, -_states.Yaw, 0 ) * _view.Rigidbody.velocity;
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
        private Vector3 GetSlopeDirection( Vector3 moveInput ) => Vector3.ProjectOnPlane( _states.Forward * moveInput.y + _states.Right * moveInput.x, _slopeHit.normal ).normalized;
        
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