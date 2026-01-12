using PuzzlescapeGames.VVM.UI;
using UnityEngine;

namespace Game.Core.UI.MenuScreen
{
    public sealed class UIMenuScreen : UIViewFade
    {
        [ field: SerializeField ] public UIOptionMenuTextButton StartButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuTextButton OptionsButton { get; private set; }
        [ field: SerializeField ] public UIOptionMenuTextButton ExitButton { get; private set; }
    }
}