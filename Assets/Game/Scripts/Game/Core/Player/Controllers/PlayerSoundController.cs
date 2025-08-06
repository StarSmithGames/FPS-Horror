using Game.Managers.AudioManager;
using PuzzlescapeGames.Extensions;
using System;
using UnityEngine;
using AudioSettings = Game.Managers.AudioManager.AudioSettings;

namespace Game.Core.Player
{
    public sealed class PlayerSoundController
    {
        private FootStepSoundsSettings _settings;
        private float stepTimer;
        
        private readonly AudioManager _audioManager;
        private readonly AudioSettings _audioSettings;
        private readonly PlayerObject _view;
        private readonly PlayerConfig _config;
        private readonly PlayerStates _states;
        
        public PlayerSoundController(
            AudioManager audioManager,
            AudioSettings audioSettings,
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states
            )
        {
            _audioManager = audioManager ?? throw new ArgumentNullException( nameof(audioManager) );
            _audioSettings = audioSettings ?? throw new ArgumentNullException( nameof(audioSettings) );
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );

            _settings = _config.SoundsSettings.FootStepSoundsSettings;
        }

        public void FootSteps()
        {
            if ( !_states.IsGrounded || _view.Rigidbody.velocity.sqrMagnitude <= .1f )
            {
                stepTimer = 1 - _settings.FootstepSpeed;
                return;
            }

            // Wait for the next time to play a sound
            stepTimer -= Time.deltaTime * _view.Rigidbody.velocity.magnitude / 15;

            // Play the sound and reset
            if ( stepTimer <= 0 )
            {
                stepTimer = 1 - _settings.FootstepSpeed;
                var pitch = UnityEngine.Random.Range( 0.7f, 1.3f ); // Add variety to avoid boring and repetitive sounds while walking

                if ( Physics.Raycast( _view.CameraFPS.transform.position, Vector3.down, out RaycastHit hit, 2.5f, _config.GroundSettings.GroundLayer ) )
                {
                    switch ( hit.transform.gameObject.layer )
                    {
                        // case int layer when layer == groundLayer: // Ground
                        //     _audioManager.PlaySound( footsteps.defaultStep[ i ], footstepVolume );
                        //     break;
                        // case int layer when layer == grassLayer: // Grass
                        //     _audioManager.PlaySound( footsteps.grassStep[ i ], footstepVolume );
                        //     break;
                        // case int layer when layer == metalLayer: // Metal
                        //     _audioManager.PlaySound( footsteps.metalStep[ i ], footstepVolume );
                        //     break;
                        // case int layer when layer == mudLayer: // Mud
                        //     _audioManager.PlaySound( footsteps.mudStep[ i ], footstepVolume );
                        //     break;
                        // case int layer when layer == woodLayer: // Wood
                        //     _audioManager.PlaySound( footsteps.woodStep[ i ], footstepVolume );
                        //     break;
                        default: // Default
                        {
                            float volume = _settings.FootStepsCommon.VolumeCommon;
                            if ( _states.IsSprinting )
                            {
                                volume = _settings.FootStepsCommon.VolumeSprinting;
                            }
                            if ( _states.IsCrouching )
                            {
                                volume = _settings.FootStepsCommon.VolumeCrouching;
                            }

                            _audioManager.PlaySound( _settings.FootStepsCommon.Sounds.RandomItem(), volume: volume, pitch: pitch );
                            break;
                        }
                    }
                }
            }
        }
    }
}