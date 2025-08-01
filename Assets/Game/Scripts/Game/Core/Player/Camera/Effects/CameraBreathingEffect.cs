using UnityEngine;

namespace Game.Core.Player
{
    public sealed class CameraBreathingEffect : MonoBehaviour
    {
        [ Header( "BREATHING EFFECT" ) ]
        [ Tooltip( "Maximum Breathing Amount" ) ]
        [ SerializeField ] private float _breathingAmplitude = 0.2f;
        [ Tooltip( "Breathing Speed" ) ]
        [ SerializeField ]private float _breathingFrequency = 2f;
        [ Tooltip( "Enables Rotation for the Breathing Effect" ) ]
        [ SerializeField ] private bool _isBreathingRotation;

        private void Update()
        {
            float angle = Time.timeSinceLevelLoad * _breathingFrequency;
            float distance = _breathingAmplitude * Mathf.Sin( angle ) / 400f;
            float distanceRot = _breathingAmplitude * Mathf.Cos( angle ) / 100f;

            transform.position = new Vector3( transform.position.x, transform.position.y + Vector3.up.y * distance, transform.position.z );

            if ( _isBreathingRotation )
            {
                transform.Rotate( distanceRot, 0, 0, Space.Self );
            }
        }
    }
}