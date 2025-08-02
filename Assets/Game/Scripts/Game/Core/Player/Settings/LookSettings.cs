using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class LookSettings
    {
        [ field: Tooltip( "Maximum Vertical Angle for the camera" ) ]
        [ field: Range( 20, 89.7f ) ]
        [ field: SerializeField ] public float MaxCameraAngle { get; private set; } = 89.7f;
        [ field: SerializeField ] public bool InvertYSensitivity { get; private set; } = false;
        [ field: SerializeField ] public bool InvertYControllerSensitivity { get; private set; } = false;

        // [Range(.1f, 1f), Tooltip("Sensitivity will be multiplied by this value when aiming"), SerializeField] private float aimingSensitivityMultiplier = .4f;

        [ field: Header( "Camera" ) ]
        [ field: Tooltip( "Speed of the tilt camera movement. This is essentially used for wall running" ) ]
        [ field: SerializeField ] public float CameraTiltTransitionSpeed { get; private set; } = 5f;
        [ field: Range( -30, 30 ) ]
        [ field: Tooltip( "Rotation of the camera when sliding. The rotation direction is defined by the sign of the value." ) ]
        [ field: SerializeField ] public float SlidingCameraTiltAmount { get; private set; } = 5f;
    }
}