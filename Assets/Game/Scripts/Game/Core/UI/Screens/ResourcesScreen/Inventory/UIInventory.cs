using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIInventory : MonoBehaviour
    {
        [ field: SerializeField ] public Transform Content { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public UIInventoryCell CellPrefab { get; private set; }
    }
}