using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.World.JournalSystem
{
    [ Serializable ]
    public sealed class Quest
    {
        [ field: SerializeField ] public string UID { get; set; }
        [ field: SerializeField ] public List< QuestObjective > Objectives { get; private set; } = new();
    }
}