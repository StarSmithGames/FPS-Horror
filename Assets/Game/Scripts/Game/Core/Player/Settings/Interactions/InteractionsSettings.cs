using UnityEngine;

namespace Game.Core.Player
{
    [ System.Serializable ]
    public sealed class InteractionsSettings
    {
        [ field: SerializeField ] public float MaxDistance { get; set; }
        [ field: SerializeField ] public float KeyDistance { get; set; }
        
        public float MaxDistanceSquared => MaxDistance * MaxDistance;
        public float KeyDistanceSquared => KeyDistance * KeyDistance;
    }
}