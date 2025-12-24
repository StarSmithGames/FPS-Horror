using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Entity
{
    [ System.Serializable ]
    public sealed class OpenableSettings
    {
        [ field: SerializeField ] public bool IsLocked { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public bool IsFlipped { get; private set; }
        [ field: SerializeField ] public float DoorOpenAngle { get; private set; } = 90f;
        [ field: SerializeField ] public float DoorDuration { get; private set; } = 1f;
        [ field: SerializeField ] public Axis DoorAxis { get; private set; } = Axis.Y;
        [ field: Space ]
        [ field: SerializeField ] public float HandleAngle { get; private set; } = 30f;
        [ field: SerializeField ] public float HandleDuration { get; private set; } = 0.33f;
        [ field: SerializeField ] public Axis HandleAxis { get; private set; } = Axis.Z;
        [ field: Space ]
        [ field: SerializeField ] public List< AudioClip > OpenSounds { get; private set; } = new();
        [ field: SerializeField ] public List< AudioClip > CloseSounds { get; private set; } = new();
    }
}