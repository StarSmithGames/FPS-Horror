using PuzzlescapeGames.VVM.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.OptionsDialog
{
    public sealed class OptionsDialog : UIViewFade
    {
        [ field: SerializeField ] public List< UITab > Tabs { get; private set; } = new();
        [ field: SerializeField ] public Transform Content { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public UIOptionLeftRightSelector OptionLeftRightSelectorPrefab { get; private set; }
    }
}