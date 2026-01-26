using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.ContextMenu
{
    public sealed class UIContextMenu : UIViewFade
    {
        [ field: SerializeField ] public RectTransform Root { get; private set; }
        [ field: SerializeField ] public Transform Content { get; private set; }
        [ field: Space ]
        [ field: SerializeField ] public UIContextMenuItem ItemPrefab { get; private set; }
    }
}