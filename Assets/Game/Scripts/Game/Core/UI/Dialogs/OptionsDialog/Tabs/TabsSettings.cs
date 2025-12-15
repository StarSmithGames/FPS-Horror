using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    [ System.Serializable ]
    public sealed class TabsSettings
    {
        [ field: SerializeField ] public UIOptionTextToggle OptionTogglePrefab { get; private set; }
        [ field: SerializeField ] public UIOptionLeftRightText OptionLeftRightPrefab { get; private set; }
        [ field: SerializeField ] public UIOptionKey OptionKeyPrefab { get; private set; }
    }
}