using System;
using UnityEngine;
using Zenject;

namespace Game.Core.Player
{
    public sealed class CameraHeadBobEffect : MonoBehaviour
    {
        [ Header( "HEAD BOB EFFECT" ) ]
        [ Tooltip( "Maximum Head Bob" ) ]
        [ SerializeField ] private float _headBobAmplitude = 1f;
        [ Tooltip( "Speed to reach the Maximum Head Bob" ) ]
        [ SerializeField ] private float _headBobFrequency = 10f;

        private Vector3 _origPos;
        private Quaternion _origRot;
        
        private PlayerObject _view;
        private PlayerConfig _config;
        private PlayerStates _states;
        
        [ Inject ]
        private void Construct(
            PlayerObject view,
            PlayerConfig config,
            PlayerStates states
            )
        {
            _view = view ?? throw new ArgumentNullException( nameof(view) );
            _config = config ?? throw new ArgumentNullException( nameof(config) );
            _states = states ?? throw new ArgumentNullException( nameof(states) );
            
            _origPos = transform.localPosition;
            _origRot = transform.localRotation;
        }
        
        private void Update()
        {
            if ( _view.Rigidbody.velocity.magnitude < _config.MovementSettings.WalkSpeed || _states.IsJumping )
            {
                transform.localPosition = Vector3.Lerp( transform.localPosition, _origPos, Time.deltaTime * 2f );
                transform.localRotation = Quaternion.Lerp( transform.localRotation, _origRot, Time.deltaTime * 2f );
                return;
            }

            float angle = Time.timeSinceLevelLoad * _headBobFrequency;
            float distanceY = _headBobAmplitude * Mathf.Sin( angle ) / 400f;
            float distanceX = _headBobAmplitude * Mathf.Cos( angle ) / 100f;

            transform.position = new Vector3( transform.position.x, transform.position.y + Vector3.up.y * distanceY, transform.position.z );
            transform.Rotate( distanceX, 0, 0, Space.Self );
        }
    }
}