using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.World.JournalSystem
{
    [ CreateAssetMenu( fileName = "Journal", menuName = "Game/Journal" ) ]
    public sealed class JournalConfig : ScriptableObject
    {
        [ field: SerializeField ] public List< Quest > Quests { get; private set; } = new();
    }
}