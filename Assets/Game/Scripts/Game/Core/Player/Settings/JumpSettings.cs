using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class JumpSettings
    {
        [ field: SerializeField ] public bool AllowJump { get; private set; }

        [ field: Tooltip( "Amount of jumps you can do without touching the ground" ) ]
        [ field: Min( 1 ) ]
        [ field: SerializeField ] public int MaxJumps { get; private set; } = 1;

        [ field: Tooltip( "Method to apply on jumping when the player is not grounded, related to the directional jump" ) ]
        [ field: SerializeField ] public JumpType JumpType { get; private set; } = JumpType.Common;
        [ field: Tooltip("Force applied on an object in the direction of the directional jump") ]
        [ field: SerializeField ] public float DirectionalJumpForce { get; private set; }

        [ field: Tooltip( "The higher this value is, the higher you will get to jump." ) ]
        [ field: SerializeField ] public float JumpForce { get; private set; } = 20f;

        [ field: Tooltip( "How much control you own while you are not grounded. Being 0 = no control of it, 1 = Full control." ) ]
        [ field: Range( 0, 1 ) ]
        [ field: SerializeField ] public float ControlAirborne { get; private set; } = .5f;

        [ field: Min( .25f ) ]
        [ field: Tooltip( "Interval between jumping" ) ]
        [ field: SerializeField ] public float JumpCooldown { get; private set; } = .25f;
    }
}