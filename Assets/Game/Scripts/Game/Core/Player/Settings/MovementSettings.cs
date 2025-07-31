using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class MovementSettings
    {
        [ field: Min( 0.01f ) ]
        [ field: Tooltip( "Max speed the player can reach. Velocity is clamped by this value." ) ]
        [ field: SerializeField ] public float MaxSpeedAllowed { get; private set; } = 20f;
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float RunSpeed { get; private set; } = 10f;
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float WalkSpeed { get; private set; } = 5f;
        [ field: Min( 0.01f ) ]
        [ field: SerializeField ] public float CrouchSpeed { get; private set; } = 3f;
        [ field: Tooltip( "Capacity to gain speed." ) ]
        [ field: SerializeField ] public float Acceleration { get; private set; } = 4500;
        [ field: Range( 0, 1f ) ]
        [ field: Tooltip( "Controls the snappiness of the character. The higher, the more responsive." ) ]
        [ field: SerializeField ] public float ControlsResponsiveness { get; private set; } = 0.175f;
    }
}