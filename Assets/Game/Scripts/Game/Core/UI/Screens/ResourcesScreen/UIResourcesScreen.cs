using PuzzlescapeGames.VVM.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIResourcesScreen : UIViewFade
    {
        [ field: SerializeField ] public List< UIOptionMenuButton > MenuOptions = new();
        [ field: SerializeField ] public UIInventory Inventory { get; private set; }
    }
}