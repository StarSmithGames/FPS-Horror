using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class CameraFOVSettings
    {
        [ field: Range( 1, 179 ) ]
        [ field: Tooltip( "Default field of view of your camera" ) ]
        [ field: SerializeField ] public float NormalFOV { get; private set; } = 60f;

        [ field: Range( 1, 179 ) ]
        [ field: Tooltip( "Running field of view of your camera" ) ]
        [ field: SerializeField ] public float RunningFOV { get; private set; } = 80f;

        [ field: Tooltip( "Fade Speed - Start Transition for the field of view" ) ]
        [ field: SerializeField ] public float FadeFOVAmount { get; private set; } = 5f;
    }
}