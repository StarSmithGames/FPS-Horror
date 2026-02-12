using System;
using UnityEngine;

namespace Game.Core.World.JournalSystem
{
    [ Serializable ]
    public sealed class QuestObjective
    {
        [ field: SerializeField ] public string UID { get; set; }
        [ field: SerializeField ] public string TitleId { get; set; }
        [ field: SerializeField ] public string DescriptionId { get; set; }
    }
}