using Game.Managers.InputManager;
using UnityEngine;

namespace Game.Core.World.Systems.InteractionSystem
{
    [ System.Serializable ]
    public sealed class InteractionsSettings
    {
        [ field: SerializeField ] public InputKeyAction InteractAction { get; private set; }
        [ field: SerializeField ] public InputKeyAction InspectAction { get; private set; }
    }
}