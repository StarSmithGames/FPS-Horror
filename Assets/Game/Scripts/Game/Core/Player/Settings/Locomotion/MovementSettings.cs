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
        [ field: Tooltip( "Player deceleration from running speed to walking" ) ]
        [ field: SerializeField ] public float LoseSpeedDeceleration { get; private set; } = 500;
        [ field: Tooltip("Maximum slope angle that you can walk through.") ]
        [ field: Range(10, 80) ]
        [ field: SerializeField ] public float MaxSlopeAngle { get; private set; } = 35f;
        [ field: Range( 0, 1f ) ]
        [ field: Tooltip( "Controls the snappiness of the character. The higher, the more responsive." ) ]
        [ field: SerializeField ] public float ControlsResponsiveness { get; private set; } = 0.175f;
        [ field: Space ]
        [ field: SerializeField ] public bool AutoRun { get; private set; } = false;
        [ field: SerializeField ] public bool CanRunSideways { get; private set; } = true;
        [ field: SerializeField ] public bool CanRunBackwards { get; private set; } = false;
        [ field: SerializeField ] public bool CanRunWhileShooting { get; private set; } = false;
    }
}