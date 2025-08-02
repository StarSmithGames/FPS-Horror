using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class CameraVisionSettings
    {
        [ field: SerializeField ] public LayerMask InteractLayers { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public float MaxRayDistance { get; private set; } = 5f;
        [ field: SerializeField ] public float RayDistance { get; private set; } = 5f;
        [ field: SerializeField ] public float SphereRadius { get; private set; } = 1f;
    }
}