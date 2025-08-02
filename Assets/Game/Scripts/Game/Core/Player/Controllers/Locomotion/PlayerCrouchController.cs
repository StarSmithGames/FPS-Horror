using DG.Tweening;
using Game.Extensions;
using System;
using UnityEngine;

namespace Game.Core.Player
{
    public sealed class PlayerCrouchController
    {
        private Transform Root => _view.transform;
        
        private Vector3 _localScale;
        private Tween _crouchTween;

        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public PlayerCrouchController(
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
        
        public void StartCrouch()
        {
            _states.IsCrouching = true;
            if ( _config.SlidingSettings.AllowSliding )
            {
                if ( _view.Rigidbody.velocity.magnitude >= _config.MovementSettings.WalkSpeed && _states.IsGrounded && !_states.IsJumping )
                {
                    // Add the force on slide
                    _view.Rigidbody.AddForce( Root.rotation.Forward() * _config.SlidingSettings.SlideForce );
                    //staminaLoss
                    // if ( usesStamina ) stamina -= staminaLossOnSlide;
                }
            }

            _crouchTween?.Kill();
            _crouchTween = Root.DOScale( _config.CrouchSettings.CrouchScale, _config.CrouchSettings.CrouchTransitionSpeed * 1.5f );
        }

        public void StopCrouch()
        {
            _states.IsCrouching = false;
            
            _crouchTween?.Kill();
            _crouchTween = Root.DOScale( _localScale, _config.CrouchSettings.CrouchTransitionSpeed );
        }
        
        public bool IsSliding() => _states.IsCrouching && _view.Rigidbody.velocity.magnitude >= _config.MovementSettings.CrouchSpeed;
    }
}