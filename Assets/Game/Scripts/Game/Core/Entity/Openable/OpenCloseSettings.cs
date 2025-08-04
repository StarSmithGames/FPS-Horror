using UnityEngine;

namespace Game.Core.Entity
{
    [ System.Serializable ]
    public sealed class OpenCloseSettings
    {
        [ field: SerializeField ] public bool IsLocked { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public float DoorOpenAngle { get; private set; } = 90f;
        [ field: SerializeField ] public float DoorDuration { get; private set; } = 1f;

        [ field: SerializeField ] public float HandleAngle { get; private set; } = 30f;
        [ field: SerializeField ] public float HandleDuration { get; private set; } = 0.33f;
    }
}