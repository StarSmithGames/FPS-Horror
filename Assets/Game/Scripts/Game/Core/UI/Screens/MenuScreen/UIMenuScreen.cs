using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.MenuScreen
{
    public sealed class UIMenuScreen : UIViewFade
    {
        [ field: SerializeField ] public UIOptionMenuButton StartButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuButton OptionsButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuButton ExitButton { get; private set; }
    }
}