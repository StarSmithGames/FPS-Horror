using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class LookSettings
    {
        [ field: Tooltip( "Maximum Vertical Angle for the camera" ) ]
        [ field: Range( 20, 89.7f ) ]
        [ field: SerializeField ] public float MaxCameraAngle { get; private set; } = 89.7f;

        [Tooltip("Horizontal sensitivity (X Axis)")] public float sensitivityX = 4;

        [Tooltip("Vertical sensitivity (Y Axis)")] public float sensitivityY = 4;

        [ field: SerializeField ] public bool InvertYSensitivity { get; private set; } = false;

        [Tooltip("Horizontal sensitivity (X Axis) using controllers"), SerializeField] private float controllerSensitivityX = 35;

        [Tooltip("Vertical sensitivity (Y Axis) using controllers"), SerializeField] private float controllerSensitivityY = 35;

        [ field: SerializeField ] public bool InvertYControllerSensitivity { get; private set; } = false;

        [Range(.1f, 1f), Tooltip("Sensitivity will be multiplied by this value when aiming"), SerializeField] private float aimingSensitivityMultiplier = .4f;

        [Tooltip("Default field of view of your camera"), Range(1, 179), SerializeField] private float normalFOV;

        [Tooltip("Running field of view of your camera"), Range(1, 179), SerializeField] private float runningFOV;

        [Tooltip("Wallrunning field of view of your camera"), Range(1, 179)] public float wallrunningFOV;

        [Tooltip("Amount of field of view that will be added to your camera when dashing."), Range(-179, 179)] public float fovToAddOnDash;

        [Tooltip("Fade Speed - Start Transition for the field of view")] public float fadeFOVAmount;

        [SerializeField, Range(-30, 30), Tooltip("Rotation of the camera when sliding. The rotation direction is defined by the sign of the value.")]
        private float slidingCameraTiltAmount;

        [ field: Header( "Camera" ) ]
        [ field: Tooltip( "Speed of the tilt camera movement. This is essentially used for wall running" ) ]
        [ field: SerializeField ] public float CameraTiltTransitionSpeed { get; private set; } = 5f;
    }
}