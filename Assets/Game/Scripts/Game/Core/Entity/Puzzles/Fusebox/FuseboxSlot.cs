using UnityEngine;

namespace Game.Core.Entity
{
    [ System.Serializable ]
    public sealed class FuseboxSlot
    {
        [ field: SerializeField ] public MeshRenderer Light { get; private set; }
        [ field: SerializeField ] public Transform Nest { get; private set; }
        [ field: SerializeField ] public bool IsInserted { get; set; }
    }
}