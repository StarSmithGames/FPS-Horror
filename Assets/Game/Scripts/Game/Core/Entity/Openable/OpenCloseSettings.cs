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
        [ field: SerializeField ] public float HandleDuration { get; private set; } = 0.3f;

        // [ field: SerializeField ] public float OpenSpeed { get; private set; } = 1f;
        // [ field: SerializeField ] public AnimationClip OpenAnimationClip { get; private set; }
        // [ field: SerializeField ] public float CloseSpeed { get; private set; } = 1f;
        // [ field: SerializeField ] public bool IsCloseAnimationIsReverseOpen { get; private set; } = true;
        // [ field: SerializeField ] public AnimationClip CloseAnimationClip { get; private set; }
    }
}