using UnityEngine;

namespace Game.Core.Entity
{
    //Paper, boor or readable things
    public sealed class ExamineItemObject : InspectableEntityObject
    {
        [ field: SerializeField ] public string TextId { get; private set; }
    }
}