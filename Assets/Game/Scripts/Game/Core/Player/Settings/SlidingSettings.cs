using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class SlidingSettings
    {
        [ field: Tooltip( "When true, player will be allowed to slide." ) ]
        [ field: SerializeField ] public bool AllowSliding { get; private set; }

        [ field: Tooltip( "Force added on sliding." ) ]
        [ field: SerializeField ] public float SlideForce { get; private set; } = 300;

        [ field: Tooltip( "If true, the player will be able to move while sliding." ) ]
        [ field: SerializeField ] public bool AllowMoveWhileSliding { get; private set; } = true;

        [ field: Range( 0, 1f ) ]
        [ field: Tooltip( "Force applied to counter movement when sliding" ) ]
        [ field: SerializeField ] public float SlideFrictionForceAmount { get; private set; } = 0.05f;
    }
}