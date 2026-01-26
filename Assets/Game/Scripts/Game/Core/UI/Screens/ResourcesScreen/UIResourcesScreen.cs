using Game.Core.UI.ContextMenu;
using PuzzlescapeGames.VVM.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.UI.ResourcesScreen
{
    public sealed class UIResourcesScreen : UIViewFade
    {
        public event Action OnBackButtonClicked;
        
        [ field: SerializeField ] public List< UIOptionMenuButton > MenuOptions = new();
        [ field: SerializeField ] public UIInventory Inventory { get; private set; }
        [ field: SerializeField ] public UIContextMenu ContextMenu { get; private set; }

        public void OnBackButtonClick()
        {
            OnBackButtonClicked?.Invoke();
        }
    }
}