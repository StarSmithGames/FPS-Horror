using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class FootStepSoundsSettings
    {
        [ field: SerializeField ] public FootStepSoundsGroup FootStepsCommon { get; private set; } = new();
        [ field: Range( .1f, .95f ) ]
        [ field: SerializeField ] public float FootstepSpeed { get; private set; } = 1f;
    }
}