using UnityEngine;

namespace Game.Core.Entity
{
    [ System.Serializable ] 
    public sealed class PullableSettings
    {
        [ field: SerializeField ] public bool IsLocked { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public float OpenScalar { get; private set; } = 0.3f;
        [ field: SerializeField ] public float Duration { get; private set; } = 0.33f;
    }
}