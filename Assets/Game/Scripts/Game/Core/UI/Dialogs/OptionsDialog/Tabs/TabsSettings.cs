using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    [ System.Serializable ]
    public sealed class TabsSettings
    {
        [ field: SerializeField ] public UIOptionToggle OptionTogglePrefab { get; private set; }
        [ field: SerializeField ] public UIOptionLeftRight OptionLeftRightPrefab { get; private set; }
        [ field: SerializeField ] public UIOptionKey OptionKeyPrefab { get; private set; }
    }
}