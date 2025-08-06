using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class SoundsSettings
    {
        [ field: SerializeField ] public FootStepSoundsSettings FootStepSoundsSettings { get; private set; }
    }
}