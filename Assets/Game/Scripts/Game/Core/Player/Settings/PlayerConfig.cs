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
        [ field: SerializeField ] public GroundSettings GroundSettings { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public CameraFOVSettings CameraFOVSettings { get; private set; }
        [ field: SerializeField ] public CameraVisionSettings CameraVisionSettings { get; private set; } 
    }
}