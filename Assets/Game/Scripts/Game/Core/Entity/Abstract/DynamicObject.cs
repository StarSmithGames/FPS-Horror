using UnityEngine;

namespace Game.Core.Entity
{
    public abstract class DynamicObject : ObservableObject
    {
        [ field: SerializeField ] public string NameId { get; private set; }
    }
}