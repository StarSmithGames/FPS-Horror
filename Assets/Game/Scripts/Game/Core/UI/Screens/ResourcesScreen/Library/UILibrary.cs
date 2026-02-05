using TMPro;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UILibrary : MonoBehaviour
    {
        [ field: SerializeField ] public Transform Content { get; private set; }
        [ field: SerializeField ] public TextMeshProUGUI MainText { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public UILibraryOption OptionPrefab { get; private set; }
    }
}