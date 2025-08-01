using UnityEngine;

namespace Game.Core.Player
{
    [ CreateAssetMenu( fileName = "PlayerConfig", menuName = "Game/PlayerConfig" ) ]
    public sealed class PlayerConfig : ScriptableObject
    {
        [ field: SerializeField ] public LookSettings LookSettings { get; private set; }
        [ field: SerializeField ] public MovementSettings MovementSettings { get; private set; }
        [ field: SerializeField ] public JumpSettings JumpSettings { get; private set; }
        [ field: SerializeField ] public CrouchSettings CrouchSettings { get; private set; }
        [ field: SerializeField ] public SlidingSettings SlidingSettings { get; private set; }
        [ field: Space ]
        #region Ground
        [ field: Header( "Ground" ) ]
        [ field: Tooltip( "Every object with this layer will be detected as ground, so you will be able to walk on it" ) ]
        [ field: SerializeField ] public LayerMask GroundLayer { get; private set; }

        [ field: Tooltip( "Distance from the bottom of the player to detect ground" ) ]
        [ field: Min( 0 ) ]
        [ field: SerializeField ] public float GroundCheckDistance { get; private set; } = 1.2f;
        #endregion
        [ field: Space ]
        [ field: SerializeField ] public CameraFOVSettings CameraFOVSettings { get; private set; }
        [ field: SerializeField ] public CameraVisionSettings CameraVisionSettings { get; private set; } 
    }
}