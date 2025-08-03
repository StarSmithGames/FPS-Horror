using UnityEngine;

namespace Game.Core.Entity
{
    [ System.Serializable ]
    public sealed class OpenCloseSettings
    {
        [ field: SerializeField ] public bool IsLocked { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public float OpenSpeed { get; private set; } = 1f;
        [ field: SerializeField ] public string OpenAnimationName { get; private set; }
        [ field: SerializeField ] public float CloseSpeed { get; private set; } = 1f;
        [ field: SerializeField ] public bool IsCloseAnimationIsOpenReverse { get; private set; } = true;
        [ field: SerializeField ] public string CloseAnimationName { get; private set; }
    }
}