using Game.Extensions;
using PuzzlescapeGames.Extensions;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerJumpController
    {
        public event Action OnJumped;
        
        private Transform Root => _view.transform;
        private Transform Head => _view.FirstPersonCamera.transform;
        
        private bool enoughStaminaToJump = true;
        private bool readyToJump = true;
        
        private int _jumpCount;

        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public PlayerJumpController(
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
            _jumpCount = _config.JumpSettings.MaxJumps;
        }
        
        public void Jump( Vector2 moveInput )
        {
            if (!_config.JumpSettings.AllowJump || _jumpCount <= 0) return;

            OnJumped?.Invoke();

            _jumpCount--;
            readyToJump = false;
            _states.IsJumping = true;

            Vector3 velocity = _view.Rigidbody.velocity;
            velocity.y = 0f;
            _view.Rigidbody.velocity = velocity;

            // if (doubleJumpResetsFallDamage) fallHeightProvider?.SetFallHeight(transform.position.y);

            _view.Rigidbody.AddForce( Vector3.up * _config.JumpSettings.JumpForce, ForceMode.Impulse );
            HandleDirectionalJumping();

            //staminaLoss
            // if (usesStamina) stamina -= staminaLossOnJump;

            // SoundManager.Instance.PlaySound(sounds.jumpSFX, 0, 0, false);
            AsyncDelaying.DelayCall( _config.JumpSettings.JumpCooldown, ResetJump );
            
            void HandleDirectionalJumping()
            {
                if ( !_states.IsGrounded && _config.JumpSettings.JumpType != JumpType.Common && _config.JumpSettings.MaxJumps > 1 ) // && !wallOpposite)
                {
                    if ( Vector3.Dot( _view.Rigidbody.velocity, new Vector3( moveInput.x, 0, moveInput.y ) ) > .5f )
                        _view.Rigidbody.velocity /= 2f;

                    if ( _config.JumpSettings.JumpType == JumpType.InputBased )//Input based method for directional jumping
                    {
                        _view.Rigidbody.AddForce( Root.rotation.Right() * moveInput.x * _config.JumpSettings.DirectionalJumpForce, ForceMode.Impulse );
                        _view.Rigidbody.AddForce( Root.rotation.Forward() * moveInput.y * _config.JumpSettings.DirectionalJumpForce, ForceMode.Impulse );
                    }

                    if ( _config.JumpSettings.JumpType == JumpType.ForwardMovement )//Forward Movement method for directional jumping, dependant on orientation
                    {
                        _view.Rigidbody.AddForce( Root.rotation.Forward() * Mathf.Abs( moveInput.y ) * _config.JumpSettings.DirectionalJumpForce, ForceMode.VelocityChange );
                    }
                }
            }
        }

        public void OnLanded()
        {
            Debug.LogError( "Landed" );
            _jumpCount = _config.JumpSettings.MaxJumps;
            _states.IsJumping = false;
        }

        private void ResetJump() => readyToJump = true;
    }
}