using UnityEngine;

namespace Game.Core.World.InspectionSystem
{
    [ System.Serializable ]
    public sealed class InspectionSettings
    {
        [ field: SerializeField ] public bool IsRotatable { get; private set; } = true;
        [ field: SerializeField ] public bool IsInverseY { get; private set; }
        [ field: SerializeField ] public Vector3 PositionOffset { get; private set; }
        [ field: SerializeField ] public Vector3 RotationOffset { get; private set; }
        
        [field: SerializeField] public InspectionAxis FaceAxis { get; private set; } = InspectionAxis.Up;
        [field: SerializeField] public InspectionAxis BottomAxis { get; private set; } = InspectionAxis.Forward;
    }
}