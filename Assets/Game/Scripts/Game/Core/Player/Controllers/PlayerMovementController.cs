using Game.Extensions;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerMovementController
    {
        private readonly float FrictionThreshold = 0.1f;

        private Transform Root => _view.transform;
        private Transform Head => _view.FirstPersonCamera.transform;

        private Vector3 _localScale;
        private Vector3 _moveDirection;
        private float _currentSpeed;
        
        private RaycastHit _slopeHit;
        
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        private readonly CameraFOVController _cameraFOVController;
        
        public PlayerMovementController(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states,
            CameraFOVController cameraFOVController
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            _cameraFOVController = cameraFOVController ?? throw new ArgumentNullException( nameof(cameraFOVController) );
        }

        public void Initialize()
        {
            _localScale = _view.transform.localScale;
        }

        public void Movement( bool isSliding, Vector2 moveInput )
        {
            // Added Gravity
            // Gravity is added only if we are not on a slope or climbing to prevent unvoluntary sliding
            if ( !IsOnSlope() && !_states.IsClimbing )
            {
                _view.Rigidbody.AddForce( Vector3.down * 30.19f, ForceMode.Acceleration );
            }
            
            if ( _view.Rigidbody.velocity.magnitude > _config.MovementSettings.MaxSpeedAllowed )
            {
                _view.Rigidbody.velocity = Vector3.ClampMagnitude( _view.Rigidbody.velocity, _config.MovementSettings.MaxSpeedAllowed );
            }
            
            //Extra gravity
            _view.Rigidbody.AddForce( Vector3.down * Time.fixedDeltaTime * 10 );

            //Find actual velocity relative to where player is looking
            Vector2 relativeVelocity = FindVelRelativeToLook();

            //Counteract sliding and sloppy movement
            FrictionForce( moveInput.x, moveInput.y, relativeVelocity );
            //If speed is larger than maxspeed, clamp the velocity so you don't go over max speed
            ClampToCurrentSpeed();

            if ( _view.Rigidbody.velocity.sqrMagnitude < .02f ) _view.Rigidbody.velocity = Vector3.zero;

            if ( _states.IsBlocked )
            {
                if ( _states.IsGrounded ) _view.Rigidbody.velocity = Vector3.zero;
                return;
            }
            if ( isSliding && !_config.SlidingSettings.AllowMoveWhileSliding ) return;

            float airborneMultiplier = !_states.IsGrounded ? _config.JumpSettings.ControlAirborne : 1;
            float movementMultipliers = _config.MovementSettings.Acceleration * Time.deltaTime * airborneMultiplier;

            if ( IsOnSlope() )
            {
                _moveDirection = GetSlopeDirection( moveInput );
                _view.Rigidbody.useGravity = false;
                if ( _view.Rigidbody.velocity.y > 0 ) _view.Rigidbody.AddForce( Vector3.down * 150 );
            }
            else
            {
                _moveDirection = ( Root.rotation.Forward() * moveInput.y + Root.rotation.Right() * moveInput.x ).normalized;
            }

            _view.Rigidbody.AddForce( _moveDirection * movementMultipliers );
            
            void FrictionForce( float x, float y, Vector2 mag )
            {
                // Prevent from adding friction on an airborne body
                if ( !_states.IsGrounded ) return; //|| InputManager.jumping || hasJumped) return;

                float friction = isSliding ? _config.SlidingSettings.SlideFrictionForceAmount : _config.MovementSettings.ControlsResponsiveness;

                // Counter movement ( Friction while moving )
                // Prevent from sliding not on purpose
                if ( Math.Abs( mag.x ) > FrictionThreshold && Math.Abs( x ) < 0.5f || ( mag.x < -FrictionThreshold && x > 0 ) || ( mag.x > FrictionThreshold && x < 0 ) )
                {
                    _view.Rigidbody.AddForce( _config.MovementSettings.Acceleration * Root.rotation.Right() * Time.deltaTime * -mag.x * friction );
                }

                if ( Math.Abs( mag.y ) > FrictionThreshold && Math.Abs( y ) < 0.05f || ( mag.y < -FrictionThreshold && y > 0 ) || ( mag.y > FrictionThreshold && y < 0 ) )
                {
                    _view.Rigidbody.AddForce( _config.MovementSettings.Acceleration * Root.rotation.Forward() * Time.deltaTime * -mag.y * friction );
                }
            }
            
            Vector2 FindVelRelativeToLook()
            {
                /// Find the velocity relative to where the player is looking
                /// Useful for vectors calculations regarding movement and limiting movement
                // Convert velocity to local space relative to the player's look direction
                Vector3 localVel = Quaternion.Euler( 0, -Root.rotation.Yaw(), 0 ) * _view.Rigidbody.velocity;
                return new Vector2( localVel.x, localVel.z );
            }

            void ClampToCurrentSpeed()
            {
                Vector3 horizontalVelocity = new Vector3( _view.Rigidbody.velocity.x, 0, _view.Rigidbody.velocity.z );
                float currentWeightedSpeed = _currentSpeed;// * playerMultipliers.playerWeightMultiplier;
                if ( horizontalVelocity.magnitude > currentWeightedSpeed )
                {
                    horizontalVelocity = horizontalVelocity.normalized * currentWeightedSpeed;
                    _view.Rigidbody.velocity = new Vector3( horizontalVelocity.x, _view.Rigidbody.velocity.y, horizontalVelocity.z );
                }
            }
        }
        
        public void HandleVelocities( bool isSprinting, bool isShooting, Vector2 moveInput )
        {
            // if (weaponReference.Weapon != null && weaponController.IsAiming && weaponReference.Weapon.setMovementSpeedWhileAiming)
            // {
            //     currentSpeed = weaponReference.Weapon.movementSpeedWhileAiming;
            //     return;
            // }

            bool enoughStaminaToRun = true;
            
            if ( ( isSprinting || _config.MovementSettings.AutoRun ) && enoughStaminaToRun )
            {
                bool movingBackward = moveInput.y < 0;
                bool shootingWhileDisallowed = isShooting && !_config.MovementSettings.CanRunWhileShooting;;//&& weaponReference.Weapon != null
                bool onlyStrafing = moveInput.x != 0 && moveInput.y == 0 && !_config.MovementSettings.CanRunSideways;

                bool canRun = !( ( !_config.MovementSettings.CanRunBackwards && movingBackward ) || shootingWhileDisallowed || onlyStrafing );

                if ( canRun )
                {
                    bool movingForward = Vector3.Dot( Root.rotation.Forward(), _view.Rigidbody.velocity ) > 0;
                    bool forwardAllowed = _config.MovementSettings.CanRunBackwards || movingForward;
                    bool sidewaysAllowed = _config.MovementSettings.CanRunSideways || ( moveInput.x == 0 && moveInput.y != 0 );
                    bool shootingAllowed = _config.MovementSettings.CanRunWhileShooting || !isShooting;

                    if ( forwardAllowed && sidewaysAllowed && shootingAllowed )
                    {

                        if ( _currentSpeed != _config.MovementSettings.RunSpeed && _view.Rigidbody.velocity.magnitude > .1f )// && !wallRunning
                        {
                            _cameraFOVController.SetFOV( _config.CameraFOVSettings.RunningFOV );
                        }
                        _currentSpeed = _config.MovementSettings.RunSpeed;
                        return;
                    }
                }

                _currentSpeed = Mathf.MoveTowards( _currentSpeed, _config.MovementSettings.WalkSpeed, Time.deltaTime * _config.MovementSettings.LoseSpeedDeceleration );
            }
            else
            {
                if ( _currentSpeed != _config.MovementSettings.WalkSpeed ) //&& !wallRunning
                {
                    _cameraFOVController.SetFOV( _config.CameraFOVSettings.NormalFOV );
                }
                _currentSpeed = _config.MovementSettings.WalkSpeed;
            }

            if ( _view.Rigidbody.velocity.sqrMagnitude < 0.0001f )
            {
                if ( _currentSpeed != _config.MovementSettings.WalkSpeed )
                {
                    _cameraFOVController.SetFOV( _config.CameraFOVSettings.NormalFOV );
                }
                _currentSpeed = _config.MovementSettings.WalkSpeed;
            }
        }

        /// <summary>
        /// Get the direction of movement in a slope
        /// </summary>
        /// <returns></returns>
        private Vector3 GetSlopeDirection( Vector3 moveInput ) => Vector3.ProjectOnPlane( Root.rotation.Forward() * moveInput.y + Root.rotation.Right() * moveInput.x, _slopeHit.normal ).normalized;
        
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
        
        public bool IsFloor( Vector3 v ) => Vector3.Angle( Vector3.up, v ) < _config.MaxSlopeAngle;
    }
}