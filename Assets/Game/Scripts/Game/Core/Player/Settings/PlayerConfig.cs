using UnityEngine;

namespace Game.Core.Player
{
    [ CreateAssetMenu( fileName = "PlayerConfig", menuName = "Game/PlayerConfig" ) ]
    public sealed class PlayerConfig : ScriptableObject
    {
        [ field: SerializeField ] public LookSettings LookSettings { get; private set; }
        [ field: SerializeField ] public MovementSettings MovementSettings { get; private set; }
        [ field: SerializeField ] public JumpSettings JumpSettings { get; private set; }
        
        #region Ground
        [ field: Header( "Ground" ) ]
        [ field: Tooltip( "Every object with this layer will be detected as ground, so you will be able to walk on it" ) ]
        [ field: SerializeField ] public LayerMask GroundLayer { get; private set; }

        [ field: Tooltip( "Distance from the bottom of the player to detect ground" ) ]
        [ field: Min( 0 ) ]
        [ field: SerializeField ] public float GroundCheckDistance { get; private set; } = 1.2f;
        #endregion

        #region Sliding
        [ field: Header( "Sliding" ) ]
        [ field: Tooltip( "When true, player will be allowed to slide." ) ]
        [ field: SerializeField ] public bool AllowSliding { get; private set; }
        [Tooltip("Force added on sliding."), SerializeField]
        private float slideForce = 400;
        [ field: Tooltip( "If true, the player will be able to move while sliding." ) ]
        [ field: SerializeField ] public bool AllowMoveWhileSliding { get; private set; }
        [ field: Range( 0, 1f ) ]
        [ field: Tooltip( "Force applied to counter movement when sliding" ) ]
        [ field: SerializeField ] public float SlideFrictionForceAmount { get; private set; } = 0.05f;
        #endregion
        
        [ field: Tooltip("Maximum slope angle that you can walk through.") ]
        [ field: Range(10, 80) ]
        [ field: SerializeField ] public float MaxSlopeAngle { get; private set; } = 35f;

        [ field: SerializeField ] public CameraFOVSettings CameraFOVSettings { get; private set; }
    }
}