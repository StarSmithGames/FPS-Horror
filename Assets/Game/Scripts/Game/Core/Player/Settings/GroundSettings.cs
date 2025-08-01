using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class GroundSettings
    {
        [ field: Tooltip( "Every object with this layer will be detected as ground, so you will be able to walk on it" ) ]
        [ field: SerializeField ] public LayerMask GroundLayer { get; private set; }

        [ field: Tooltip( "Distance from the bottom of the player to detect ground" ) ]
        [ field: Min( 0 ) ]
        [ field: SerializeField ] public float GroundCheckDistance { get; private set; } = 1.2f;
    }
}