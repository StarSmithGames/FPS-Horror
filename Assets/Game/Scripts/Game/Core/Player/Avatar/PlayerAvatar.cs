using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class PlayerAvatar
    {
        [ field: SerializeField ] public PlayerRightHandObject HandRight { get; private set; }
    }
}