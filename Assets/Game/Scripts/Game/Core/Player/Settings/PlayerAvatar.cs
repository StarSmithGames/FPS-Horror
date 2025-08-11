using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class PlayerAvatar
    {
        [ field: SerializeField ] public Transform HandRight { get; private set; }
    }
}